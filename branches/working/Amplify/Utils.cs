using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify
{
	public class EnumerableUtil
	{

		public static string Join<T>(IEnumerable<T> obj, string delimiter)
		{
			string concat = "";
			foreach (T item in obj)
				concat += item.ToString() + delimiter;
			return concat.TrimEnd(delimiter.ToCharArray());
		}
	}
}
