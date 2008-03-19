

namespace Amplify.Data.SqlClient
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;

	using Amplify.Linq;


	public class SqlAdapter : Adapter
	{
		private static Hash nativeDatabaseTypes = null;
		private static string primaryKeyType = "uniqueidentifier";
		private System.Data.SqlClient.SqlConnection connection;
		private string connectionString = "";

		public SqlAdapter()
		{
			this.connectionString = this.ConnectionString;
		}

		public SqlAdapter(string connectionString) 
		{
			this.connectionString = connectionString;
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
					nativeDatabaseTypes = Hash.New(
						PrimaryKey	=>		primary,
						@String		=>		Hash.New(Name	=>	"nvarchar",		Limit	=>	255),
						@Guid		=>		Hash.New(Name	=>	"uniqueidentifier"),
						Text 		=>		Hash.New(Name	=>	"ntext"),
						Integer		=>		Hash.New(Name	=>	"int"),
						@Float		=>		Hash.New(Name	=>	"float",		Limit	=>	8),
						@Decimal	=>		Hash.New(Name	=>	"decimal"),
						@DateTime	=>		Hash.New(Name	=>	"datetime"),
						Timestamp	=>		Hash.New(Name	=>	"datetime"),
						Time		=>		Hash.New(Name	=>	"datetime"),
						Date		=>		Hash.New(Name	=>	"datetime"),
						Binary		=>		Hash.New(Name	=>	"image"),
						@Boolean	=>		Hash.New(Name	=>	"bit",			Default => false)
					);
				}
				return nativeDatabaseTypes;
			}
		}

		public override IDbConnection Connect()
		{
			if (this.connection == null || this.connection.State == ConnectionState.Closed)
			{
				this.connection = new System.Data.SqlClient.SqlConnection(this.connectionString);
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

		public IEnumerable<string> GetPrimaryKeys(string tableName)
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
				tableName = tableName.Split(".".ToCharArray()).Last();
			tableName = tableName.Gsub(@"[\[\]]", "");
			List<string> primaryKeys = GetPrimaryKeys(tableName).ToList();
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
					string	type = dr["ColType"].ToString().ToLower(),
							sqlType = "";
					string defaultValue = dr["DefaultValue"].ToString().Gsub("[()\']", "").Match("null", RegexOptions.IgnoreCase) ? "null" : dr["DefaultValue"].ToString();
					if(type.Match("numeric|decimal", RegexOptions.IgnoreCase)) 
						sqlType = string.Format("{0}({1},{2})", type, 
							dr["numeric_precision"], dr["numeric_scale"]);
					else 
						sqlType = string.Format("{0}({1})", type, dr["Length"]);
					
					columns.Add(new SqlColumn(dr["ColName"].ToString(), defaultValue, sqlType, 
						(dr["IsNullable"].ToString() == "YES") , primaryKeys.Contains(dr["ColName"].ToString())));
				}
			}
			return columns;			
		}

		public override void CreateDatabase(string name)
		{
			this.ExecuteNonQuery("CREATE DATABASE {0}", name);
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
					if (!index.Match("primary key"))
					{
						list.Add(new IndexDefinition()
						{
							TableName = tableName,
							Name = dr[0].ToString(),
							IsUnique = dr[1].ToString().Match("unique"),
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

		public override void AddColumn(string tableName, string columnName, string type, params Func<object, object>[] options)
		{
			Hash hash = Hash.New(options);
			string sql = String.Format("ALTER TABLE {0} ADD {1} {2}", QuoteTableName(tableName), QuoteColumnName(columnName), TypeToSql(type, (hash["Limit"] as int?), (hash["Precision"] as int?), (hash["Scale"] as int?)));
			sql += AddColumnOptions(hash);
			this.ExecuteNonQuery(sql);
		}

		public override void RemoveColumn(string tableName, string columnName)
		{
			this.RemoveCheckConstraints(tableName, columnName);
			this.RemoveDefaultConstraint(tableName, columnName);
			this.ExecuteNonQuery("ALTER TABLE [{0}] DROP COLUMN [{1}]".Inject(tableName, columnName));
		}

		public override void RenameColumn(string tableName, string name, string newName)
		{
			this.ExecuteNonQuery(
				string.Format("EXEC sp_rename '{0}.{1}', '{2}'",
					tableName,
					name,
					newName));
		}

		public override void ChangeColumn(string tableName, string name, string type, params Func<object, object>[] options)
		{
			Hash hash = Hash.New(options);
			List<string> commands = new List<string>() {
				string.Format("ALTER TABLE {0} ALTER COLUMN {1} {2}", 
					tableName, name, 
						this.TypeToSql(type, (int?)hash["Limit"], (int?)hash["Precision"], (int?)hash["Scale"]))
			};
			if (this.OptionsIncludeDefault(hash))
			{
				this.RemoveDefaultConstraint(tableName, name);
				commands.Add(
					string.Format("ALTER TABLE {0} ADD CONSTRAINT DF_{0}_{1} DEFAULT {2} FOR {1}",
						tableName, name, this.Quote(hash[@default], (hash["column"] as ColumnDefinition))));
			}
			commands.Each(item => this.ExecuteNonQuery(item));
		}

		public override void RemoveIndex(string tableName, IEnumerable<string> columnNames)
		{
			this.ExecuteNonQuery(
				"DROP INDEX {0}.{1}".Inject(tableName, 
				this.QuoteColumnName(this.IndexName(tableName, columnNames))));
		}

		public void RemoveDefaultConstraint(string tableName, string columnName)
		{
			List<object> list = new List<object>();
			using(IDataReader dr = Select(
				@"SELECT 
						def.name 
					FROM 
						sysobjects def, syscolumns col, sysobjects tab 
					WHERE 
						col.cdefault = def.id and col.name = {1} and tab.name = {0} and col.id = tab.id",
				tableName, columnName)) {

				while(dr.Read()) {
					list.Add(dr[0]);
				}
			}
			foreach (object item in list)
				this.ExecuteNonQuery("ALTER TABLE {0} DROP CONSTRAINT {1}".Inject(tableName, item));
			
		}

		public void RemoveCheckConstraints(string tableName, string columnName)
		{
			List<object> list = new List<object>();
			using(IDataReader dr = this.Select(
				"SELECT CONSTRAINT_NAME FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE where TABLE_NAME = '{0}' and COLUMN_NAME = '{1}'".Inject(tableName, columnName))){
				while(dr.Read()) {
					list.Add(dr[0]);
				}
			}
			foreach(object item in list)
				this.ExecuteNonQuery("ALTER TABLE {0} DROP CONSTRAINT {1}".Inject(tableName, item));
		}

		public override object Insert(string sql, params object[] values)
		{
			sql += " SELECT @@IDENTITY AS ReturnValue";
			return this.ExecuteScalar(sql, values);
		}

		public override int Update(string sql, params object[] values)
		{
			sql += " SELECT @@ROWCOUNT AS ReturnValue";
			return this.ExecuteNonQuery(sql, values);
		}

		public override int Delete(string sql, params object[] values)
		{
			return this.Update(sql, values);
		}

		protected override string AddLimit(string sql, Options options)
		{
			if (options.Limit != null && options.Offset != null)
			{
				string query = sql.Gsub(@"^\s*SELECT(\s+DISTINCT)?",
							"SELECT {0} TOP 1000000000".Inject(options.Distinct ? "DISTINCT" : ""),
							RegexOptions.IgnoreCase);
				int totalrows = 0;
				using(IDataReader dr = ExecuteReader(
					"SELECT count(*) as TotalRows from {0} tally ".Inject(query))){
						totalrows =	dr.GetInt32(0);
				}

				if ((options.Limit + options.Offset) >= totalrows)
					options.Limit = (totalrows - options.Offset >= 0) ? (totalrows - options.Offset) : 0;

				sql = sql.Gsub(@"^\s*SELECT(\s+DISTINCT)?", 
					"SELECT * FROM (SELECT TOP {0} * FROM (SELECT {1} TOP {2} ".Inject(
						options.Limit, 
						options.Distinct ? "DISTINCT" : "", 
						options.Limit + options.Offset), 
					RegexOptions.IgnoreCase);
				sql += ") as tmp1";
				if (!string.IsNullOrEmpty(options.Order))
				{
					sql += "ORDER BY {0}) as tmp2 ORDER BY {1}".Inject(ChangeOrder(options.Order), options.Order);
				}
				else
				{
					sql += ") as tmp2";
				}

				return sql;
			}
			else if (options.Limit != null && !sql.Match(@"^\s*SELECT (@@|COUNT\()", RegexOptions.IgnoreCase))
			{
				return sql.Gsub(@"^\s*SELECT(\s+DISTINCT)?", 
					"SELECT {0} TOP {1}".Inject(options.Distinct ? "DISTINCT" : "", options.Limit), 
					RegexOptions.IgnoreCase);
			}
			return sql;
		}

		private string ChangeOrder(string order)
		{
			return order.Split(",").Each(delegate(string item)
			{
				if(item.Match(@"\bASC\b",RegexOptions.IgnoreCase))
					item = item.Gsub(@"\bASC\b", "DESC", RegexOptions.IgnoreCase);
				else if(item.Match(@"\bDESC\b", RegexOptions.IgnoreCase))
					item = item.Gsub(@"\bDESC\b", "ASC", RegexOptions.IgnoreCase);
			}).Join(",");
		}

	}
}
