using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Models
{
	using Amplify.Data;

	[AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]
	public class ColumnAttribute : System.Attribute, IColumnDescriptor 
	{
		private object defaultValue = null;
		private string propertyName = null;

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
	
		public Type PropertyType { get; set; }


		public string PropertyName
		{
			get
			{
				if (propertyName == null)
					return this.Name;
				return this.propertyName;
			}
			set {
				if (this.Name == null)
					this.Name = value;
				if(!Object.Equals(this.Name, value))
					this.propertyName = value; 
			}
		}

		public object DefaultValue
		{
			get { 
				if(defaultValue == null) 
				{
					if (PropertyType == typeof(Guid))
						return Guid.NewGuid();
					if (PropertyType == typeof(DateTime))
						return DateTime.Now;
				}
				return defaultValue;
			}
			set
			{
				this.defaultValue = value;
			}

		}


		public ColumnAttribute()
		{
			this.IsPrimaryKey = false;
			this.IsIndex = false;
			this.Name = null;
		}


		public ColumnAttribute(string name) : this()
		{
			this.Name = name;
		}

		public ColumnAttribute(string name, string tableName):this(name)
		{
			this.Table = tableName;
		}
	}
}
