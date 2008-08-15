//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace Amplify.Data
{
	using System;
	using System.Collections.Generic;
	
	using System.Text;

	using Amplify.Linq;
	using Amplify.Diagnostics;

	public abstract partial class Adapter
	{
		public abstract void CreateDatabase(string name);

		public abstract void CreateDatabase();

		public abstract void DropDatabase(string name);

		public abstract void DropDatabase();

		public abstract string[] GetDatabases();

		public abstract void RecreateDatabase();

		public abstract void RecreateDatabase(string name);

		public abstract string CurrentDatabase();

		public abstract List<string> GetTableNames();

		public abstract List<IndexDefinition> GetIndexes(string tableName);

		public abstract List<ForeignKeyDefinition> GetForeignKeys(string tableName);

		public abstract List<ForeignKeyDefinition> GetForeignKeys(string tableName, bool isForeign);


		public abstract void RenameTable(string name, string newName);

#if LINQ
		public void AddColumn(string tableName, string columnName, string type, params Func<object, object>[] options)
		{
			this.AddColumn(tableName, columnName, type, Hash.New(options));
		}
#endif

		public virtual void AddColumn(string tableName, string columnName, string type, ColumnOptions options)
		{
			this.AddColumn(tableName, columnName, type, options.ToHash());
		}

		public abstract void AddColumn(string tableName, string columnName, string type, Hash options);


#if LINQ
		public virtual void ChangeColumn(string tableName, string columnName, string name, params Func<object, object>[] options)
		{
			this.ChangeColumn(tableName, columnName, null, Hash.New(options));
		}
#endif 

		public virtual void ChangeColumn(string tableName, string columnName, string type, ColumnOptions options)
		{
			this.ChangeColumn(tableName, columnName, type, options.ToHash());
		}

		public abstract void ChangeColumn(string tableName, string columnName, string type, Hash options);

		public abstract void RemoveColumn(string tableName, string columnName);

		public abstract void RenameColumn(string tableName, string name, string newName);

		public virtual string IndexName(string tableName, IEnumerable<string> columnNames)
		{
			string concat = "";

			foreach (string columnName in columnNames)
				concat += columnName + "_and_";

			return string.Format("index_{0}_on_{1}", tableName, StringUtil.TrimEnd(concat, "_and_"));
		}

		
		public virtual void CreateTable(string name, Hash options, TableCreationHandler handler)
		{
			TableDefinition table = new TableDefinition(this);
			options = options == null ? new Hash() : options;
			object key = options[primaryKey];

			if(!Object.Equals(key, false))
				table.PrimaryKey((key == null) ? "Id" : key.ToString());

			handler(table);

			if (options == null)
				options = Hash.New();

			bool force = (options["Force"] == null) ? false : (bool)options["Force"];

			if (force)
			{
				try
				{
					// throws an exception if the table does not exist.
					DropTable(name); 
				}
				catch (Exception ex)
				{
					Log.Debug(ex.Message);
				}
			}
		
			object temp = options["temporary"];
			if(temp == null || temp.ToString().Trim() == "")
				temp = "";
			else 
				temp = "TEMPORARY";

			string sql = string.Format("CREATE {0} TABLE ", temp);
			sql += string.Format("{0} (", name);
			sql += table.ToString();
			sql += string.Format(") {0}", options["options"]);

			this.ExecuteNonQuery(sql);
		}

		public virtual void DropTable(string tableName) 
		{
			this.ExecuteNonQuery(string.Format("DROP TABLE {0}", tableName));	
		}


		public virtual void AddIndex(string tableName, IEnumerable<string> columnNames)
		{
			this.AddIndex(tableName, columnNames, null);
		}

		public virtual void AddIndex(string tableName, IEnumerable<string> columnNames, Hash options)
		{
			string type = "";
			string indexName = IndexName(tableName, columnNames);
			if (options == null)
				options = new Hash();

			if(options.Count > 1) {
				type = (options["Unique"] == null) ?  "" : "UNIQUE";
				indexName  = (options["Name"] == null) ? indexName : options["Name"].ToString();
			} else if(options.Count == 1) {
				foreach(object value in options.Values)
					type = value.ToString();
			}
			string columns = "";
			
			foreach (string columName in columnNames)
				columns += columName + ", ";
			
			columns = StringUtil.TrimEnd(columns, ", ");

		
			this.ExecuteNonQuery(
				string.Format("CREATE {1} INDEX {2} ON {0} ({3})",
						tableName, 
						type, 
						QuoteColumnName(indexName), 
						columns)
				);
		}

		public virtual void RemoveIndex(string tableName, IEnumerable<string> columnNames)
		{
			this.ExecuteNonQuery(
				string.Format(
					"DROP INDEX {1} ON {0}",
					tableName, 
					this.QuoteColumnName(IndexName(tableName, columnNames)))
			);
		}

		public virtual void AddForeignKey(string tableName, string referenceTableName, string referenceColumnName)
		{
			this.AddForeignKey(tableName, referenceColumnName, referenceTableName, referenceColumnName);
		}

		public virtual void AddForeignKey(string tableName, string columnName, string referenceTableName, string referenceColumnName)
		{
			string query = string.Format(
					@"ALTER TABLE {0}
						ADD FK_{0}_{2}_{3} FOREIGN KEY
							({1})
						REFERENCES {2}
						({3}) ",
					new[] {
						this.QuoteTableName(tableName), 
						this.QuoteColumnName(columnName), 
						this.QuoteTableName(referenceTableName), 
						this.QuoteColumnName(referenceColumnName)
					}
				);

			this.ExecuteNonQuery(query);
		}

		public virtual void RemoveForeignKey(string tableName, string referenceTableName, string referenceColumnName)
		{
			this.ExecuteNonQuery(
				string.Format(
					@"	ALTER TABLE {0} 
						DROP FOREIGN KEY FK_{0}_{1}_{2}
					",
					tableName, referenceTableName, referenceColumnName
				));
		}

		


		public virtual void AddView(string viewName, string[] columnNames, string sql)
		{
			this.ExecuteNonQuery(
				string.Format(
					@"
						CREATE VIEW V_{0} (
							{1}
						)	AS 
						{2}
					",
					 viewName, EnumerableUtil.Join(columnNames, ", \n\t\t"), sql
			));
		}

		public virtual void RemoveView(string viewName)
		{
			this.ExecuteNonQuery(
				string.Format(
					@"
						DROP VIEW V_{0}
					",
					 viewName));
		}


		public void InitializeSchemaInformation()
		{
			try
			{

			}
			catch
			{
				
			}
		}
		 
		 

		public virtual string TypeToSql(string type, int? limit, int? precision, int? scale)
		{
			if (type == primaryKey)
				return this.NativeDatabaseTypes[type].ToString();

			Hash native = (Hash)NativeDatabaseTypes[type];

			string columnType = native[name].ToString();
			if (type.ToLower() == @decimal)
			{
				precision = (precision.HasValue) ? precision : (int)native[Adapter.precision];
				scale = (scale.HasValue) ? scale : (int)native[Adapter.scale]; 

				if (precision.HasValue)
				{
					if (scale.HasValue)
						columnType += string.Format("({0},{1})", precision, scale);
					else
						columnType += string.Format("({0})", precision);
				}
				else
					throw new ArgumentException("A decimal column must include a precision value");
				return columnType;
			}
			else
			{

				if (!limit.HasValue && native["Limit"] != null)
					limit = (int)native["Limit"];

				if (limit.HasValue)
					columnType += string.Format("({0})", limit);

				return columnType;
			}
		}

	}
}
