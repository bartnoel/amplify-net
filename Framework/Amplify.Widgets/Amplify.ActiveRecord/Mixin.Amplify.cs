
namespace Amplify.ActiveRecord
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Reflection;

	using Amplify.Mixin.Ruby;

	public static class Mixin
	{
		public static string Join(this IEnumerable<string> obj, string concat)
		{
			string value = "";
			obj.Each(s => value += (s + concat));
			return value.TrimEnd(concat.ToCharArray());
		}

		public static bool IsNull(this object obj)
		{
			return (obj == null);
		}

		public static string Wrap(this string obj, string format)
		{
			return string.Format(format, obj);
		}
	}
}
