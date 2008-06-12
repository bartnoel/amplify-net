using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Amplify
{
	public class StringUtil
	{

		public static string Gsub(string input, string pattern, string replace)
		{
			return Regex.Replace(input, pattern, replace, RegexOptions.ECMAScript);
		}

		public static string Gsub(string input, string pattern, string replace, RegexOptions options)
		{
			return Regex.Replace(input, pattern, replace, options);
		}

		public static string Gsub(string input, string pattern, MatchEvaluator evaluator, RegexOptions options)
		{
			return Regex.Replace(input, pattern, evaluator, options);
		}

		public static string Gsub(string input, string pattern, MatchEvaluator evaluator)
		{
			return Regex.Replace(input, pattern, evaluator, RegexOptions.ECMAScript);
		}

		public static Match Match(string input, string pattern)
		{
			return Regex.Match(input, pattern, RegexOptions.ECMAScript);
		}

		public static Match Match(string input, string pattern, RegexOptions options)
		{
			return Regex.Match(input, pattern, options);
		}

		/// <summary>
		/// Returns true if the string finds a pattern match. 
		/// Uses <see cref="System.Text.RegularExpressions.RegexOptions"/>.ECMAScript by default.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="pattern"></param>
		public static bool IsMatch(string input, string pattern)
		{
			return Regex.IsMatch(input, pattern, RegexOptions.ECMAScript);
		}

		public static bool IsMatch(string input, string pattern, RegexOptions options)
		{
			return Regex.IsMatch(input, pattern, options);
		}

		

		public static int ToInt(string input) 
		{
			int i = default(int);
			int.TryParse(input, out i);
			return i;
		}

		public static DateTime ToDateTime(string input)
		{
			DateTime date = default(DateTime);
			DateTime.TryParse(input, out date);
			return date;
		}

		public static DateTime ToDate(string input)
		{
			return ToDateTime(input).Date;
		}

		public static TimeSpan ToTime(string input)
		{
			return ToDateTime(input).TimeOfDay;
		}

		public static string[] Split(string input, string delimiter) 
		{
			return input.Split(delimiter.ToCharArray());
		}

		public static string Each(string input, string delimiter, Action<string> action)
		{
			string[] parts = Split(input, delimiter);
			string returnValue = "";

			foreach (string part in parts)
			{
				action(part);
				returnValue += part;
			}

			return returnValue;
		}
	}
}
