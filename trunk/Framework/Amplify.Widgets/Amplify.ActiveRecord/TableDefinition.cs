using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data
{
	using Amplify.Linq;

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

		public TableDefinition Column(string name, string type, params Func<object, object>[] options)
		{
		
			Hash hash = (type == primaryKey)? new Hash() : (Hash)Native[type];
			if(hash == null)
				hash = new Hash();
			hash.Merge(Hash.New(options));

			ColumnDefinition column = new ColumnDefinition(this.adapter) { Name = name, Type = type };
			column.Limit = (int?)hash["Limit"];
			column.Precision = (int?)hash["Precision"];
			column.Scale = (int?)hash["Scale"];
			column.Default = hash["Default"].Default("");
			column.IsNull = hash["Null"].Default(false);
			if (!this.Columns.Exists(item => item.Name == column.Name))
				this.Columns.Add(column);
			
			return this;
		}


		#region add multilple columns
		public void Column(IEnumerable<string> names, string type, params Func<object, object>[] options)
		{
			names.Each(name => this.Column(name, type, options));
		}

		
		#endregion

		public override string ToString()
		{
			string append = "";
			foreach (ColumnDefinition column in this.Columns)
				append += column.ToString() + ", ";
			return append.TrimEnd(", ".ToCharArray());
		}
	}
}
