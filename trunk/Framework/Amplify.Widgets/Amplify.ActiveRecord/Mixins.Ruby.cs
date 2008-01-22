using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Amplify.Mixin.Ruby
{

	public delegate void PairDo<TKey, TValue>(TKey key, TValue value);

	public static class Mixin
	{

		public static T[] Collect<T>(this T[] obj, Predicate<T> match)
		{
			List<T> list = new List<T>();
			foreach (T item in obj)
				if (match(item))
					list.Add(item);
			return list.ToArray();
		}

		public static List<T> Collect<T>(this List<T> obj, Predicate<T> match)
		{
			List<T> list = new List<T>();
			foreach (T item in obj)
				if (match(item))
					list.Add(item);
			return list;
			string[] test;
		}



		public static void Each<T>(this IEnumerable<T> obj, Action<T> action)
		{
			foreach (T item in obj)
				action(item);
		}



		public static T[] Reverse<T>(this T[] obj)
		{
			List<T> list = new List<T>();
			foreach (T item in obj)
				list.Insert(0, item);
			return list.ToArray();
		}
	}
}
