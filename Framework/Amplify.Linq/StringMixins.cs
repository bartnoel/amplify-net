namespace Amplify.Linq
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;

	public static class StringMixins
	{
		#region Gsub
		/// <summary>
		/// Returns a string that has the replaced values that matched the pattern
		/// </summary>
		/// <param name="obj">String</param>
		/// <param name="pattern">The regular expressions pattern.</param>
		/// <param name="replacement">The replacement string.</param>
		/// <returns>A string with the replaced values.</returns>
		public static string Gsub(this string obj, string pattern, string replacement)
		{
			return Regex.Replace(obj, pattern, replacement);
		}

		/// <summary>
		/// Returns a string that has the replaced values that matched the pattern
		/// </summary>
		/// <param name="obj">String</param>
		/// <param name="pattern">The regular expressions pattern.</param>
		/// <param name="replacement">The replacement string.</param>
		/// <param name="options">Regular expression options such as ignoring the case.</param>
		/// <returns>A string with the replaced values.</returns>
		public static string Gsub(this string obj, string pattern, string replacement, RegexOptions options)
		{
			return Regex.Replace(obj, pattern, replacement, options);
		}

		public static string Gsub(this string obj, string pattern, MatchEvaluator evaluator)
		{
			return Regex.Replace(obj, pattern, evaluator);
		}

		public static string Gsub(this string obj, string pattern, MatchEvaluator evaluator, RegexOptions options)
		{
			return Regex.Replace(obj, pattern, evaluator, options);
		}
		#endregion

		#region Match
		public static bool Match(this string obj, string pattern, RegexOptions options)
		{
			return Regex.IsMatch(obj, pattern, options);
		}

		public static bool Match(this string obj, string pattern)
		{
			return Regex.IsMatch(obj, pattern);
		}
		#endregion

		#region Split
		public static string[] Split(this string obj, string split)
		{
			return obj.Split(split.ToCharArray());
		}
		#endregion

		#region Inject
		public static string Inject(this string obj, params object[] values)
		{
			return string.Format(obj, values);
		}

		public static string Inject(this string obj, IFormatProvider provider, params object[] values)
		{
			return string.Format(provider, obj, values);
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

		#region Each
		public static string Each(this string obj, string split, Action<string> action)
		{
			string[] parts = obj.Split(split);
			foreach (string part in parts)
				action(part);

			return obj;
		}
		#endregion

		public static int ToInt(this string obj)
		{
			return int.Parse(obj);
		}

		public static DateTime ToDateTime(this string obj)
		{
			return DateTime.Parse(obj);
		}
	}
}
