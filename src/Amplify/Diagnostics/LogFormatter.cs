using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify.Diagnostics
{
	public class LogFormatter
	{

		static LogFormatter()
		{
			EnumType = typeof(LogLevel);
		}

		public static Type EnumType { get; set; }

		public static string Format(string format, object level)
		{
			return format.Replace("%d",
				DateTime.Now.ToShortDateString()).Replace("%t",
				DateTime.Now.ToLongTimeString()).Replace("%l",
				Enum.GetName(EnumType, level)).Replace("%t", System.Threading.Thread.CurrentThread.Name);
		}
	}
}
