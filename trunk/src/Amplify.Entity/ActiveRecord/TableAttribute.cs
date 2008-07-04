using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.ActiveRecord
{

	

	

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
