

namespace Amplify.Linq
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Reflection;


	public static class IEnumerableMixins
	{
		public static IEnumerable<T> Collect<T>(this IEnumerable<T> obj, Predicate<T> match)
		{
			List<T> list = new List<T>();
			foreach (T item in obj)
				if (match(item))
					list.Add(item);
			return list;
		}

		public static IEnumerable<T> Map<T>(this IEnumerable<T> obj, Predicate<T> match)
		{
			List<T> list = new List<T>();
			foreach (T item in obj)
				if (match(item))
					list.Add(item);
			return list;
		}
		

		public static Hash ToHash<T>(this IEnumerable<T> obj)
		{
			Hash h = new Hash();
			int count = 0;
			foreach (T item in obj)
				h.Add((count++).ToString(), item);
			return h;
		}

		public static Hash ToHash<T>(this IEnumerable<T> obj, Func<T, string> func)
		{
			Hash h = new Hash();
			foreach (T item in obj)
				h.Add(func(item), item);
			return h;
		}

		public static IEnumerable<T> Each<T>(this IEnumerable<T> obj, Action<T> action)
		{
			foreach (T item in obj)
				action(item);

			return obj;
		}

		public static bool Includes<T>(this IEnumerable<T> obj, Predicate<T> match)
		{
			foreach (T item in obj)
				if (match(item))
					return true;
			return false;
		}

		public static bool Includes<T>(this IEnumerable<T> obj, params T[] matches)
		{
			List<T> found = new List<T>();
			foreach (T item in obj) 
			{
				foreach(T match in matches) 
				{
					if(match.Equals(item) && !found.Contains(item)) {
						found.Add(item);
						break;
					}
				}
				if(found.Count == matches.Length)
					return true;
			}

			return (found.Count == matches.Length);
		}

		public static bool Includes<T>(this IEnumerable<T> obj, T match)
		{
			foreach (T item in obj)
				if (item.Equals(match))
					return true;
			return false;
		}

		public static bool IsEmpty<T>(this IEnumerable<T> obj)
		{
			return (obj.Count() == 0);
		}



		
		public static void Merge(this IDictionary obj, IDictionary values)
		{
			foreach (object key in values.Keys)
				obj[key] = values[key];
		}

		public static string Join<T>(this IEnumerable<T> obj, string join)
		{
			string concat = "";
			foreach (T item in obj)
				concat += item.ToString() + join;
			return concat.TrimEnd(join.ToCharArray());
		}

		public static string Join<T>(this IEnumerable<T> obj, string join, Func<T, string> func)
		{
			string concat = "";
			foreach (T item in obj)
				concat +=  func(item) + join;
			return concat.TrimEnd(join.ToCharArray());
		}
	}
}
