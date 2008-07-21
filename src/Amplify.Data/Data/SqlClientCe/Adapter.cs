

namespace Amplify.Data.SqlClientCe
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	
	using System.Text;
	using System.Text.RegularExpressions;

	using Amplify.Linq;


	public class SqlAdapter : SqlClient.SqlAdapter
	{
		private static Hash nativeDatabaseTypes = null;
		private static string primaryKeyType = "uniqueidentifier";
		private System.Data.SqlServerCe.SqlCeConnection connection;
		

		public SqlAdapter()
		{
			this.ConnectionString = "";
		}

		public SqlAdapter(string connectionString) 
		{
			this.ConnectionString = connectionString;
		}

		

		public override IDbConnection Connect()
		{
			if (this.connection == null || this.connection.State == ConnectionState.Closed)
			{
				this.connection = new System.Data.SqlServerCe.SqlCeConnection(this.ConnectionString);
				this.connection.Open();
			}
			return this.connection;
		}

		

		public override string[] GetDatabases()
		{
			var builder = this.GetBuilder();
			builder.ConnectionString = this.ConnectionString;
			string database = (builder["Data Source"] as string);
			return string.IsNullOrEmpty(database) ? new string[] { } : new string[] { database };
		}

		public override IEnumerable<string> GetPrimaryKeys(string tableName)
		{
			List<string> primaryKeys = new List<string>();
			using (IDataReader dr = this.Select(
							@"SELECT  COLUMN_NAME 
							FROM INFORMATION_SCHEMA.INDEXES 
							WHERE TABLE_NAME = {0} AND PRIMARY_KEY = 1 ", tableName)) 
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
				tableName = parts[parts.Length -1];
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
				cols.CHARACTER_MAXIMUM_LENGTH as Length,  
				(cols.AUTOINC_INCREMENT) as IsIdentity,  
				cols.NUMERIC_SCALE as Scale 
			FROM 
				INFORMATION_SCHEMA.COLUMNS cols 
			WHERE 
				cols.TABLE_NAME = {0} 
			", tableName))
			{
				while(dr.Read())
				{
					//SqlColumn column = new SqlColumn(,
					string	type = dr["ColType"].ToString().ToLower(),
							sqlType = "";

					string value = StringUtil.Gsub(dr["DefaultValue"].ToString(), "[()\']", "");
					bool isMatch = StringUtil.IsMatch(value,"null", RegexOptions.IgnoreCase);
					string defaultValue = isMatch	? "null" : dr["DefaultValue"].ToString();

					if(StringUtil.IsMatch(type, "numeric|decimal", RegexOptions.IgnoreCase)) 
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

	

		
		public override  string CurrentDatabase()
		{
			throw new NotImplementedException();
		}

		public override List<string> GetTableNames()
		{
			List<string> tables = new List<string>();
			using (IDataReader dr = ExecuteReader("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'TABLE'"))
			{
				while (dr.Read())
				{
					string table = dr["TABLE_NAME"].ToString();
					tables.Add(table);
				}
			}
			return tables;
		}

		public override List<IndexDefinition> GetIndexes(string tableName)
		{
			List<IndexDefinition> list = new List<IndexDefinition>();
			using (IDataReader dr = ExecuteReader(@"SELECT COLUMN_NAME, UNIQUE 
									FROM INFORMATION_SCHEMA.INDEXES 
									WHERE TABLE_NAME = {0} AND PRIMARY_KEY = 0", tableName))
			{
				while (dr.Read())
				{
					list.Add(new IndexDefinition()
					{
						TableName = tableName,
						Name = dr[0].ToString(),
						IsUnique = dr.GetBoolean(1),
						Columns = new List<string>(dr[2].ToString().Split(", ".ToCharArray()))
					});
					
				}
			}
			return list;
		}

		/// <summary>
		/// Overridden. Not Implemented.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="newName"></param>
		public override void RenameTable(string name, string newName)
		{
			throw new NotImplementedException();
		}

		

	
		/// <summary>
		/// Overridden Not Implemented
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="name"></param>
		/// <param name="newName"></param>
		public override void RenameColumn(string tableName, string name, string newName)
		{
			throw new NotImplementedException();
		}

		

	

		public void RemoveDefaultConstraint(string tableName, string columnName)
		{
			/*
			List<object> list = new List<object>();
			using(IDataReader dr = Select(
				@"",
				tableName, columnName)) {

				while(dr.Read()) {
					list.Add(dr[0]);
				}
			}
			foreach (object item in list)
				this.ExecuteNonQuery("ALTER TABLE {0} DROP CONSTRAINT {1}".Fuse(tableName, item));*/
			
		}

		protected override string AddInsertParameters(string[] columns, object[] values, string sql, IDbCommand command)
		{
			string valueString = "";
			for (int i = 0; i < values.Length; i++)
			{
				string name = columns[i];
				object value = values[i];
				IDataParameter param = command.CreateParameter();
				
				param.Value = value;
				param.ParameterName = name;
				command.Parameters.Add(param);
				sql += name + ",";
				valueString +=  "?,";
			}

			char delimiter = Char.Parse(",");
			sql = sql.TrimEnd(delimiter) + ") VALUES (";
			sql += valueString.TrimEnd(delimiter) + ")";
			return sql;
		}

		protected override string AddUpdateParameters(string[] columns, object[] values, string sql, IDbCommand command)
		{
			for (int i = 0; i < values.Length; i++)
			{
				string name = columns[i];
				object value = values[i];
				IDataParameter param = command.CreateParameter();
				param.Value = value;
				param.ParameterName =  name;
				command.Parameters.Add(param);
				sql += string.Format("{0} = {1},", name, "?");
			}

			char delimiter = Char.Parse(",");
			sql = sql.TrimEnd(delimiter) + " ";
			return sql;
		}

		public void RemoveCheckConstraints(string tableName, string columnName)
		{
			List<object> list = new List<object>();
			string query = string.Format(
				"SELECT CONSTRAINT_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE  WHERE TABLE_NAME = {0} AND COLUMN_NAME = {1} ",
				tableName, columnName);
			using(IDataReader dr = this.Select(query))
			{
				while(dr.Read()) 
				{
					list.Add(dr[0]);
				}
			}

			foreach(object item in list)
				this.ExecuteNonQuery(string.Format("ALTER TABLE {0} DROP CONSTRAINT {1}",tableName, item));
		}

		

		

		

		protected override string AddLimit(string sql, IOptions options)
		{
			if (options.Limit != null && options.Offset != null)
			{
				string select = string.Format("SELECT {0} TOP 1000000000", options.IsDistinct ? "DISTINCT" : "");

				string query = StringUtil.Gsub(sql, @"^\s*SELECT(\s+DISTINCT)?", select, RegexOptions.IgnoreCase);
				int totalrows = 0;
				query = string.Format("SELECT count(*) as TotalRows from {0} tally ", query);
				
				using(IDataReader dr = ExecuteReader(query))
				{	
					totalrows =	dr.GetInt32(0);
				}

				if ((options.Limit + options.Offset) >= totalrows)
					options.Limit = (totalrows - options.Offset >= 0) ? (totalrows - options.Offset) : 0;
				
				select =  string.Format("SELECT * FROM (SELECT TOP {0} * FROM (SELECT {1} TOP {2} ",
							options.Limit, 
							options.IsDistinct ? "DISTINCT" : "", 
							options.Limit + options.Offset);

				sql = StringUtil.Gsub(sql, @"^\s*SELECT(\s+DISTINCT)?", select, RegexOptions.IgnoreCase);
				sql += ") as tmp1";
				if (!string.IsNullOrEmpty(options.Order))
					sql += string.Format("ORDER BY {0}) as tmp2 ORDER BY {1}", ChangeOrder(options.Order), options.Order);
				else
					sql += ") as tmp2";

				return sql;
			}
			else if (options.Limit != null && ! StringUtil.IsMatch(sql, @"^\s*SELECT (@@|COUNT\()", RegexOptions.IgnoreCase))
			{
				string select = string.Format(
									"SELECT {0} TOP {1}",
									options.IsDistinct ? "DISTINCT" : "", 
									options.Limit);
				
				return StringUtil.Gsub(sql, @"^\s*SELECT(\s+DISTINCT)?", select, RegexOptions.IgnoreCase);
			}
			return sql;
		}

		private string ChangeOrder(string order)
		{
			string newOrder = "";
			foreach (string item in StringUtil.Split(order, ","))
			{
				if(StringUtil.IsMatch(item, @"\bASC\b",RegexOptions.IgnoreCase))
					newOrder += StringUtil.Gsub(item, @"\bASC\b", "DESC", RegexOptions.IgnoreCase);
				else if(StringUtil.IsMatch(item, @"\bDESC\b", RegexOptions.IgnoreCase))
					newOrder += StringUtil.Gsub(item,@"\bDESC\b", "ASC", RegexOptions.IgnoreCase); 
				else 
					newOrder += item;

				newOrder += ", ";
			}

			return StringUtil.TrimEnd(newOrder, ",  ");
		}

	}
}
