using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuse.Models
{
	using System.Reflection;

	public class ColumnAttribute : System.Attribute
	{

		public string Name { get; set; }
		public bool IsPrimaryKey { get; set; }
		public string TableName { get; set;}

		public object Default { get; set; }

		public bool IsDesciminatior { get; set; }

		public object GetDefaultValue()
		{
			if (this.Default == null)
				return null;
			else if (this.Default is ValueFactory)
				return ((ValueFactory)Activator.CreateInstance((Type)this.Default)).CreateValue();
			return this.Default;
		}

		public PropertyInfo Property { get; set; }
	}
}
