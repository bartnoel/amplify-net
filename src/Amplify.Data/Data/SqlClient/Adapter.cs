

namespace Amplify.Data.SqlClient
{
	using System;
	using System.Collections.Generic;
	using System.Data.SqlClient;
	using System.Data;
	
	using System.Text;
	using System.Text.RegularExpressions;

	using Amplify.Linq;


	public class SqlAdapter : Adapter
	{
		private static Hash nativeDatabaseTypes = null;
		private static string primaryKeyType = "uniqueidentifier";
		private System.Data.SqlClient.SqlConnection connection;
		

		public SqlAdapter()
		{
			this.ConnectionString = "";
		}

		public SqlAdapter(string connectionString) 
		{
			this.ConnectionString = connectionString;
		}

		public override System.Data.Common.DbConnectionStringBuilder GetBuilder()
		{
			return new System.Data.SqlClient.SqlConnectionStringBuilder(this.ConnectionString);
		}

		public override string PrimaryKeyType
		{
			get { return primaryKeyType; }
		}

		public override Hash NativeDatabaseTypes
		{
			get {
				if (nativeDatabaseTypes == null)
				{
					string primary = (this.PrimaryKeyType == integer) ? 
						"int NOT NULL IDENTITY(1, 1) PRIMARY KEY" : 
						"uniqueidentifier NOT NULL PRIMARY KEY";

					nativeDatabaseTypes = new Hash() {
						{"PrimaryKey",		primary															},
						{ "String",			new Hash() { { "Name", "nvarchar"} ,			{ "Limit", 255 }}},
						{ "Guid",			new Hash() { { "Name", "uniqueidentifier" }						}},
						{ "Text", 			new Hash() { { "Name", "ntext" }								}},
						{ "Integer",		new Hash() { { "Name", "int"}									}},
						{ "Float",			new Hash() { { "Name", "float"},				{ "Limit",8 }	}},
						{ "Decimal",		new Hash() { { "Name", "decimal"}								}},
						{ "DateTime",		new Hash() { { "Name", "datetime"}								}},
						{ "Timestamp",		new	Hash() { { "Name", "timestamp"}								}},
						{ "Time",			new Hash() { { "Name", "datetime"}								}},
						{ "Date",			new Hash() { { "Name", "datetime"}								}},
						{ "Binary",			new Hash() { { "Name", "binary"}								}},				
						{ "Image",			new Hash() { { "Name", "image"}								}},
						{ "Boolean",		new Hash() { { "Name", "bit"},					{"Default",false}}}
					};
				}
				return nativeDatabaseTypes;
			}
		}

		public override IDbConnection Connect()
		{
			if (this.connection == null || this.connection.State == ConnectionState.Closed)
			{
				this.connection = new System.Data.SqlClient.SqlConnection(this.ConnectionString);
				this.connection.Open();
			}
			return this.connection;
		}

		public override string TypeToSql(string type, int? limit, int? precision, int? scale)
		{
			if (type == integer)
			{
				if (limit == null || limit == 4)
					return integer;
				else if (limit < 4)
					return "smallint";
				else
					return "bigint";
			}
			return base.TypeToSql(type, limit, precision, scale);
		}

		public override string[] GetDatabases()
		{
			List<string> databases = new List<string>();
			using(IDataReader dr = this.ExecuteReader("EXEC sp_databases")) {
				while (dr.Read())
				{
					databases.Add(dr["DATABASE_NAME"].ToString());
				}
			}
			return databases.ToArray();
		}

