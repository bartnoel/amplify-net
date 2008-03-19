using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify
{
	public class Store : Dictionary<string, object>
	{

		public Store() { }

		public Store(params object[] values)
		{
			int count = 1;
			string key = "";
			foreach (object value in values)
			{
				if (count % 2 == 0)
					this.Add(key, value);
				else
					key = value.ToString();
				count++;
			}
		}

		new public object this[string key]
		{
			get
			{
				if (this.ContainsKey(key))
					return base[key];
				return null;
			}
			set
			{
				base[key] = value;
			}
		}

		public Store Merge(IDictionary<string, object> values)
		{
			foreach (string key in values.Keys)
				this[key] = values[key];

			return this;
		}

		public static Store CreateLookup(object obj)
		{
			Store store = new Store();
			System.Reflection.PropertyInfo[] properties = obj.GetType().GetProperties();
			foreach (System.Reflection.PropertyInfo info in properties)
			{
				if (info.CanRead)
					store.Add(info.Name, info.GetValue(obj, null));
			}
			return store;
		}
	}
}
