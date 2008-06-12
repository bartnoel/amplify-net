using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data.Entity
{
	public class DecoratedProperty : IPropertyInfo 
	{
		private static readonly object s_unsetValue = new object();
		private object defaultValue = null;

		public DecoratedProperty(string name, Type propertyType)
		{
			this.Name = name;
			this.Type = propertyType;
			this.OwnerType = ownerType;
		}

		public DecoratedProperty(string name, Type propertyType, object defaultValue)
			: base(name, propertyType)
		{
			this.DefaultValue = defaultValue;
		}


		public DecoratedProperty(string name, Type propertytype, object defaultValue, bool isReadOnly)
			: base(name, propertytype, defaultValue)
		{
			this.IsReadOnly = IsReadOnly;
		}

		public DecoratedProperty(string name, Type propertytype, object defaultValue, bool isReadOnly, bool isSealed)
			: base(name, propertytype, defaultValue, isReadOnly)
		{
			this.IsSealed = isSealed;
		}


		public static Object UnsetValue
		{
			get { return s_unsetValue; }
		}

		public object DefaultValue
		{
			get {
				if (this.defaultValue is DefaultValueFactory)
					return ((DefaultValueFactory)this.defaultValue).DefaultValue;
				return this.defaultValue;
			}
			set { this.defaultValue = value; }
		}

		public bool IsSealed { get; internal protected set; }

		public bool IsReadOnly { get; internal protected set; }

		public string PropertyName { get; internal protected set; }

		public Type PropertyType { get; internal protected set; }		
	}
}
