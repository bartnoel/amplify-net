using System;
using System.Collections.Generic;

using System.Text;

namespace Amplify.Data
{
	using Amplify.Linq;
	using Amplify.Diagnostics;

	public class TableDefinition : SchemaBase 
	{
		private Adapter adapter;

		public TableDefinition(Adapter adapter)
		{
			this.adapter = adapter;
		}

		private List<ColumnDefinition> columns;

		public string Name { get; set; }

		public List<ColumnDefinition> Columns
		{
			get {
				if (this.columns == null)
					this.columns = new List<ColumnDefinition>();
				return this.columns;
			}
		}

		public Hash Native
		{
			get {
				return this.adapter.NativeDatabaseTypes;
			}
		}

		public ColumnDefinition this[string name]
		{
			get
			{

				return this.Columns.Find(item => item.Name.ToLower() == name.ToLower());
			}
		}

		public TableDefinition PrimaryKey(string name)
		{
			return this.Column(name, primaryKey);
		}

		public TableDefinition Column(string name, string type)
		{
			return this.Column(name, type, new ColumnOptions());
		}

		public TableDefinition Column(string name, string type, ColumnOptions options)
		{
			return this.Column(name, type, options.ToHash());
		}

		public TableDefinition Column(string name, string type, Hash options)
		{
			Hash hash = (type == primaryKey) ? new Hash() : (Hash)Native[type];

			if (options == null)
				options = Hash.New();
			
			foreach (string key in hash.Keys)
				options[key] = hash[key];

			ColumnDefinition column = new ColumnDefinition(this.adapter) { Name = name, Type = type };
			column.Limit = (int?)options["Limit"];
			column.Precision = (int?)options["Precision"];
			column.Scale = (int?)options["Scale"];
			column.Default = options["Default"];
			column.IsNull = (options["Null"] == null) ? false : true;

			if (!this.Columns.Exists(item => item.Name == column.Name))
				this.Columns.Add(column);
			
			return this;
		}


		#region add multilple columns
#if LINQ
		public void Column(IEnumerable<string> names, string type, params Func<object, object>[] options)
		{
			foreach(string name in names)
				this.Column(name, type, Hash.New(options));
		}
#endif 

		public void Column(IEnumerable<string> names, string type, ColumnOptions options)
		{
			foreach (string name in names)
				this.Column(name, type, options.ToHash());
		}

		public void Column(IEnumerable<string> names, string type, Hash options)
		{
			foreach (string name in names)
				this.Column(name, type, options);
		}


		
		#endregion

		public override string ToString()
		{
			string append = "";
			foreach (ColumnDefinition column in this.Columns)
				append += column.ToString() + ",\n ";
			return append.TrimEnd(", ".ToCharArray());
		}
	}
}
