using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data
{
	public abstract class ConstraintDefinition
	{
		private string name;

		private List<string> columnNames;

		public abstract string Prefix { get; }

		public virtual string Name 
		{
			get {
				if (string.IsNullOrEmpty(this.name))
					name = string.Format("{0}_{1}_{2}", this.Prefix, this.TableName, EnumerableUtil.Join(this.ColumnNames, "_AND_"));
				return name;
			}
			set { this.name = value; }
		}

		public string TableName { get; set; }

		public List<string> ColumnNames
		{
			get {
				if (this.columnNames == null)
					this.columnNames = new List<string>();
				return this.columnNames;
			}

			internal protected set
			{
				this.columnNames = value;
			}
		}
	}
}
