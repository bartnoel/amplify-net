using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuse.Models
{

	[AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
	public class JoinedTable : TableAttribute
	{
		private string foreignKey = "";

		public string ForeignKey 
		{
			get
			{
				if (!string.IsNullOrEmpty(this.foreignKey))
					return this.foreignKey;
				return this.TableName + "Id";
			}
			set { this.foreignKey = value; }
		}

		public override bool IsPrimary
		{
			get
			{
				return false;
			}
			set
			{
				
			}
		}
	}

	public class PrimaryTableAttribute : TableAttribute
	{

		public override bool IsPrimary
		{
			get
			{
				return true;
			}
			
		}
	}

	public abstract class TableAttribute : System.Attribute 
	{
		private string alias = "";
		private List<ColumnAttribute> columns;

		public string TableName { get; set; }

		public string Selection { get; set; }

		public List<ColumnAttribute> Columns
		{
			get {
				if (this.columns == null)
					this.columns = new List<ColumnAttribute>();
				return this.columns;
			}
		}

		public virtual bool IsPrimary { get; set; }

		public string As
		{
			get
			{
				if (string.IsNullOrEmpty(this.alias))
					return this.TableName;
				return this.alias;
			}
			set { this.alias = value; }
		}
	}
}
