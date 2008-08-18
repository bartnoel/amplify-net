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

		public TableDefinition()
		{
			this.Options = "";
		}

		public TableDefinition(Adapter adapter)
			:this()
		{
			this.adapter = adapter;
		}

		private List<ColumnDefinition> columns;

		public string Name { get; set; }

		public bool Force { get; set; }

		public bool Temporary { get; set; }

		public string Options { get; set; }

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

		public TableDefinition PrimaryKey(string name, string type)
		{
			return this.PrimaryKey(name, type, new Hash());
		}

		public TableDefinition PrimaryKey(string name, Type type)
		{
			return this.PrimaryKey(name, type, new Hash());
		}

		public TableDefinition PrimaryKey(string name, Type type, ColumnOptions options)
		{
			return this.PrimaryKey(name, type, options.ToHash());
		}

		public TableDefinition PrimaryKey(string name, string type, ColumnOptions options)
		{
			return this.PrimaryKey(name, type, options.ToHash());
		}

		public TableDefinition PrimaryKey(string name, Type type, Hash options)
		{
			string typeName = type.Name.ToLower();
			if (!this.adapter.NativeDatabaseTypes.ContainsKey(typeName))
				throw new Exception("type is not a valid type can be converted to an sql type");


			return this.PrimaryKey(name, typeName, options);
		}

		public TableDefinition PrimaryKey(string name, string type, Hash options)
		{
			if (type == @string || type == @integer || type == @guid)
				type = "primarykey" + type;
			if (!type.Contains("primarykey"))
				throw new Exception("type is not a valid primary key type");

			this.Column(name, type, options);
			return this;
		}

		public TableDefinition Column(string name, string type)
		{
			return this.Column(name, type, new ColumnOptions());
		}

		public TableDefinition Column(string name, Type type, ColumnOptions options)
		{
			return this.Column(name, type, options.ToHash());
		}

		public TableDefinition Column(string name, string type, ColumnOptions options)
		{
			return this.Column(name, type, options.ToHash());
		}

		public TableDefinition Column(string name, Type type, Hash options)
		{
			string typeName = type.Name.ToLower();
			if (!this.adapter.NativeDatabaseTypes.ContainsKey(typeName))
				throw new Exception("type is not a valid type can be converted to an sql type");

			return this.Column(name, typeName, options);
		}

		public TableDefinition Column(string name, string type, Hash options)
		{
			Hash hash = (type.Contains("primarykey")) ? new Hash() : (Hash)Native[type];

			if (options == null)
				options = Hash.New();
			
			foreach (string key in hash.Keys)
				options[key] = hash[key];

			ColumnDefinition column = new ColumnDefinition(this.adapter) { Name = name, SqlType = type };
			column.Limit = (int?)options["limit"];
			column.Precision = (int?)options["precision"];
			column.Scale = (int?)options["scale"];
			column.Default = options["default"];
			column.IsNull = (options["null"] == null) ? false : true;

			if (!this.Columns.Exists(item => item.Name == column.Name))
				this.Columns.Add(column);
			
			return this;
		}


		#region add multilple columns
#if LINQ
		public TableDefinition Column(IEnumerable<string> names, string type, params Func<object, object>[] options)
		{
			foreach(string name in names)
				this.Column(name, type, Hash.New(options));
			return this;
		}
#endif 

		public TableDefinition Column(IEnumerable<string> names, string type, ColumnOptions options)
		{
			foreach (string name in names)
				this.Column(name, type, options.ToHash());

			return this;
		}

		public TableDefinition Column(IEnumerable<string> names, string type, Hash options)
		{
			foreach (string name in names)
				this.Column(name, type, options);
			return this;
		}


		
		#endregion

		public override string ToString()
		{
			if (string.IsNullOrEmpty(this.Name))
				throw new InvalidOperationException("The name of the table can not be empty or null");

			string append = string.Format("CREATE {0}TABLE {1} ( \n\t", this.Temporary ? "TEMPORARY " : "", this.Name);
			
			foreach (ColumnDefinition column in this.Columns)
				append += column.ToString() + ",\n\t ";

			return string.Format("{0}\n) {1}", 
				append.TrimEnd(",\n\t ".ToCharArray()), this.Options);
		}
	}
}
