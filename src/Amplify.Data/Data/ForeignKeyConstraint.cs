using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data
{

	public enum ConstraintDeleteAction
	{
		None,
		Cascade
	}

	public enum ConstraintUpdateAction
	{
		None,
		Cascade
	}

	/// <summary>
	/// 
	/// </summary>
	public class ForeignKeyConstraint : KeyConstraint
	{
		private string name = "";
		private List<string> referenceColumns;


		/// <summary>
		/// Initializes a new instance of the <see cref="ForeignKeyDefinition"/> class.
		/// </summary>
		public ForeignKeyConstraint()
		{
			this.OnDelete = ConstraintDeleteAction.None;
			this.OnUpdate = ConstraintUpdateAction.None;
			this.ReferenceColumnNames = "";
			this.ReferenceTableName = "";
			this.PrimaryColumnName = "";
			this.PrimaryTableName = "";
		}

		internal protected Adapter Adapter { get; set; }

		public override string Prefix
		{
			get { return "FK"; }
		}

		

		public ColumnDefinition Column { get; set; }

		/// <summary>
		/// Gets or sets the name of the primary column.
		/// </summary>
		/// <value>The name of the primary column.</value>
		public string PrimaryColumnName { get; set; }

		/// <summary>
		/// Gets or sets the name of the primary table.
		/// </summary>
		/// <value>The name of the primary table.</value>
		public string PrimaryTableName { get; set; }

		/// <summary>
		/// Gets or sets the name of the reference table.
		/// </summary>
		/// <value>The name of the reference table.</value>
		public string ReferenceTableName { get; set; }


		public List<String> ReferenceColumns
		{
			get {
				if (this.referenceColumns == null)
					this.referenceColumns = new List<string>();
				return this.referenceColumns;
			}
		}

		/// <summary>
		/// Gets or sets the name of the reference column.
		/// </summary>
		/// <value>The name of the reference column.</value>
		public string ReferenceColumnNames { 
			get 
			{
				return EnumerableUtil.Join(this.ReferenceColumns, ",");
			} 
			set 
			{
				this.ReferenceColumns.Clear();
				if (value != null)
				{
					string[] columns = StringUtil.Split(value, ",");
					foreach (string column in columns)
							this.ReferenceColumns.Add(column);
				}
			} 
		}

		/// <summary>
		/// Gets or sets the on delete.
		/// </summary>
		/// <value>The on delete.</value>
		public ConstraintDeleteAction OnDelete { get; set; }

		/// <summary>
		/// Gets or sets the on update.
		/// </summary>
		/// <value>The on update.</value>
		public ConstraintUpdateAction OnUpdate { get; set; }

		public override string ToString()
		{
			return this.Adapter.BuildCreateTableForeignKey(this);
		}

	}
}
