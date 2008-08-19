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
	public class ForeignKeyDefinition
	{
		private string name = "";


		/// <summary>
		/// Initializes a new instance of the <see cref="ForeignKeyDefinition"/> class.
		/// </summary>
		public ForeignKeyDefinition()
		{
			this.OnDelete = ConstraintDeleteAction.None;
			this.OnUpdate = ConstraintUpdateAction.None;
			this.ReferenceColumnName = "";
			this.ReferenceTableName = "";
			this.PrimaryColumnName = "";
			this.PrimaryTableName = "";
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name 
		{	
			get 
			{
				if (string.IsNullOrEmpty(this.name))
					return "FK_" + this.ReferenceTableName + "_" + this.ReferenceColumnName;
				return this.Name;
			} 
			set { this.name = value; } 
		}

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

		/// <summary>
		/// Gets or sets the name of the reference column.
		/// </summary>
		/// <value>The name of the reference column.</value>
		public string ReferenceColumnName { get; set; }

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

	}
}
