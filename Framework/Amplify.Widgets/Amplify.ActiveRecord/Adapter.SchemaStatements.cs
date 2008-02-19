//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace Amplify.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using Amplify.Linq;

	public abstract partial class Adapter
	{
		public abstract void CreateDatabase(string name);

		public abstract void DropDatabase(string name);

		public abstract void RecreateDatabase(string name);

		public abstract string CurrentDatabase();

		public abstract List<string> GetTableNames();

		public abstract List<IndexDefinition> GetIndexes(string tableName);


		public abstract void RenameTable(string name, string newName);

		public abstract void AddColumn(string tableName, string columnName, string type, params Func<object, object>[] options);

		public abstract void ChangeColumn(string tableName, string columnName, string type, params Func<object, object>[] options);

		public abstract void RemoveColumn(string tableName, string columnName);

		public abstract void RenameColumn(string tableName, string name, string newName);

		public virtual string IndexName(string tableName, IEnumerable<string> columnNames)
		{
			string concat = "";
			columnNames.Each(name => concat += name + "_and_");
			return ("index_{0}_on_{1}").Inject(new object[] { tableName, concat.TrimEnd("_and_".ToCharArray()) });	
		}


		public virtual void CreateTable(string name, Hash options, TableCreationHandler handler)
		{
			TableDefinition table = new TableDefinition(this);
			if(!Object.Equals(options[primaryKey], false))
				table.PrimaryKey(options[primaryKey].Default("Id"));

			handler(table);
			
			if(options["Force"].Default(false) == true)
				DropTable(name);

			string sql = "CREATE {0} TABLE ".Inject((options["temporary"].Default(false) ? "TEMPORARY" : ""));
			sql += "{0} (".Inject(name);
			sql +=  table.ToString();
			sql += ") {0}".Inject(options["options"]);
			this.ExecuteNonQuery(sql);
		}

		public virtual void DropTable(string tableName) 
		{
			this.ExecuteNonQuery("DROP TABLE {0}".Inject(tableName));	
		}


		public virtual void AddIndex(string tableName, IEnumerable<string> columnNames, params object[] values)
		{
			string type;
			string indexName = IndexName(tableName, columnNames); 
			if(values.Length > 1) {
				Store options = new Store(values);
				type = (options["unique"].Default(false)) ? "UNIQUE" : "";
				indexName  = options["name"].Default(indexName);
			} else {
				type = values.GetValue(0).ToString();
			}
			string join = columnNames.Join(", ");
			this.ExecuteNonQuery("CREATE {1} INDEX {2} ON {0} ({3})".Inject(tableName, 
				type, QuoteColumnName(indexName), join));
		}

		public virtual void RemoveIndex(string tableName, IEnumerable<string> columnNames)
		{
			this.ExecuteNonQuery("DROP INDEX {1} ON {0}".Inject(tableName, this.QuoteColumnName(IndexName(tableName, columnNames))));
		}
		
		 
		 

		public virtual string TypeToSql(string type, int? limit, int? precision, int? scale)
		{
			if (type == primaryKey)
				return this.NativeDatabaseTypes[type].ToString();

			Store native = (Store)NativeDatabaseTypes[type];

			string columnType = native[name].ToString();
			if (type.ToLower() == @decimal)
			{
				precision = precision.Default((int)native[Adapter.precision]);
				scale = scale.Default((int)native[Adapter.scale]);

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

				if (!limit.HasValue && native[Adapter.limit] != null)
					limit = (int)((Store)native[type])[Adapter.limit];

				if (limit.HasValue)
					columnType += string.Format("({0})", limit);

				return columnType;
			}
		}

	}
}
