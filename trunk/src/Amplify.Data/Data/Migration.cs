using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data
{
	using Amplify.Linq;

	public abstract class Migration : SchemaBase
	{
		private Adapter adapter;


		public Adapter Adapter
		{
			get { return this.adapter; }
			set { this.adapter = value; }
		}

		#region CreateTable

		public void CreateTable(string tableName, TableCreationHandler handler, params Func<object, object>[] options)
		{
		
			this.adapter.CreateTable(tableName, (options == null) ? null :  Hash.New(options), handler);
		}

		public void CreateTable(string tableName, Hash options, TableCreationHandler handler)
		{
			this.adapter.CreateTable(tableName, options, handler);
		}

		public void CreateTable(string tableName, object primaryKey, TableCreationHandler handler)
		{
			this.CreateTable(tableName, Hash.New(PrimaryKey => primaryKey), handler);
		}

		public void CreateTable(string tableName, bool force, TableCreationHandler handler)
		{
			this.CreateTable(tableName, Hash.New(Force => force), handler);
		}

		public void CreateTable(string tableName, bool force, object primaryKey, TableCreationHandler handler)
		{
			this.CreateTable(tableName, Hash.New(Force => force, PrimaryKey => primaryKey), handler);
		}
		#endregion

		public void DropTable(string tableName)
		{
			this.adapter.DropTable(tableName);
		}

		#region Add Column
		

		public void AddColumn(string tableName, string columnName, string type, params Func<object, object>[] options)
		{
			this.adapter.AddColumn(tableName, columnName, type, options);
		}


		
		#endregion

		#region Change Column
		public void ChangeColumn(string tableName, string columnName, string type, params Func<object, object>[] options)
		{
			this.adapter.ChangeColumn(tableName, columnName, type, options);
		}

		
		#endregion

		public void RemoveColumn(string tableName, string columnName)
		{
			this.adapter.RemoveColumn(tableName, columnName);
		}

		public void AddIndex(string tableName, IEnumerable<string> columnNames, params Func<object, object>[] options)
		{
			this.adapter.AddIndex(tableName, columnNames, options);
		}

		public void RemoveIndex(string tableName, IEnumerable<string> columnNames)
		{
			this.adapter.RemoveIndex(tableName, columnNames);
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
