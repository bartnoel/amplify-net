using System;
using System.Collections.Generic;

using System.Text;

namespace Amplify.Data
{
	using Amplify.Linq;
	using Amplify.Diagnostics;

	public class TableDefinition : SchemaBase 
	{
		private List<ColumnDefinition> columns;
		private Hash properties;

		public TableDefinition()
		{
			this.properties = new Hash();
			this.IsTemporary = false;
			this.Options = "";
			this.Id = "Id";
			this.Force = false;
		}

		public TableDefinition(Hash options) :this()
		{
			foreach(string key in  options.Keys)
				this.properties[key] = options[key];
		}

		public TableDefinition(Adapter adapter) : this()
		{
			this.Adapter = adapter;
		}

		public TableDefinition(Hash options, Adapter adapter)
			: this(options)
		{
			this.Adapter = adapter;
		}

		internal protected Adapter Adapter { get; set; }

		public string Name 
		{
			get { return this.properties["name"] as string; }
			set { this.properties["name"] = value; }
		}

		public bool IsTemporary
		{
			get { return (bool)this.properties["temporary"]; }
			set { this.properties["temporary"] = value; }
		}

		public bool Force
		{
			get { return (bool)this.properties["force"]; }
			set { this.properties["force"] = value; }
		}

		public object Id
		{
			get { return this.properties["id"]; }
			set { this.properties["id"] = value; }
		}

		public string Options
		{
			get { return this.properties["options"] as string; }
			set { this.properties["options"] = value; }
		}

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
				return this.Adapter.NativeDatabaseTypes;
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
			return this.Column(name, type, delegate(ColumnDefinition item) { });
		}

		public TableDefinition Column(string name, string type, Action<ColumnDefinition> action)
		{
			ColumnDefinition definition = new ColumnDefinition();
			definition.Adapter = this.Adapter;
			definition.Table = this;
			definition.Name = name;
			definition.Type = type;
			if (action != null)
				action(definition);
			this.Columns.Add(definition);
			return this;
		}

		public TableDefinition Column(Action<ColumnDefinition> action)
		{
			ColumnDefinition definition = new ColumnDefinition();
			definition.Adapter = this.Adapter;
			definition.Table = this;
			action(definition);
			this.Columns.Add(definition);
			return this;
		}

		public TableDefinition Column(string name, string type, Hash options)
		{
			Hash hash = (type == primaryKey) ? new Hash() : (Hash)Native[type];

			if (options == null)
				options = Hash.New();
			
			foreach (string key in hash.Keys)
				options[key] = hash[key];

			ColumnDefinition column = new ColumnDefinition(this.Adapter) { Name = name, Type = type };
			column.Limit = (int?)options["limit"];
			column.Precision = (int?)options["precision"];
			column.Scale = (int?)options["scale"];
			column.Default = options["default"];
			column.IsNull = (options["null"] == null) ? false : true;

			if (!this.Columns.Exists(item => item.Name == column.Name))
			{
				if (column.Type.Contains("PrimaryKey"))
					this.Columns.Insert(0, column);
				else 
					this.Columns.Add(column);
			}
			
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
			string append = string.Format("CREATE {0}TABLE {1} (",
				this.IsTemporary ? "TEMPORARY " : "", this.Name);
			foreach (ColumnDefinition column in this.Columns)
				append += column.ToString() + ",\n ";
			append = append.TrimEnd(",\n ".ToCharArray()) +
				string.Format("{0}) ", this.Options);

			return append;
		}
	}
}
