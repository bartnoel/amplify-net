using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify.Diagnostics
{
	using System.Diagnostics;

	public delegate bool EvaluateLevelHandler(object level);

	/// <summary>
	/// Static Loggging Class with a default console appender. 
	/// </summary>
	public class Log  
	{

		static Log()
		{
			Level = 0;
			On = true;
			Appenders.Add(new ConsoleAppender());
		}

		private static List<ILogAppender> appenders;

		/// <summary>
		/// Gets the list of ILogApenders that is currently in rotation. 
		/// </summary>
		public static List<ILogAppender> Appenders
		{
			get {
				if (appenders == null)
					appenders = new List<ILogAppender>();
				return appenders;
			}
		}

		/// <summary>
		/// The current level of logging.
		/// </summary>
		public static Object Level { get; set; }

		/// <summary>
		/// Gets or sets whether the log is on or not.
		/// </summary>
		public static bool On { get; set; }

		/// <summary>
		/// Called to invaluate the level in case a custom level is used.
		/// </summary>
		public static EvaluateLevelHandler EvaluateLevel;

		/// <summary>
		/// Writes out debug information.
		/// </summary>
		/// <param name="message">The object message.</param>
	
		public static void Debug(object message)
		{
			WriteLine(message, 5);
		}

		/// <summary>
		/// Writes out debug information.
		/// </summary>
		/// <param name="message">The object message.</param>
		/// <param name="values">The values that will be formated into the string.</param>
		public static void Debug(object message, params object[] values)
		{
			WriteLine(message, 5, values);
		}

		/// <summary>
		/// Writes out sql information.
		/// </summary>
		/// <param name="message">The object message.</param>
		public static void Sql(object message)
		{
			WriteLine(message, 0);
		}

		/// <summary>
		/// Writes out sql information.
		/// </summary>
		/// <param name="message">The object message.</param>
		/// <param name="values">The values that will be formatted into the string.</param>
		public static void Sql(object message, params object[] values)
		{
			WriteLine(message, 0, values);
		}

		/// <summary>
		/// Writes out informative information.
		/// </summary>
		/// <param name="message">The object message.</param>
		public static void Info(object message)
		{
			WriteLine(message, 4);
		}

		/// <summary>
		/// Writes out informative information.
		/// </summary>
		/// <param name="message">The object message.</param>
		/// <param name="values">The values that will be formatted into the string.</param>
		public static void Info(object message, params object[] values)
		{
			WriteLine(message, 4, values);
		}

		/// <summary>
		/// Writes out warning information.
		/// </summary>
		/// <param name="message">The object message.</param>
		public static void Warn(object message)
		{
			WriteLine(message, 3);
		}

		/// <summary>
		/// Writes out warning information.
		/// </summary>
		/// <param name="message">The object message.</param>
		/// <param name="values">The values that will be formatted into the string.</param>
		public static void Warn(object message, params object[] values)
		{
			WriteLine(message, 3, values);
		}

		/// <summary>
		/// Writes out the Expception.
		/// </summary>
		/// <param name="ex">The <see cref="System.Exception"/></param>
		public static void Exception(Exception ex)
		{
			Write(ex);
		}


		/// <summary>
		/// Writes out the information about anything would could cause a fatal exception.
		/// </summary>
		/// <param name="message">The object message.</param>
		public static void Fatal(object message)
		{
			WriteLine(message, 1);
		}

		/// <summary>
		/// Writes out the information about anything would could cause a fatal exception.
		/// </summary>
		/// <param name="message">The object message.</param>
		/// <param name="values">The values that will be formatted into the string.</param>
		public static void Fatal(object message, params object[] values)
		{
			WriteLine(message, 1, values);
		}

		

		/// <summary>
		/// Writes a line to the log. 
		/// </summary>
		/// <param name="message">The message that will be written.</param>
		/// <param name="level">The level of the message.</param>
		/// <param name="values">The values that will be formatted into the string.</param>
		public static void WriteLine(object message, object level, params object[] values)
		{
			if (ShouldWrite(level))
				appenders.ForEach(delegate(ILogAppender item) {
					item.WriteLine(message, level, values);
				});
		}

		/// <summary>
		/// Writes a line to the log.
		/// </summary>
		/// <param name="message">The message that will be written.</param>
		/// <param name="level">The level of the message.</param>
		public static void WriteLine(object message, object level)
		{
			if(ShouldWrite(level)) 
				appenders.ForEach(delegate(ILogAppender item)
				{
					item.WriteLine(message, level);
				});
		}

		/// <summary>
		/// Writes a line to the log.
		/// </summary>
		/// <param name="message">The message that will be written.</param>
		public static void WriteLine(object message)
		{
			appenders.ForEach(delegate(ILogAppender item)
			{
				item.WriteLine(message);
			});
		}

		/// <summary>
		/// Writes the exception to the log.
		/// </summary>
		/// <param name="ex">The <see cref="System.Exception"/></param>
		public static void Write(Exception ex)
		{
			appenders.ForEach(delegate(ILogAppender item)
			{
				item.WriteLine(ex);
			});
		}

		/// <summary>
		/// Writes the message to the log.
		/// </summary>
		/// <param name="message">The object message.</param>
		/// <param name="level">The level of the message.</param>
		/// <param name="values">The values that will be formatted into the message.</param>
		public static void Write(object message, object level, params object[] values)
		{
			if (ShouldWrite(level))
				appenders.ForEach(delegate(ILogAppender item)
				{
					item.Write(message, level, values);
				});
		}

		/// <summary>
		/// Writes the message to the log.
		/// </summary>
		/// <param name="message">The object message.</param>
		/// <param name="level">The level of the message.</param>
		public static void Write(object message, object level)
		{
			if (ShouldWrite(level))
				appenders.ForEach(delegate(ILogAppender item)
				{
					item.Write(message, level);
				});
		}

		/// <summary>
		/// Writes the message to the log.
		/// </summary>
		/// <param name="message">The object message.</param>
		public static void Write(object message)
		{
			appenders.ForEach(delegate(ILogAppender item)
			{
				item.WriteLine(message);
			});
		}

		private static bool ShouldWrite(object level)
		{
			if (!On)
				return false;

			if (level is Int32 && Level is Int32)
			{
				if ((int)level < (int)Level)
					return true;
			}

			EvaluateLevelHandler eh = Log.EvaluateLevel;
			if (eh != null)
				return eh(level);

			return true;
		}
	}

	public enum LogLevel
	{
		Sql,
		Fatal, 
		Exception, 
		Warn,
		Info,
		Debug
	}
}
