

namespace Amplify.Linq
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Text;

	using Amplify.Reflection;

	public static class ObjectMixins
	{

		public static Hash Hasherize(this object obj)
		{
			Hash h = new Hash();
			PropertyInfo[] properties = obj.GetType().GetProperties();
			foreach (PropertyInfo info in properties)
				if (info.CanRead)
					h.Add(info.Name, info.GetValue(obj, null));
			return h;
		}

		public static T Default<T>(this object obj, T value)
		{
			return (obj != null) ? (T)obj : value;
		}
	}
}
