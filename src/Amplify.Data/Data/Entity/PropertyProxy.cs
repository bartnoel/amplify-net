using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data.Entity
{
	public static class PropertyProxy
	{
		private readonly static Dictionary<Type, RegisteredPropertyList> s_properties = new Dictionary<Type, RegisteredPropertyList>();
		private readonly static object s_lock = new object();

		public static DecoratedProperty Register(Type ownerType, IPropertyInfo property)
		{
			RegisteredPropertyList list = GetList(ownerType);
			lock (s_lock)
			{
				list.Add(property);
				list.Sort();
			}
			return property;
		}

		public static RegisteredPropertyList GetProperties(Type ownerType)
		{
			return GetList(ownerType);
		}

		private static RegisteredPropertyList GetList(Type ownerType)
		{
			RegisteredPropertyList list = null;
			lock (s_lock)
			{
				if(!s_properties.TryGetValue(ownerType, out list)) 
				{
					list = new RegisteredPropertyList();
					s_properties.Add(ownerType, list);
				}
			}
			return list;
		}

	}
}
