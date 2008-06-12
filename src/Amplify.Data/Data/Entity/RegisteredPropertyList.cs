using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data.Entity
{
	public class RegisteredPropertyList : List<IPropertyInfo>, IService  
	{
		public IPropertyInfo this[string propertyName]
		{
			get {
				foreach (IPropertyInfo  item in this)
					if (item.PropertyName.Equals(propertyName))
						return item;
				return null;
			}
		}
	}
}
