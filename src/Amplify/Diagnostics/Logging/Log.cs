using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify.Diagnostics.Logging
{
	using System.Diagnostics;

	public delegate bool EvaluateLevelHandler(object level);

	/// <summary>
	/// Static Loggging Class with a default console appender. 
	/// </summary>
	public class Log : ICloneable 
	{
		internal protected string ClassName { get; set; }

		static Log()
		{
			
		}

		private List<IAppender> appenders;

		/// <summary>
		/// Gets the list of ILogApenders that is currently in rotation. 
		/// </summary>
		public List<IAppender> Appenders
		{
			get {
				if (appenders == null)
					appenders = new List<IAppender>();
				return appenders;
			}
		}

		/// <summary>
		/// The current level of logging.
		/// </summary>
		public Object Level { get; set; }

		/// <summary>
		/// Gets or sets whether the log is on or not.
		/// </summary>
		public bool On { get; set; }

		/// <summary>
		/// Called to invaluate the level in case a custom level is used.
		/// </summary>
		public EvaluateLevelHandler EvaluateLevel;

		/// <summary>
		/// Writes out debug information.
		/// </summary>
		/// <param name="message">The object message.</param>
	
		public void Debug(object message)
		{
			//WriteLine(message, 5);
		}

		/// <summary>
		/// Writes out debug information.
		/// </summary>
		/// <param name="message">The object message.</param>
		/// <param name="values">The values that will be formated into the string.</param>
		public void Debug(object message, params object[] values)
		{
			//WriteLine(message, Level.Debug, values);
		}

		/// <summary>
		/// Writes out sql information.
		/// </summary>
		/// <param name="message">The object message.</param>
		public void Sql(object message)
		{
			//WriteLine(message, Level.Sql);
		}

		/// <summary>
		/// Writes out sql information.
		/// </summary>
		/// <param name="message">The object message.</param>
		/// <param name="values">The values that will be formatted into the string.</param>
		public void Sql(object message, params object[] values)
		{
			//WriteLine(message, Level.Sql, values);
		}

		/// <summary>
		/// Writes out informative information.
		/// </summary>
		/// <param name="message">The object message.</param>
		public void Info(object message)
		{
			//WriteLine(message, 4);
		}

		/// <summary>
		/// Writes out informative information.
		/// </summary>
		/// <param name="message">The object message.</param>
		/// <param name="values">The values that will be formatted into the string.</param>
		public void Info(object message, params object[] values)
		{
			//WriteLine(message, Level.Info, values);
		}

		/// <summary>
		/// Writes out warning information.
		/// </summary>
		/// <param name="message">The object message.</param>
		public void Warn(object message)
		{
			//WriteLine(message,Level.Warn);
		}

		/// <summary>
		/// Writes out warning information.
		/// </summary>
		/// <param name="message">The object message.</param>
		/// <param name="values">The values that will be formatted into the string.</param>
		public void Warn(object message, params object[] values)
		{
			//WriteLine(message, Level.Warn, values);
		}

		/// <summary>
		/// Writes out the Expception.
		/// </summary>
		/// <param name="ex">The <see cref="System.Exception"/></param>
		public void Exception(Exception ex)
		{
			//Write(ex);
		}


		/// <summary>
		/// Writes out the information about anything would could cause a fatal exception.
		/// </summary>
		/// <param name="message">The object message.</param>
		public void Fatal(object message)
		{
			//WriteLine(message, Level.Fatal);
		}

		/// <summary>
		/// Writes out the information about anything would could cause a fatal exception.
		/// </summary>
		/// <param name="message">The object message.</param>
		/// <param name="values">The values that will be formatted into the string.</param>
		public void Fatal(object message, params object[] values)
		{
			//WriteLine(message, Level.Fatal, values);
		}

		

		/// <summary>
		/// Writes a line to the log. 
		/// </summary>
		/// <param name="message">The message that will be written.</param>
		/// <param name="level">The level of the message.</param>
		/// <param name="values">The values that will be formatted into the string.</param>
		public void WriteLine(object message, object level, params object[] values)
		{
			//if (ShouldWrite(level))
			//    foreach (IAppender appender in this.Appenders)
			//        appender.WriteLine(message, level, values);
		}

		

		/// <summary>
		/// Writes a line to the log.
		/// </summary>
		/// <param name="message">The message that will be written.</param>
		public void WriteLine(object message)
		{
			//foreach (IAppender appender in this.Appenders)
			//    appender.WriteLine(message);
		}

		/// <summary>
		/// Writes the exception to the log.
		/// </summary>
		/// <param name="ex">The <see cref="System.Exception"/></param>
		public void Write(Exception ex)
		{
			//foreach (IAppender appender in this.Appenders)
			//    appender.Write(ex);
		}

		/// <summary>
		/// Writes the message to the log.
		/// </summary>
		/// <param name="message">The object message.</param>
		/// <param name="level">The level of the message.</param>
		/// <param name="values">The values that will be formatted into the message.</param>
		public void Write(object message, object level, params object[] values)
		{
			/*
			if (ShouldWrite(level))
				foreach (IAppender appender in this.Appenders)
					appender.Write(message, level, values);
			 */
		}

		

		/// <summary>
		/// Writes the message to the log.
		/// </summary>
		/// <param name="message">The object message.</param>
		public void Write(object message)
		{
			/*
			foreach (IAppender appender in this.Appenders)
				appender.Write(message);
			 */
		}

		private bool ShouldWrite(object level)
		{
			if (!On)
				return false;

			if (level is Int32 && Level is Int32)
			{
				if ((int)level < (int)Level)
					return true;
			}

			EvaluateLevelHandler eh = this.EvaluateLevel;
			if (eh != null)
				return eh(level);

			return true;
		}

		#region ICloneable Members

		public object Clone()
		{
			throw new NotImplementedException();
		}

		#endregion
	}

	
}
