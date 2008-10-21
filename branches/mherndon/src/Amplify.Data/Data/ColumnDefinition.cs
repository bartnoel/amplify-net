﻿using System;
using System.Collections.Generic;

using System.Text;

namespace Amplify.Data
{

	using Amplify.Linq;

	/// <summary>
	/// 
	/// </summary>
    public class ColumnDefinition : SchemaBase, ICloneable 
    {
		private List<String> checks;
		private List<ForeignKeyConstraint> foreignKeys;
		private Hash properties = new Hash();


		/// <summary>
		/// Initializes a new instance of the <see cref="ColumnDefinition"/> class.
		/// </summary>
		public ColumnDefinition()
		{
			this.IsNull = true;
			this.IsPrimaryKey = false;
			this.IsUnique = false;
			this.Type = "";
			this.Identity = "";
			this.IsSpecial = false;
			this["dbtype"] = DbTypes.None;
		}

		internal ColumnDefinition(Hash options)
			: this()
		{
			foreach (string key in options.Keys)
				this.properties[key] = options[key];
		}

		internal ColumnDefinition(Hash options, Adapter adapter)
			: this(options)
		{
			this.Adapter = adapter;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColumnDefinition"/> class.
		/// </summary>
		/// <param name="adapter">The adapter.</param>
		internal ColumnDefinition(Adapter adapter) : this()
		{
			this.Adapter = adapter;
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
        public string Name 
		{
			get {
				return (this["name"] as string);
			}
			set {
				if (this.Adapter != null && this.Adapter.LowerNaming)
					value = value.ToLower();

				this["name"] = value;
			}
		}

		public DbTypes DbType
		{
			get { return (DbTypes)this["dbtype"]; }
			set {
				if (!Object.Equals(value, this["dbtype"]))
				{
					this.Adapter.MapColumn(value, this);
					this["dbtype"] = value;
				}
			}
		}

		public TableDefinition Table { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is primary key.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is primary key; otherwise, <c>false</c>.
		/// </value>
		public bool IsPrimaryKey 
		{
			get { return (bool)this["primarykey"]; }
			set {
				if (!Object.Equals(value, this["primarykey"]))
				{
					this["primarykey"] = value;
					if(this.Adapter != null)
						this["dbtype"] = this.Adapter.MapDbType(this);
				}
			}
		}

		public bool IsSpecial { get; internal protected set; }

		public string Identity
		{
			get { return this["identity"].ToString(); }
			set { this["identity"] = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is unique.
		/// </summary>
		/// <value><c>true</c> if this instance is unique; otherwise, <c>false</c>.</value>
		public bool IsUnique 
		{
			get { return (bool)this["unique"]; }
			set { this["unique"] = value; }
		}

		/// <summary>
		/// Gets the foreign keys.
		/// </summary>
		/// <value>The foreign keys.</value>
		public List<ForeignKeyConstraint> ForeignKeys
		{
			get {
				if (this["foreignkeys"] == null)
					this["foreignkeys"] = new List<ForeignKeyConstraint>();
				return (List<ForeignKeyConstraint>)this["foreignkeys"];
			}
		}

		/// <summary>
		/// Gets the checks.
		/// </summary>
		/// <value>The checks.</value>
		public List<String> Checks
		{
			get {
				if (this["checks"] == null)
					this["checks"] = new List<string>();
				return (List<string>)this["checks"];
			}
		}

		/// <summary>
		/// Gets or sets the seudo type.
		/// </summary>
		/// <value>The type.</value>
		public string Type 
		{ 
			get { return (this["type"] as string); }
			set {
				if(!Object.Equals(value, this["type"]))
				{
					this["type"] = value; 
					if(!string.IsNullOrEmpty(value) && this.Adapter != null)
						this["dbtype"] = this.Adapter.MapDbType(this);
				}
			}
		}

		/// <summary>
		/// Gets or sets the limit.
		/// </summary>
		/// <value>The limit.</value>
		public int? Limit 
		{
			get { return (int?)this["limit"]; }
			set { this["limit"] = value; } 
		}

		/// <summary>
		/// Gets or sets the precision.
		/// </summary>
		/// <value>The precision.</value>
		public int? Precision 
		{
			get { return (int?)this["precision"]; }
			set { this["precision"] = value; }
		}

		/// <summary>
		/// Gets or sets the scale.
		/// </summary>
		/// <value>The scale.</value>
		public int? Scale
		{
			get { return (int?)this["scale"]; }
			set { this["scale"] = value; }
		}

		/// <summary>
		/// Gets or sets the default.
		/// </summary>
		/// <value>The default.</value>
		public object Default 
		{
			get { return this["default"]; }
			set { this["default"] = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is null.
		/// </summary>
		/// <value><c>true</c> if this instance is null; otherwise, <c>false</c>.</value>
		public bool IsNull
		{
			get { return (bool)this["isnull"]; }
			set { this["isnull"] = value; }
		}
		
		/// <summary>
		/// Gets or sets the <see cref="System.Object"/> with the specified key.
		/// </summary>
		/// <value></value>
		public object this[string key]
		{
			get {
				return this.properties[key];
			}
			set {
				this.properties[key] = value;
			}
		}

		internal protected Adapter Adapter { get; set; }

		public ColumnDefinition ForeignKey(Action<ForeignKeyConstraint> handler)
		{
			ForeignKeyConstraint fk = new ForeignKeyConstraint()
			{
				Column = this,
				PrimaryColumnName = this.Name,
				PrimaryTableName = this.Table.Name
			};
			handler(fk);
			this.ForeignKeys.Add(fk);

			return this;
		}

		public ColumnDefinition ForeignKey(string referenceTable, string referenceColumn)
		{
			return this.ForeignKey(referenceTable, referenceColumn,
				ConstraintDeleteAction.None, ConstraintUpdateAction.None);
		}

		public ColumnDefinition ForeignKey(string referenceTable, string referenceColumn,
			ConstraintDeleteAction deleteAction, ConstraintUpdateAction updateAction)
		{
			return this.ForeignKey(referenceTable, referenceColumn,
				ConstraintDeleteAction.None, ConstraintUpdateAction.None, this.Table.Name);
		}



		public ColumnDefinition ForeignKey(string referenceTable, string referenceColumn,
			ConstraintDeleteAction deleteAction, ConstraintUpdateAction updateAction, string primaryTableName)
		{
			this.ForeignKeys.Add(new ForeignKeyConstraint()
			{
				Column = this,
				PrimaryTableName = primaryTableName,
				PrimaryColumnName = this.Name,
				ReferenceTableName = referenceTable,
				ReferenceColumnNames = referenceColumn,
				OnDelete = deleteAction,
				OnUpdate = updateAction 
			});

			return this;
		}
				
		

		protected virtual string ToSql() 
		{
			string sql = string.Format("{0} {1}",this.Adapter.QuoteColumnName(this.Name), this.Adapter.TypeToSql(this));

			if (!string.IsNullOrEmpty(this.Identity))
				sql += " " + this.Identity + " ";

			if (!this.IsNull)
				sql += " NOT NULL ";

			if (this.Table != null && this.IsUnique)
				sql += string.Format(" CONSTRAINT UQ_{0}_{1} UNIQUE ", this.Table.Name, this.Name);

			if (this.Table != null && this.Default != null)
				sql += string.Format(" CONSTRAINT DF_{0}_{1} DEFAULT ({2})", 
					this.Table.Name, this.Name,
					(this.Default is string && this.Default.ToString().Contains("(") ? this.Default : this.Adapter.Quote(this.Default)));

		
			

			if (this.Table != null && this.Checks.Count > 0)
			{
				foreach (string check in checks)
					sql += string.Format(" CONSTRAINT CK_{0}_{1} CHECK({0}) ",
						this.Table.Name, this.Name, string.Format(check, this.Name));
			}

			if (this.ForeignKeys.Count > 0)
			{
				foreach (ForeignKeyConstraint foreignKey in this.ForeignKeys)
				{
					if (this.Table != null && this.Adapter.BuildCreateTableForeignKeyAtEnd)
						this.Table.Options += foreignKey.ToString();
					else
						sql += foreignKey.ToString();
				}
			}
			
			
			return sql;
		}
	
        public override string  ToString()
        {
			return this.ToSql();        
        }


		#region ICloneable Members

		public object Clone()
		{
			ColumnDefinition column = new ColumnDefinition();
			foreach (string key in this.properties.Keys)
				column[key] = this.properties[key];

			return column;
		}

		#endregion
	}
}