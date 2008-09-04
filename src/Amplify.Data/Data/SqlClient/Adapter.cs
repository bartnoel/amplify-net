﻿

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
		private static string primaryKeyType = "integer";
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
					string identity = ApplicationContext.IsTesting ? "" : "IDENTITY(1, 1)";

					string primary = string.Format("int NOT NULL {0} PRIMARY KEY", identity);
					
					

					nativeDatabaseTypes = new Hash() {
						{ "primarykey",			primary															},
						{ "primarykeyint",		primary															},
						{ "primarykeyguid",		"uniqueidentifier NOT NULL PRIMARY KEY DEFAULT (newid()) "		},
						{ "ansistring",		new Hash() { { "name", "varchar"}, {"limit", 255}}},
						{ "ansitext",		new Hash() { { "name", "text"} }},
						{ "string",			new Hash() { { "name", "nvarchar"} ,			{ "limit", 255 }}},
						{ "guid",			new Hash() { { "name", "uniqueidentifier" }						}},
						{ "text", 			new Hash() { { "name", "ntext" }								}},
						{ "integer",		new Hash() { { "name", "int"}									}},
						{ "int16",			new Hash() { { "name", "tinyint"} }},
						{ "int32",			new Hash() { { "name", "int"} }},
						{ "int64",			new Hash() { { "name", "bigint" }}},
						{ "uint16",			new Hash() { { "name", "tinyint"}, { "check", "{0} > -1" } }},
						{ "uint32",			new Hash() { { "name", "int"}, { "check", "{0} > -1" } }},
						{ "uint64",			new Hash() { { "name", "bigint"}, { "check", "{0} > -1" } }},
						{ "float",			new Hash() { { "name", "float"},				{ "limit",8 }	}},
						{ "decimal",		new Hash() { { "name", "decimal"}								}},
						{ "varnumeric",		new Hash() { { "name", "numeric"}								}},
						{ "datetime",		new Hash() { { "name", "datetime"}								}},
						{ "timestamp",		new Hash() { { "name", "datetime"} }},
						{ "rowversion",		new	Hash() { { "name", "rowversion"}							}},
						{ "time",			new Hash() { { "name", "datetime"}								}},
						{ "date",			new Hash() { { "name", "datetime"}								}},
						{ "binary",			new Hash() { { "name", "binary"}								}},				
						{ "image",			new Hash() { { "name", "image"}								}},
						{ "boolean",		new Hash() { { "name", "bit"},					{"default",false}}},
						{ "byte",			new Hash() { { "name", "bit"},				    }},
						{ "single",			new Hash() { { "name", "smallint"},				}},
						{ "double",			new Hash() { { "name", "double" }}},
						{ "currency",		new Hash() { { "name", "money" }}},
						{ "xml",			new Hash() { { "name", "xml" }}}
					};
				}
				return nativeDatabaseTypes;
			}
		}


		public ColumnDefinition MapColumn(DbTypes dbtype)
		{
			ColumnDefinition column = new ColumnDefinition(this);

			switch (dbtype)
			{
				case DbTypes.AnsiString:
					column.Type = "varchar";
					if (!column.Limit.HasValue)
						column.Limit = 255;
					break;
				case DbTypes.AnsiText:
					column.Type = "text";
					break;
				case DbTypes.Binary:
					column.Type = "varbinary";
					if (!column.Limit.HasValue)
						column.Limit = 255;
					break;
				case DbTypes.Blob:
					column.Type = "image";
					break;
				case DbTypes.Boolean:
					column.Type = "bit";
					break;
				case DbTypes.Byte:
					column.Type = "bit";
					break;
				case DbTypes.Currency:
					column.Type = "money";
					break;
				case DbTypes.Date:
				case DbTypes.DateTime:
				case DbTypes.DateTime2:
				case DbTypes.DateTimeOffset:
				case DbTypes.TimeStamp:
				case DbTypes.Time:
					column.Type = "datetime";
					break;
				case DbTypes.Decimal:
					column.Type = "decimal";
					if (!column.Scale.HasValue)
						column.Scale = 2;
					if (!column.Precision.HasValue)
						column.Precision = 18;
					break;
				case DbTypes.Double:
					column.Type = "decimal";
					if (!column.Scale.HasValue)
						column.Scale = 2;
					if (!column.Precision.HasValue)
						column.Precision = 18;
			}

			return column;
		}

		public DbTypes MapDbType(ColumnDefinition columDefintion)
		{
			return DbTypes.AnsiString;
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

		public override string[] GetDatabases(bool usePrimary)
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

		public override IEnumerable<string> GetColumnNames(string tableName)
		{
			List<string> columns = new List<string>();

			if (string.IsNullOrEmpty(tableName))
				return columns;

			tableName = ParseTableName(tableName);

			using (IDataReader dr = this.Select(@"
					SELECT DISTINCT 
						c.COLUMN_NAME as [name]
					FROM 
						INFORMATION_SCHEMA.COLUMNS c 
					WHERE 
						c.TABLE_NAME = {0} 
			", tableName))
			{
				while (dr.Read())
				{
					columns.Add(dr.GetString(0));
				}
			}
			return columns;
		}

		private static string ParseTableName(string tableName)
		{
			if (tableName.Contains("."))
			{
				string[] parts = tableName.Split(".".ToCharArray());
				tableName = parts[parts.Length - 1];
			}

			tableName = StringUtil.Gsub(tableName, @"[\[\]]", "");
			return tableName;
		}

		public override IEnumerable<ColumnDefinition> GetColumns(string tableName)
		{
			List<ColumnDefinition> columns = new List<ColumnDefinition>();

			if (string.IsNullOrEmpty(tableName))
				return columns;

			tableName = ParseTableName(tableName);

			using (IDataReader dr = this.Select(@"
			  SELECT 
				cols.COLUMN_NAME as [name],  
				cols.COLUMN_DEFAULT as [default],
				cols.NUMERIC_SCALE as [scale],
				cols.NUMERIC_PRECISION as [precision], 
				cols.DATA_TYPE as [type], 
				cols.IS_NULLABLE As [null],  
				COL_LENGTH(cols.TABLE_NAME, cols.COLUMN_NAME) as [limit],  
				COLUMNPROPERTY(OBJECT_ID(cols.TABLE_NAME), cols.COLUMN_NAME, 'IsIdentity') as [identity],  
			FROM 
				INFORMATION_SCHEMA.COLUMNS cols 
			WHERE 
				cols.TABLE_NAME = {0} 
			", tableName))
			{
				while(dr.Read())
				{

					string sqlType = dr["type"].ToString().ToLower();

					ColumnDefinition column = new ColumnDefinition(this)
					{
						Name = dr["name"].ToString(),
						Limit = dr.GetInt32(dr.GetOrdinal("limit")),
						Type = this.SimplifiedType(sqlType)
					};

					if(StringUtil.IsMatch(column.Type, "(numeric|decimal|number)", RegexOptions.IgnoreCase))
					{
						column.Scale = dr.GetInt32(dr.GetOrdinal("scale"));
						column.Precision = dr.GetInt32(dr.GetOrdinal("precision"));
					}
				    

					string value = StringUtil.Gsub(dr["default"].ToString(), "[()\']", "");
					bool isMatch = StringUtil.IsMatch(value, "null", RegexOptions.IgnoreCase);
					column.Default = isMatch ? null : dr["default"].ToString();

					StringUtil.IsMatch(sqlType, "text|ntext|image", RegexOptions.IgnoreCase);

					
					columns.Add(column);

					using(IDataReader cdr = this.Select(string.Format(@"
									SELECT DISTINCT
										   cu.column_name AS [column name],
										   tc.constraint_name AS [name],
										   tc.constraint_type AS [type],
										  
										 CASE tc.is_deferrable WHEN 'NO' THEN 0 ELSE 1 END AS is_deferrable,
										 CASE tc.initially_deferred WHEN 'NO' THEN 0 ELSE 1 END AS is_deferred,
										   cc.check_clause AS [check],
										   rc.delete_rule AS [on_delete],
										   rc.update_rule AS [on_update],
										   rc.match_option AS [match_type],
										   rcu.table_name  AS [reference_table], 
										   rcu.column_name AS [reference_column]

									  FROM INFORMATION_SCHEMA.COLUMNS c
										 
									  LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
										on  tc.table_name = c.table_name
									  LEFT OUTER JOIN INFORMATION_SCHEMA.CHECK_CONSTRAINTS cc
										on  cc.constraint_name = tc.constraint_name 
									  LEFT OUTER JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc 
										ON	rc.constraint_schema = tc.constraint_schema AND
											rc.constraint_catalog = tc.constraint_catalog AND 
											rc.constraint_name = tc.constraint_name
									  LEFT OUTER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE cu
										ON  cu.constraint_name = tc.constraint_name
									  
									  LEFT OUTER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE rcu
										ON rc.unique_constraint_schema = cu.constraint_schema 
										AND rc.unique_constraint_catalog = cu.constraint_catalog 
										AND rc.unique_constraint_name = cu.constraint_name 
									 
									 WHERE tc.constraint_catalog = DB_NAME()
									   AND c.table_name = '{0}' AND cu.column_name = '{1}'
									   
									 ORDER BY [type]", tableName, column.Name))) 
					{
						while (cdr.Read())
						{
							switch (cdr.GetString(1).Trim())
							{
								case "PRIMARY KEY":
									column.IsPrimaryKey = true;
									if (column.Type.ToLower() == "integer")
										column.Identity = " IDENTITY(1,1) ";
									break;
								case "UNIQUE":
									column.IsUnique = true;
									break;
								case "CHECK":
									column.Checks.Add(cdr.GetString(5));
									break;
								case "FOREIGN KEY":
									column.ForeignKey(cdr.GetString(9), cdr.GetString(10),
										 (ConstraintDeleteAction)Enum.Parse(typeof(ConstraintDeleteAction), cdr.GetString(6)),
										 (ConstraintUpdateAction)Enum.Parse(typeof(ConstraintUpdateAction), cdr.GetString(7))
									);
								
									break;
							}
						}
					}
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
					filename = connectionString.AttachFileDbFilename.ToLower()
						.Replace("|datadirectory|", ApplicationContext.DataDirectory);
			}

			if (connectionString.IsUserInstance)
				connectionString.IsUserInstance = false;

			connectionString.Database = "master";
			connectionString.AttachFileDbFilename = "";

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

		public override void DropDatabase(string databaseName)
		{
			string currentConnectionString = this.ConnectionString;

			SqlConnectionString connectionString = new SqlConnectionString(this.ConnectionString);

			if (string.IsNullOrEmpty(databaseName))
				databaseName = connectionString.Database;

			if (connectionString.IsUserInstance)
				connectionString.IsUserInstance = false;

			connectionString.Database = "master";
			connectionString.AttachFileDbFilename = "";

			using (SqlConnection connection = new SqlConnection(connectionString.ConnectionString))
			{
				connection.Open();
				IDbCommand command = connection.CreateCommand();
				command.CommandType = CommandType.Text;

				command.CommandText = string.Format("ALTER DATABASE {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE; \n  DROP DATABASE {0}", databaseName);
				command.ExecuteNonQuery();
			}
		}

		public override void RecreateDatabase()
		{
			this.RecreateDatabase(null);
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
				TypeToSql(type, (options["limit"] as int?), (options["precision"] as int?), (options["scale"] as int?)));
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
					this.TypeToSql(type, (int?)options["limit"], 
						(int?)options["precision"], (int?)options["scale"]))
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

		public override void DropDatabase()
		{
			this.DropDatabase(null);
		}

		public override List<ForeignKeyDefinition> GetForeignKeys(string primaryTableName)
		{
			return this.GetForeignKeys(primaryTableName, false);
		}

		public override List<ForeignKeyDefinition> GetForeignKeys(string tableName, bool isForeign)
		{
			List<ForeignKeyDefinition> list = new List<ForeignKeyDefinition>();
			string query = (isForeign) ? "EXEC sp_fkeys @fktable_name = " : "EXEC sp_fkeys @pktable_name = ";

			using (IDataReader dr = ExecuteReader(query, this.QuoteTableName(tableName)))
			{
				while (dr.Read())
				{
					int update = dr.GetInt32(dr.GetOrdinal("UPDATE_RULE"));
					int delete = dr.GetInt32(dr.GetOrdinal("DELETE_RULE"));
					ConstraintDeleteAction onDelete = ConstraintDeleteAction.None;
					ConstraintUpdateAction onUpdate = ConstraintUpdateAction.None;

					switch (update)
					{
						case 0:
							onUpdate = ConstraintUpdateAction.Cascade;
							break;
						case 1:
							onUpdate = ConstraintUpdateAction.None;
							break;
					}

					switch (delete)
					{
						case 0:
							onDelete = ConstraintDeleteAction.Cascade;
							break;
						case 1:
							onDelete = ConstraintDeleteAction.None;
							break;
					}
					

					list.Add(new ForeignKeyDefinition()
					{
						PrimaryTableName = dr.GetString(dr.GetOrdinal("PKTABLE_NAME")),
						PrimaryColumnName = dr.GetString(dr.GetOrdinal("PKCOLUMN_NAME")),
						ReferenceTableName = dr.GetString(dr.GetOrdinal("FKTABLE_NAME")),
						ReferenceColumnNames = dr.GetString(dr.GetOrdinal("FKCOLUMN_NAME")),
						Name = dr.GetString(dr.GetOrdinal("FK_NAME")),
						OnDelete = onDelete,
						OnUpdate = onUpdate
					});

				}
			}
			return list;
		}
	}
}
