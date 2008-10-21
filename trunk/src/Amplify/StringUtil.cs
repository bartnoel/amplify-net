namespace Amplify
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;



	public class StringUtil
	{

		/// <summary>
		/// Grep and substring static utility method. 
		/// </summary>
		/// <param name="input">The string to be parsed.</param>
		/// <param name="pattern">The regular expression pattern (uses ECMAScript pattern by default)</param>
		/// <param name="replace">The replacement string.</param>
		/// <returns>returns the string with the replaced values. </returns>
		public static string Gsub(string input, string pattern, string replace)
		{
			return Regex.Replace(input, pattern, replace, RegexOptions.ECMAScript);
		}

		/// <summary>
		/// Grep and substring static utility method. 
		/// </summary>
		/// <param name="input">The string to be parsed.</param>
		/// <param name="pattern">The regular expression pattern.</param>
		/// <param name="replace">The replacement string</param>
		/// <param name="options">The <see cref="System.Text.RegularExpressions.RegexOptions"/></param>
		/// <returns>returns the string with the replaced values.</returns>
		public static string Gsub(string input, string pattern, string replace, RegexOptions options)
		{
			return Regex.Replace(input, pattern, replace, options);
		}

		/// <summary>
		/// Grep and substring static utility method. 
		/// </summary>
		/// <param name="input">The string to be parsed.</param>
		/// <param name="pattern">The regular expression pattern.</param>
		/// <param name="evaluator">The <see cref="System.Text.RegularExpressions.MatchEvaluator"/> evalutor for replacing the value.</param>
		/// <param name="options">The <see cref="System.Text.RegularExpressions.RegexOptions"/></param>
		/// <returns>returns the string with the replaced values.</returns>
		public static string Gsub(string input, string pattern, MatchEvaluator evaluator, RegexOptions options)
		{
			return Regex.Replace(input, pattern, evaluator, options);
		}

		/// <summary>
		/// Grep and substring static utility method. 
		/// </summary>
		/// <param name="input">The string to be parsed.</param>
		/// <param name="pattern">The regular expression pattern. (uses ECMAScript pattern)</param>
		/// <param name="evaluator">The <see cref="System.Text.RegularExpressions.MatchEvaluator"/> evalutor for replacing the value.</param>
		/// <returns>returns the string with the replaced values.</returns>
		public static string Gsub(string input, string pattern, MatchEvaluator evaluator)
		{
			return Regex.Replace(input, pattern, evaluator, RegexOptions.ECMAScript);
		}

		/// <summary>
		/// Gets the <see cref="System.Text.RegularExpressions.Match"/> for the specified pattern.
		/// </summary>
		/// <param name="input">The string to be parsed.</param>
		/// <param name="pattern">The regular expression pattern. (uses ECMAScript pattern)</param>
		/// <returns>returns the <see cref="System.Text.RegularExpressions.Match"/> value. </returns>
		public static Match Match(string input, string pattern)
		{
			return Regex.Match(input, pattern, RegexOptions.ECMAScript);
		}

		/// <summary>
		/// Gets the <see cref="System.Text.RegularExpressions.Match"/> for the specified pattern.
		/// </summary>
		/// <param name="input">The string to be parsed.</param>
		/// <param name="options">The <see cref="System.Text.RegularExpressions.RegexOptions"/></param>
		/// <returns>returns the <see cref="System.Text.RegularExpressions.Match"/> value. </returns>
		public static Match Match(string input, string pattern, RegexOptions options)
		{
			return Regex.Match(input, pattern, options);
		}

		/// <summary>
		/// Returns true if the string finds a pattern match. 
		/// Uses <see cref="System.Text.RegularExpressions.RegexOptions"/>.ECMAScript by default.
		/// </summary>
		/// <param name="input">The string to be parsed.</param>
		/// <param name="pattern">The regular expression pattern. (uses ECMAScript pattern)</param>
		/// <returns> returns true if the pattern finds a match. </returns>
		public static bool IsMatch(string input, string pattern)
		{
			return Regex.IsMatch(input, pattern, RegexOptions.ECMAScript);
		}

		/// <summary>
		/// Returns true if the string finds a pattern match. 
		/// Uses <see cref="System.Text.RegularExpressions.RegexOptions"/>.ECMAScript by default.
		/// </summary>
		/// <param name="input">The string to be parsed.</param>
		/// <param name="options">The <see cref="System.Text.RegularExpressions.RegexOptions"/></param>
		/// <returns> returns true if the pattern finds a match. </returns>
		public static bool IsMatch(string input, string pattern, RegexOptions options)
		{
			return Regex.IsMatch(input, pattern, options);
		}

		/// <summary>
		/// Parses a string into an <see cref="System.Int32"/> value.
		/// </summary>
		/// <param name="input">The string to be parsed.</param>
		/// <returns>returns an integer value (0 if it could not be parsed).</returns>
		public static int ToInt(string input) 
		{
			int i = default(int);
			int.TryParse(input, out i);
			return i;
		}

		/// <summary>
		/// Parses a string into a <see cref="System.DateTime"/> value.
		/// </summary>
		/// <param name="input">The string to be parsed.</param>
		/// <returns>returns an DateTime value.</returns>
		public static DateTime ToDateTime(string input)
		{
			DateTime date = default(DateTime);
			DateTime.TryParse(input, out date);
			return date;
		}

		/// <summary>
		/// Parses a string into just the Date portion of the DateTime object.
		/// </summary>
		/// <param name="input">The string to be parsed.</param>
		/// <returns>returns the DateTime.Date portion of the object.</returns>
		public static DateTime ToDate(string input)
		{
			return ToDateTime(input).Date;
		}

		/// <summary>
		/// Parses a string into a <see cref="System.TimeSpan"/> value.
		/// </summary>
		/// <param name="input">The string to be parsed.</param>
		/// <returns>returns a TimeSpan value.</returns>
		public static TimeSpan ToTime(string input)
		{
			return ToDateTime(input).TimeOfDay;
		}

		/// <summary>
		/// Trims the specified input.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="delimiter">The delimiter.</param>
		/// <returns></returns>
		public static string Trim(string input, string delimiter)
		{
			return input.Trim(delimiter.ToCharArray());
		}


		/// <summary>
		/// Trims the start of the string.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="delimiter">The delimiter.</param>
		/// <returns>a string</returns>
		public static string TrimStart(string input, string delimiter)
		{
			return input.TrimStart(delimiter.ToCharArray());
		}

		/// <summary>
		/// Trims the end of the string.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="delimiter">The delimiter.</param>
		/// <returns>a string</returns>
		public static string TrimEnd(string input, string delimiter)
		{
			return input.TrimEnd(delimiter.ToCharArray());
		}

		/// <summary>
		/// Splits the string input into a string array.
		/// </summary>
		/// <param name="input">The string to be parsed.</param>
		/// <param name="delimiter">The string delimiter.</param>
		/// <returns>returns an array of strings.</returns>
		public static string[] Split(string input, string delimiter) 
		{
			return input.Split(delimiter.ToCharArray());
		}

		/// <summary>
		/// Splits the string and performs an action on each part.
		/// </summary>
		/// <param name="input">The string to be parsed.</param>
		/// <param name="delimiter">The string delimiter.</param>
		/// <param name="action"></param>
		/// <returns>Returns the string of concatinated values after the action is performed.</returns>
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