		public override IEnumerable<string> GetPrimaryKeys(string tableName)
		{
			List<string> primaryKeys = new List<string>();
			using(IDataReader dr = this.Select(@"SELECT c.name
					FROM sysindexes i
					JOIN sysobjects o ON i.id = o.id
					JOIN sysobjects pk ON i.name = pk.name
					AND pk.parent_obj = i.id
					AND pk.xtype = 'PK'
					JOIN sysindexkeys ik on i.id = ik.id
					and i.indid = ik.indid
					JOIN syscolumns c ON ik.id = c.id
					AND ik.colid = c.colid
					WHERE o.name = {0}
					order by ik.keyno", tableName)) 
			{
				while (dr.Read())
				{
					primaryKeys.Add(dr.GetString(0));
				}
			}
			return primaryKeys;
		}

		public override IEnumerable<Column> GetColumns(string tableName)
		{
			List<Column> columns = new List<Column>();

			if (string.IsNullOrEmpty(tableName))
				return columns;

			if (tableName.Contains("."))
			{
				string[] parts = tableName.Split(".".ToCharArray());
				tableName = parts[parts.Length - 1];
			}

			tableName = StringUtil.Gsub(tableName, @"[\[\]]", "");
			List<string> primaryKeys = new List<string>(GetPrimaryKeys(tableName));

			using (IDataReader dr = this.Select(@"
			  SELECT 
				cols.COLUMN_NAME as ColName,  
				cols.COLUMN_DEFAULT as DefaultValue,
				cols.NUMERIC_SCALE as numeric_scale,
				cols.NUMERIC_PRECISION as numeric_precision, 
				cols.DATA_TYPE as ColType, 
				cols.IS_NULLABLE As IsNullable,  
				COL_LENGTH(cols.TABLE_NAME, cols.COLUMN_NAME) as Length,  
				COLUMNPROPERTY(OBJECT_ID(cols.TABLE_NAME), cols.COLUMN_NAME, 'IsIdentity') as IsIdentity,  
				cols.NUMERIC_SCALE as Scale 
			FROM 
				INFORMATION_SCHEMA.COLUMNS cols 
			WHERE 
				cols.TABLE_NAME = {0} 
			", tableName))
			{
				while(dr.Read())
				{
					//SqlColumn column = new SqlColumn(
					string type = dr["ColType"].ToString().ToLower(),
							sqlType = "";

					string value = StringUtil.Gsub(dr["DefaultValue"].ToString(), "[()\']", "");
					bool isMatch = StringUtil.IsMatch(value, "null", RegexOptions.IgnoreCase);
					string defaultValue = isMatch ? "null" : dr["DefaultValue"].ToString();

					if (StringUtil.IsMatch(type, "numeric|decimal", RegexOptions.IgnoreCase))
						sqlType = string.Format("{0}({1},{2})", type,
							dr["numeric_precision"], dr["numeric_scale"]);
					else
						sqlType = string.Format("{0}({1})", type, dr["Length"]);

					columns.Add(new SqlColumn(dr["ColName"].ToString(), defaultValue, sqlType,
						(dr["IsNullable"].ToString() == "YES"), primaryKeys.Contains(dr["ColName"].ToString())));
				}
			}
			return columns;			
		}

		public override void CreateDatabase(string databaseName)
		{
			string currentConnectionString = this.ConnectionString;

			SqlConnectionString connectionString = new SqlConnectionString(this.ConnectionString);
			string filename = "";

			if (string.IsNullOrEmpty(databaseName))
			{
				databaseName = connectionString.Database;


				if (!string.IsNullOrEmpty(connectionString.AttachFileDbFilename))
					filename = connectionString.AttachFileDbFilename;
			}

			if (connectionString.IsUserInstance)
				connectionString.IsUserInstance = false;

			connectionString.Database = "master";
			connectionString.AttachFileDbFilename = null;

			using (SqlConnection connection = new SqlConnection(connectionString.ConnectionString))
			{
				connection.Open();
				IDbCommand command = connection.CreateCommand();
				command.CommandType = CommandType.Text;
				if (!string.IsNullOrEmpty(filename))
					command.CommandText = string.Format("CREATE DATABASE {0} ON (NAME = '{0}', FILENAME ='{1}')", databaseName, filename);
				else
					command.CommandText = string.Format("CREATE DATABASE {0}", databaseName);
				command.ExecuteNonQuery();
			}
		}

		public override void DropDatabase(string name)
		{
			this.ExecuteNonQuery("DROP DATABASE {0}", name);
		}

		public override void RecreateDatabase(string name)
		{
			this.DropDatabase(name);
			this.CreateDatabase(name);
		}

		public override  string CurrentDatabase()
		{
			return this.ExecuteScalar("select DB_NAME()").ToString();
		}

		public override List<string> GetTableNames()
		{
			List<string> tables = new List<string>();
			using (IDataReader dr = ExecuteReader("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'"))
			{
				while (dr.Read())
				{
					string table = dr["TABLE_NAME"].ToString();
					if (table != "dtproperties")
						tables.Add(table);
				}
			}
			return tables;
		}

		public override List<IndexDefinition> GetIndexes(string tableName)
		{
			List<IndexDefinition> list = new List<IndexDefinition>();
			using (IDataReader dr = ExecuteReader("EXEC sp_helpindex {0}", tableName))
			{
				while (dr.Read())
				{
					string index = dr[1].ToString();
					if (!StringUtil.IsMatch(index, "primary key"))
					{
						list.Add(new IndexDefinition()
						{
							TableName = tableName,
							Name = dr[0].ToString(),
							IsUnique = StringUtil.IsMatch(dr[1].ToString(), "unique"),
							Columns = new List<string>(dr[2].ToString().Split(", ".ToCharArray()))
						});
					}
				}
			}
			return list;
		}

		public override void RenameTable(string name, string newName)
		{
			this.ExecuteNonQuery("EXEC sp_rename {0}, {1}", name, newName);
		}

		public override void AddColumn(string tableName, string columnName, string type, Hash options)
		{
			string sql = String.Format("ALTER TABLE {0} ADD {1} {2}", 
				QuoteTableName(tableName), 
				QuoteColumnName(columnName), 
				TypeToSql(type, (options["Limit"] as int?), (options["Precision"] as int?), (options["Scale"] as int?)));
			sql += AddColumnOptions(options);
			this.ExecuteNonQuery(sql);
		}

		public override void RemoveColumn(string tableName, string columnName)
		{
			this.RemoveCheckConstraints(tableName, columnName);
			this.RemoveDefaultConstraint(tableName, columnName);
			this.ExecuteNonQuery(string.Format("ALTER TABLE [{0}] DROP COLUMN [{1}]", tableName, columnName));
		}

		public override void RenameColumn(string tableName, string name, string newName)
		{
			this.ExecuteNonQuery(
				string.Format("EXEC sp_rename '{0}.{1}', '{2}'",
					tableName,
					name,
					newName));
		}

#if LINQ
		public override void ChangeColumn(string tableName, string name, string type,  params Func<object, object>[] options) 
		{
			this.ChangeColumn(tableName, name, type, Hash.New(options));
		}
#endif

		public override void ChangeColumn(string tableName, string name, string type, ColumnOptions options)
		{
			this.ChangeColumn(tableName, name, type, options.ToHash());
		}

		public override void ChangeColumn(string tableName, string name, string type, Hash options)
		{
		
			List<string> commands = new List<string>() {
				string.Format("ALTER TABLE {0} ALTER COLUMN {1} {2}", 
					tableName, 
					name, 	
					this.TypeToSql(type, (int?)options["Limit"], 
						(int?)options["Precision"], (int?)options["Scale"]))
			};
			
			if (this.OptionsIncludeDefault(options))
			{
				this.RemoveDefaultConstraint(tableName, name);
				commands.Add(
					string.Format("ALTER TABLE {0} ADD CONSTRAINT DF_{0}_{1} DEFAULT {2} FOR {1}",
						tableName, name, this.Quote(options[@default], (options["Column"] as ColumnDefinition))));
			}

			foreach (string command in commands)
				this.ExecuteNonQuery(command);
		}

		public override void RemoveIndex(string tableName, IEnumerable<string> columnNames)
		{
			string indexName = this.IndexName(tableName, columnNames);

			this.ExecuteNonQuery(
				string.Format(
					"DROP INDEX {0}.{1}",
					tableName, 
					this.QuoteColumnName(indexName))
			);
		}

		public void RemoveDefaultConstraint(string tableName, string columnName)
		{
			List<object> list = new List<object>();

			string query = string.Format(@"SELECT 
						def.name 
					FROM 
						sysobjects def, syscolumns col, sysobjects tab 
					WHERE 
						col.cdefault = def.id and col.name = '{1}' and tab.name = '{0}' and col.id = tab.id",
					tableName, columnName);

			using(IDataReader dr = Select(query)) 
			{
				while(dr.Read()) 
				{
					list.Add(dr[0]);
				}
			}

			foreach (object item in list)
				this.ExecuteNonQuery(string.Format("ALTER TABLE {0} DROP CONSTRAINT {1}",tableName, item));
		}

		public void RemoveCheckConstraints(string tableName, string columnName)
		{
			List<object> list = new List<object>();
			string query = string.Format(
				"SELECT CONSTRAINT_NAME FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE where TABLE_NAME = '{0}' and COLUMN_NAME = '{1}'",
				tableName,
				columnName);

			using(IDataReader dr = this.Select(query))
			{
				while(dr.Read()) 
				{
					list.Add(dr[0]);
				}
			}

			foreach(object item in list)
				this.ExecuteNonQuery(string.Format("ALTER TABLE {0} DROP CONSTRAINT {1}", tableName, item));
		}

		


		

		protected override string AddLimit(string sql, IOptions options)
		{
			if (options.Limit != null && options.Offset != null)
			{
				int totalrows = 0;
				string query = StringUtil.Gsub(
							sql, 
							@"^\s*SELECT(\s+DISTINCT)?",
							string.Format("SELECT {0} TOP 1000000000",options.IsDistinct ? "DISTINCT" : ""),
							RegexOptions.IgnoreCase
						);
				
				using(IDataReader dr = ExecuteReader(
					string.Format("SELECT count(*) as TotalRows from {0} tally ", query)))
				{
					totalrows =	dr.GetInt32(0);
				}

				if ((options.Limit + options.Offset) >= totalrows)
					options.Limit = (totalrows - options.Offset >= 0) ? (totalrows - options.Offset) : 0;

				sql = StringUtil.Gsub(
						sql, 
						@"^\s*SELECT(\s+DISTINCT)?",
						string.Format("SELECT * FROM (SELECT TOP {0} * FROM (SELECT {1} TOP {2} ",
							options.Limit, 
							options.IsDistinct ? "DISTINCT" : "", 
							options.Limit + options.Offset
						), 
						RegexOptions.IgnoreCase);

				sql += ") as tmp1";

				if (!string.IsNullOrEmpty(options.Order))
					sql += string.Format("ORDER BY {0}) as tmp2 ORDER BY {1}", ChangeOrder(options.Order), options.Order);
				else
					sql += ") as tmp2";

				return sql;
			}
			else if (options.Limit != null && !StringUtil.IsMatch(sql, @"^\s*SELECT (@@|COUNT\()", RegexOptions.IgnoreCase))
			{
				return  StringUtil.Gsub(
						sql, 
						@"^\s*SELECT(\s+DISTINCT)?", 
						string.Format("SELECT {0} TOP {1}", options.IsDistinct ? "DISTINCT" : "", options.Limit), 
						RegexOptions.IgnoreCase
					);
			}
			return sql;
		}

		private string ChangeOrder(string order)
		{
			string collect = "";

			foreach (string item in StringUtil.Split(order, ","))
			{
				if (StringUtil.IsMatch(item, @"\bASC\b", RegexOptions.IgnoreCase))
					collect += StringUtil.Gsub(item, @"\bASC\b", "DESC", RegexOptions.IgnoreCase);
				else if (StringUtil.IsMatch(item, @"\bDESC\b", RegexOptions.IgnoreCase))
					collect += StringUtil.Gsub(item, @"\bDESC\b", "ASC", RegexOptions.IgnoreCase);
				else
					collect += item;

				collect += ",";
			}

			return StringUtil.TrimEnd(collect, ",");
		}




		public override void CreateDatabase()
		{
			this.CreateDatabase(null);
		}

		public override void DeleteDatabase()
		{
			
		}
	}
}
