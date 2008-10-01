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

		public abstract DbTypes MapDbType(ColumnDefinition columnDefinition);

		public abstract ColumnDefinition MapColumn(DbTypes dbtype, ColumnDefinition columnDefinition);

		public abstract string[] GetDatabases(bool usePrimary);

		public abstract void RecreateDatabase();

		public abstract void RecreateDatabase(string name);

		public abstract string CurrentDatabase();

		public abstract List<string> GetTableNames();

		public abstract List<IndexDefinition> GetIndexes(string tableName);

		public abstract List<ForeignKeyConstraint> GetForeignKeys(string tableName);

		public abstract List<ForeignKeyConstraint> GetForeignKeys(string tableName, bool isForeign);

		public string[] GetDatabases()
		{
			return this.GetDatabases(false);
		}

		public abstract void RenameTable(string name, string newName);

#if LINQ
		public void AddColumn(string tableName, string columnName, DbTypes type, params Func<object, object>[] options)
		{
			this.AddColumn(tableName, columnName, type, Hash.New(options));
		}
#endif

		public virtual void AddColumn(string tableName, string columnName, DbTypes type, ColumnOptions options)
		{
			this.AddColumn(tableName, columnName, type, options.ToHash());
		}

		public abstract void AddColumn(string tableName, string columnName, DbTypes type, Hash options);


#if LINQ
		public virtual void ChangeColumn(string tableName, string columnName, DbTypes type,  params Func<object, object>[] options)
		{
			this.ChangeColumn(tableName, columnName, type, Hash.New(options));
		}
#endif 

		public virtual void ChangeColumn(string tableName, string columnName, DbTypes type, ColumnOptions options)
		{
			this.ChangeColumn(tableName, columnName, type, options.ToHash());
		}

		public abstract void ChangeColumn(string tableName, string columnName, DbTypes type, Hash options);

		public abstract void RemoveColumn(string tableName, string columnName);

		public abstract void RenameColumn(string tableName, string name, string newName);

		public virtual string IndexName(string tableName, IEnumerable<string> columnNames)
		{
			string concat = "";

			foreach (string columnName in columnNames)
				concat += columnName + "_";

			return string.Format("IX_{0}_{1}", tableName, StringUtil.TrimEnd(concat, "_"));
		}

		public virtual void CreateTable(string name, Action<TableDefinition> handler)
		{
			TableDefinition table = new TableDefinition(this);
			table.Name = name;

			

			handler(table);

			if (!Object.Equals(table.Id, false))
				table.AddPrimaryKey((table.Id == null) ? "Id" : table.Id.ToString());

			if (table.Force)
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

			this.ExecuteNonQuery(table.ToString());
		}

		public virtual void CreateTable(Action<TableDefinition> handler)
		{
			TableDefinition table = new TableDefinition(this);

			

			handler(table);


			if (!Object.Equals(table.Id, false))
				table.AddPrimaryKey((table.Id == null) ? "Id" : table.Id.ToString());

			if (table.Force)
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

			this.ExecuteNonQuery(table.ToString());
		}
		
		public virtual void CreateTable(string name, Hash options, TableCreationHandler handler)
		{
			TableDefinition table = new TableDefinition( 
				(options == null) ? new Hash() : options,
				this);
			
			table.Name = name;

			

			handler(table);

			if (!Object.Equals(table.Id, false))
				table.AddPrimaryKey((table.Id == null) ? "Id" : table.Id.ToString());

			if (table.Force)
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
		
			this.ExecuteNonQuery(table.ToString());
		}

		public virtual void DropTable(string tableName) 
		{
			this.ExecuteNonQuery(string.Format("DROP TABLE {0}", tableName));	
		}

		internal protected virtual bool BuildCreateTableForeignKeyAtEnd
		{
			get { return false; }
		}

		internal protected virtual string BuildCreateTableForeignKey(ForeignKeyConstraint definition)
		{
			string options = "";
			
			if(definition.OnDelete != ConstraintDeleteAction.None)
				options += " ON DELETE " + definition.OnDelete.ToString().ToUpper() + " ";
			
			if(definition.OnUpdate != ConstraintUpdateAction.None)
				options += " ON UPDATE " + definition.OnUpdate.ToString().ToUpper() + " ";

			return string.Format(" CONSTRAINT FK_{0}_{1} FOREIGN KEY () REFERENCES  {2}({3}) {4} ",
				new object[] {
					definition.PrimaryTableName,
					definition.PrimaryColumnName,
					definition.ReferenceTableName,
					definition.ReferenceColumnNames,
					options
				});
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
				indexName  = (options["name"] == null) ? indexName : options["name"].ToString();
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
		 
		 

		public virtual string TypeToSql(ColumnDefinition column)
		{
			string sql = column.Type.ToLower().Trim() + " ";

			if (column.DbType == DbTypes.Decimal)
			{
				if (column.Precision.HasValue)
				{
					if (column.Scale.HasValue)
						sql += string.Format("({0},{1})", column.Precision, column.Scale);
					else
						sql += string.Format("({0})", column.Precision);
				}
			}
			else if(StringUtil.IsMatch(column.Type, @"(char|binary)", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
			{
				sql += string.Format("({0})", column.Limit);
			}

			return sql;
		}

	}
}
