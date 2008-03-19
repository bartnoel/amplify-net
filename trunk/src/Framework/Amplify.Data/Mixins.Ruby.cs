using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Amplify
{

	public delegate void PairDo<TKey, TValue>(TKey key, TValue value);

	public static class Mixin
	{

		#region Enumerable Methods
		public static T[] Collect<T>(this T[] obj, Predicate<T> match)
		{
			List<T> list = new List<T>();
			foreach (T item in obj)
				if (match(item))
					list.Add(item);
			return list.ToArray();
		}

		public static IEnumerable<T> Collect<T>(this IEnumerable<T> obj, Predicate<T> match)
		{
			List<T> list = new List<T>();
			foreach (T item in obj)
				if (match(item))
					list.Add(item);
			return list;
		}

		public static Hashtable Merge(this Hashtable obj, IDictionary values)
		{
			return (Hashtable)obj.Merge(values);
		}

		public static Dictionary<TKey, TValue> Merge<TKey, TValue>(this Dictionary<TKey, TValue> obj, IDictionary values)
		{
			foreach (object key in obj.Keys)
				if (key is TKey && values[key] is TValue)
					obj[key] = values[key];
			return obj;
		}

		private static IDictionary Merge(this IDictionary obj, IDictionary values)
		{
			foreach (object key in values.Keys)
				obj[key] = values[key];

			return obj;
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
		#endregion


		#region Inflector Methods

		public static string Camelize(this string obj)
		{
			return Inflector.Net.Inflector.Camelize(obj);
		}

		public static string Humanize(this string obj)
		{
			return Inflector.Net.Inflector.Humanize(obj);
		}

		public static string Ordinalize(this string obj)
		{
			return Inflector.Net.Inflector.Ordinalize(obj);
		}

		public static string Singularize(this string obj)
		{
			return Inflector.Net.Inflector.Singularize(obj);
		}

		public static string Pluralize(this string obj)
		{
			return Inflector.Net.Inflector.Pluralize(obj);
		}

		public static string Pascalize(this string obj)
		{
			return Inflector.Net.Inflector.Pascalize(obj);
		}

		public static string Titlize(this string obj)
		{
			return Inflector.Net.Inflector.Titleize(obj);
		}

		public static string Capitalize(this string obj)
		{
			return Inflector.Net.Inflector.Capitalize(obj);
		}

		public static string Dasherize(this string obj)
		{
			return Inflector.Net.Inflector.Dasherize(obj);
		}

		public static string Uncapitalize(this string obj)
		{
			return Inflector.Net.Inflector.Uncapitalize(obj);
		}

		public static string Underscore(this string obj)
		{
			return Inflector.Net.Inflector.Underscore(obj);
		}

		#endregion
	}
}
