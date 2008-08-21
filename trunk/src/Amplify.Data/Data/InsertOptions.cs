

namespace Amplify.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using Amplify.Linq;

	/// <summary>
	/// 
	/// </summary>
	public class SaveOptions
	{
		private Hash columnValues;
		private List<object> where;
		private List<String> columns;
		private List<Object> values;

		/// <summary>
		/// Initializes a new instance of the <see cref="SaveOptions"/> class.
		/// </summary>
		public SaveOptions()
		{
			this.TableName = "";
			this.StoredProcedureName = "";
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SaveOptions"/> class.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columnNames">The column names.</param>
		public SaveOptions(string tableName, params string[] columnNames):this()
		{
			this.TableName = tableName;
			this.ColumnNames.AddRange(columns);
		}

		/// <summary>
		/// Gets or sets the name of the stored procedure.
		/// </summary>
		/// <value>The name of the stored procedure.</value>
		public string StoredProcedureName { get; set; }

		/// <summary>
		/// Gets or sets the name of the table.
		/// </summary>
		/// <value>The name of the table.</value>
		public string TableName { get; set;  }

		/// <summary>
		/// Gets or sets the connection string.
		/// </summary>
		/// <value>The connection string.</value>
		internal string ConnectionString { get; set; }

		/// <summary>
		/// Gets the column names.
		/// </summary>
		/// <value>The column names.</value>
		public List<String> ColumnNames
		{
			get
			{
				if (this.columns == null)
					this.columns = new List<string>();
				return this.columns;
			}
		}

		/// <summary>
		/// Gets the column values.
		/// </summary>
		/// <value>The column values.</value>
		public List<Object> ColumnValues
		{
			get
			{
				if (this.values == null)
					this.values = new List<object>();
				return this.values;
			}
		}

		/// <summary>
		/// Gets the conditions.
		/// </summary>
		/// <value>The conditions.</value>
		public List<object> Conditions
		{
			get
			{
				if (this.where == null)
					this.where = new List<object>();
				return this.where;
			}
		}

		/// <summary>
		/// Adds the specified table name.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		/// <returns></returns>
		public SaveOptions Table(string tableName)
		{
			this.TableName = tableName;
			return this;
		}

		/// <summary>
		/// Adds the specified column names.
		/// </summary>
		/// <param name="columnNames">The column names.</param>
		/// <returns></returns>
		public SaveOptions Columns(params string[] columnNames)
		{
			this.ColumnNames.AddRange(columns);
			return this;
		}

		/// <summary>
		/// Adds the specified value name.
		/// </summary>
		/// <param name="valueName">Name of the value.</param>
		/// <returns></returns>
		public SaveOptions Values(params object[] valueName)
		{
			this.ColumnValues.AddRange(values);
			return this;
		}

		/// <summary>
		/// Adds the specified condition.
		/// </summary>
		/// <param name="condition">The condition.</param>
		/// <param name="values">The values.</param>
		/// <returns></returns>
		public SaveOptions Where(string condition, params object[] values)
		{
			this.Conditions.Add(condition);
			this.Conditions.AddRange(values);
			return this;
		}

		

	}
}
