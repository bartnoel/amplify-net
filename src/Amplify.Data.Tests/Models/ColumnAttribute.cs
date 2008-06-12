using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Model
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]
	public class ColumnAttribute : System.Attribute
	{
		public bool IsPrimaryKey { get; set; }
		public bool IsIndex { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public object Default { get; set; }
		public int? Limit { get; set; }
		public int? Percision { get; set; }
		public int? Scale { get; set; }
		public bool Null { get; set; }
		public string Table { get; set; }

		public string PropertyName { get; set; }
		public object DefaultValue { get; set; }

		public ColumnAttribute(string name)
		{
			this.Name = name;
			this.IsPrimaryKey = false;
			this.IsIndex = false;
			this.Name = null;
		}
	}
}
