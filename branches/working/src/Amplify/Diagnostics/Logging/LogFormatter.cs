using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify.Diagnostics
{
	/// <summary>
	/// Helps to format string codes, %d for date, %t for time, %l for level, %t for thread.
	/// </summary>
	public class LogFormatter
	{

		static LogFormatter()
		{
			EnumType = typeof(LoggingLevel);
		}

		/// <summary>
		/// The Enumeration for the level.
		/// </summary>
		public static Type EnumType { get; set; }

		/// <summary>
		/// Formats the string by replacing symbols in the string.
		/// </summary>
		/// <param name="input">The string message input.</param>
		/// <param name="level">The current level of the message.</param>
		/// <returns>returns the new value of the message.</returns>
		public static string Format(string input, object level)
		{
			return input.Replace("%d",
				DateTime.Now.ToShortDateString()).Replace("%t",
				DateTime.Now.ToLongTimeString()).Replace("%l",
				Enum.GetName(EnumType, level)).Replace("%t", System.Threading.Thread.CurrentThread.Name);
		}
	}
}
