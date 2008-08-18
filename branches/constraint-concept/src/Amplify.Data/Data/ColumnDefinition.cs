using System;
using System.Collections.Generic;

using System.Text;

namespace Amplify.Data
{

	using Amplify.Linq;

    public class ColumnDefinition : SchemaBase
    {
		private ConstraintsList constraints = new ConstraintsList();


		/// <summary>
		/// Initializes a new instance of the <see cref="ColumnDefinition"/> class.
		/// </summary>
		public ColumnDefinition() { 
			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColumnDefinition"/> class.
		/// </summary>
		/// <param name="adapter">The adapter.</param>
		public ColumnDefinition(Adapter adapter): this()
		{
			this.Adapter = adapter;
		}

		/// <summary>
		/// Gets or sets the name of the column.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		public string TableName { get; set; }

		public bool ConstraintsInitialized { get; set; }

		/// <summary>
		/// Gets or sets the type of the SQL.
		/// </summary>
		/// <value>The type of the SQL.</value>
		public string SqlType { get; set; }


		/// <summary>
		/// Gets or sets the limit.
		/// </summary>
		/// <value>The limit.</value>
		public int? Limit { get; set; }

		/// <summary>
		/// Gets or sets the precision.
		/// </summary>
		/// <value>The precision.</value>
		public int? Precision { get; set; }


		/// <summary>
		/// Gets or sets the scale.
		/// </summary>
		/// <value>The scale.</value>
		public int? Scale { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this column is special.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this column is special; otherwise, <c>false</c>.
		/// </value>
		public bool IsSpecial { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether allows nulls.
		/// </summary>
		/// <value><c>true</c> if this column allows null; otherwise, <c>false</c>.</value>
		public bool IsNull { get; set; }

		/// <summary>
		/// Gets a value if this is a primary key. 
		/// </summary>
		/// <value>The is primary key.</value>
		public bool IsPrimaryKey 
		{ 
			get 
			{
				return this.Contraints.PrimaryKey != null;
			}
			set
			{
				if (value)
					this.Contraints.PrimaryKey = new PrimaryKeyConstraint()
					{
						TableName = this.TableName,
						ColumnName = this.Name
					};
				else
					this.Contraints.PrimaryKey = null;
			}
		}

		public object Default
		{
			get {
				if(this.HasConstraint(typeof(DefaultConstraint))) {
					return (DefaultConstraint) this.Contraints.Find(delegate(ConstraintDefinition item) {
						return item is DefaultConstraint;
					});
				}
				return null;
			}
			set {
				if (!this.HasConstraint(typeof(DefaultConstraint)))
				{
					this.Contraints.Add(new DefaultConstraint()
					{
						TableName = this.TableName,
						ColumnName = this.Name,
						DefaultValue = value
					});
				}
				else
				{
					((DefaultConstraint)this.Contraints.Find(delegate(ConstraintDefinition item)
					{
						return item is DefaultConstraint;
					}))
					.DefaultValue = value;
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is foreign key.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is foreign key; otherwise, <c>false</c>.
		/// </value>
		public bool IsForeignKey { 
			get {
				return this.HasConstraint(typeof(ForeignKeyConstraint));
			}	 
		}

		public List<ForeignKeyConstraint> ForeignKeys
		{
			get {
				List<ForeignKeyConstraint> list = new List<ForeignKeyConstraint>();
				foreach (ConstraintDefinition item in this.Contraints)
					if (item is ForeignKeyConstraint)
						list.Add((ForeignKeyConstraint)item);
				return list;
			}
		}


		public ConstraintsList Contraints
		{
			get
			{
				this.ConstraintsInitialized = true;
				return this.constraints;
			}
		}

		


		/// <summary>
		/// Gets or sets the adapter.
		/// </summary>
		/// <value>The adapter.</value>
		internal protected Adapter Adapter { get; set; }

		protected bool HasConstraint(Type type)
		{
			foreach(ConstraintDefinition constraint in this.Contraints)
				if(constraint.GetType() == type)
					return true;
				return false;
		}

		/// <summary>
		/// Types to SQL.
		/// </summary>
		/// <returns></returns>
		protected virtual string TypeToSql()
		{
			return this.Adapter.TypeToSql(this.SqlType, this.Limit, this.Precision, this.Scale);
		}

		/// <summary>
		/// Toes the SQL.
		/// </summary>
		/// <returns></returns>
		protected virtual string ToSql() 
		{
			string sql = string.Format("{0} {1}",this.Adapter.QuoteColumnName(this.Name), this.TypeToSql());
			if(!this.SqlType.Contains("primarykey"))
				sql += this.Adapter.AddColumnOptions(new Hash() {{ "null", this.IsNull }, {"default",this.Default}});
			return sql;
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
        public override string  ToString()
        {
			return this.ToSql();        
        }
     
    }
}
