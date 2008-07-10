//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.ActiveRecord
{
	using System;
	using System.Reflection;
	using ObjectModel;

	public class ColumnAttribute : System.Attribute
	{
		public string ColumnName { get; set; }
		public string TableName { get; set; }

		public bool IsPrimaryKey { get; set; }
		public bool IsDescriminator { get; set; }
		public int? Limit { get; set; }
		public int? Precision { get; set; }
		public int? Scale { get; set; }

		public object Descrimination { get; set; }

		public object Default { get; set; }

		public PropertyInfo Property { get; set; }

		public object GetDesicriminator(Type type)
		{
			if (this.Descrimination == null)
				return null;
			else
				return this.Descrimination;
		}
		
		public object GetDefaultValue()
		{
			if (this.Default == null)
				return null;
			else if (this.Default is Type)
				return ((ValueFactory)Activator.CreateInstance((Type)this.Default)).CreateValue();
			return this.Default;
		}

		
	}
}
