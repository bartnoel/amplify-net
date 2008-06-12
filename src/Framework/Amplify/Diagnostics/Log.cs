using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify.Diagnostics
{
	public delegate bool EvaluateLevelHandler(object level);

	public class Log  
	{

		static Log()
		{
			Level = 0;
			On = true;
			Appenders.Add(new ConsoleAppender());
		}

		private static List<ILogApender> appenders;

		public static List<ILogApender> Appenders
		{
			get {
				if (appenders == null)
					appenders = new List<ILogApender>();
				return appenders;
			}
		}

		public static Object Level { get; set; }

		public static bool On { get; set; }

		public static EvaluateLevelHandler EvaluateLevel;

		public static void Debug(object message)
		{
			WriteLine(message, 0);
		}

		public static void Debug(object message, params object[] values)
		{
			WriteLine(message, 0, values);
		}

		public static void Sql(object message)
		{
			WriteLine(message, 1);
		}

		public static void Sql(object message, params object[] values)
		{
			WriteLine(message, 1, values);
		}

		public static void Info(object message)
		{
			WriteLine(message, 2);
		}

		public static void Info(object message, params object[] values)
		{
			WriteLine(message, 2, values);
		}

		public static void Warn(object message)
		{
			WriteLine(message, 3);
		}

		public static void Warn(object message, params object[] values)
		{
			WriteLine(message, 3, values);
		}

		public static void Exception(Exception ex)
		{
			Write(ex);
		}

		public static void Fatal(object message, params object[] values)
		{
			WriteLine(message, 5, values);
		}

		public static void Fatal(object message)
		{
			WriteLine(message, 5);
		}


		public static void WriteLine(object message, object level, params object[] values)
		{
			if (ShouldWrite(level))
				appenders.ForEach(delegate(ILogApender item) {
					item.WriteLine(message, level, values);
				});
		}

		public static void WriteLine(object message, object level)
		{
			if(ShouldWrite(level)) 
				appenders.ForEach(delegate(ILogApender item)
				{
					item.WriteLine(message, level);
				});
		}

		public static void WriteLine(object message)
		{
			appenders.ForEach(delegate(ILogApender item)
			{
				item.WriteLine(message);
			});
		}

		public static void Write(Exception ex)
		{
			appenders.ForEach(delegate(ILogApender item)
			{
				item.WriteLine(ex);
			});
		}

		public static void Write(object message, object level, params object[] values)
		{
			if (ShouldWrite(level))
				appenders.ForEach(delegate(ILogApender item)
				{
					item.Write(message, level, values);
				});
		}

		public static void Write(object message, object level)
		{
			if (ShouldWrite(level))
				appenders.ForEach(delegate(ILogApender item)
				{
					item.Write(message, level);
				});
		}

		public static void Write(object message)
		{
			appenders.ForEach(delegate(ILogApender item)
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
					return false;
			}

			EvaluateLevelHandler eh = Log.EvaluateLevel;
			if (eh != null)
				return eh(level);

			return true;
		}
	}

	public enum LogLevel
	{
		Debug,
		Sql,
		Info,
		Warn,
		Exception,
		Fatal,
	}
}
