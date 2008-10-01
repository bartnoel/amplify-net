using System;
using System.Collections.Generic;

using System.Text;

namespace Amplify.Data
{
	using Amplify.Linq;

	public abstract class Migration : SchemaBase
	{
		private Adapter adapter;

		public abstract DateTime CreatedOn { get; }

		public virtual int Version { get; set;  }

		static Migration()
		{
			Mode = "production";
			if (ApplicationContext.IsDevelopment)
				Mode = "development";
			else if (ApplicationContext.IsTesting)
				Mode = "test";
		}

		public Adapter Adapter
		{
			get { return this.adapter; }
			set { this.adapter = value; }
		}

		public static string  Mode { get; set; }


		#region CreateTable

#if LINQ
		public Migration CreateTable(string tableName, TableCreationHandler handler, params Func<object, object>[] options)
		{
			this.adapter.CreateTable(tableName, (options == null) ? null :  Hash.New(options), handler);
			return this;
		}
#endif 

		public Migration CreateTable(string tableName, TableCreationHandler handler)
		{
			this.adapter.CreateTable(tableName, Hash.New(), handler);
			return this;
		}

		public Migration CreateTable(string tableName, Hash options, TableCreationHandler handler)
		{
			this.adapter.CreateTable(tableName, options, handler);
			return this;
		}

		public Migration CreateTable(string tableName, object primaryKey, TableCreationHandler handler)
		{
			this.CreateTable(tableName, new Hash(){{"primarykey",  primaryKey }}, handler);
			return this; 
		}

		public Migration CreateTable(string tableName, bool force, TableCreationHandler handler)
		{
			this.CreateTable(tableName, new Hash{{ "Force", force }} , handler);
			return this;
		}

		public Migration CreateTable(string tableName, bool force, object primaryKey, TableCreationHandler handler)
		{
			this.CreateTable(tableName, new Hash() { {"Force", force}, {"primarykey", primaryKey} }, handler);
			return this;
		}
		#endregion

		public Migration CreateDatabase()
		{
			this.Adapter.CreateDatabase();
			return this;
		}

		public Migration DropDatabase()
		{
			this.Adapter.DropDatabase();
			return this;
		}

		public Migration CreateDatabase(string database, bool useSuffix)
		{
			if(useSuffix)
				database += "_" + Mode;
			this.Adapter.CreateDatabase(database);
			return this;
		}

		public Migration DropDatabase(string database, bool useSuffix)
		{
			if (useSuffix)
				database += "_" + Mode;
			this.Adapter.DropDatabase(database);
			return this;
		}

		public Migration DropTable(string tableName)
		{
			this.adapter.DropTable(tableName);
			return this;
		}

		#region Add Column


		public Migration AddColumn(string tableName, string columnName, DbTypes type, ColumnOptions options)
		{
			this.adapter.AddColumn(tableName, columnName, type, options);
			return this;
		}

		public Migration AddColumn(string tableName, string columnName, DbTypes type, Hash options)
		{
			this.adapter.AddColumn(tableName, columnName, type, options);
			return this;
		}

#if LINQ
		public Migration AddColumn(string tableName, string columnName, DbTypes type, params Func<object, object>[] options)
		{
			this.adapter.AddColumn(tableName, columnName, type, options);
			return this;
		}
#endif


		
		#endregion

		#region Change Column


		public Migration ChangeColumn(string tableName, string columnName, DbTypes type, ColumnOptions options)
		{
			this.adapter.ChangeColumn(tableName, columnName, type, options);
			return this;
		}

		public Migration ChangeColumn(string tableName, string columnName, DbTypes type, Hash options)
		{
			this.adapter.ChangeColumn(tableName, columnName, type, options);
			return this;
		}

#if LINQ
		public Migration ChangeColumn(string tableName, string columnName, DbTypes type, params Func<object, object>[] options)
		{
			this.adapter.ChangeColumn(tableName, columnName, type, options);
			return this;
		}
#endif

		
		#endregion

		public Migration RemoveColumn(string tableName, string columnName)
		{
			this.adapter.RemoveColumn(tableName, columnName);
			return this;
		}

#if LINQ
		public Migration AddIndex(string tableName, IEnumerable<string> columnNames, params Func<object, object>[] options) 
		{
			this.adapter.AddIndex(tableName, columnNames, Hash.New(options));
			return this;
		}
#endif

		public Migration AddIndex(string tableName, IEnumerable<string> columnNames, Hash options)
		{
			this.adapter.AddIndex(tableName, columnNames, options);
			return this;
		}

		public Migration RemoveIndex(string tableName, IEnumerable<string> columnNames)
		{
			this.adapter.RemoveIndex(tableName, columnNames);
			return this;
		}

		public int ExecuteNonQuery(string sql, params object[] values)
		{
			return this.adapter.ExecuteNonQuery(sql, values);
		}

		public abstract void Up();

		public abstract void Down();
	}

	public delegate void TableCreationHandler(TableDefinition table);
}
