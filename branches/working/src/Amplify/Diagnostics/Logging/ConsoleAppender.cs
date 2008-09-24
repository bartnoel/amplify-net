using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify.Diagnostics.Logging
{
	public class ConsoleAppender : IAppender 
	{
		#region ILogApender Members

		public ConsoleAppender()
		{
			this.Format = "[%l] @ (%d %t): {0}";
		}

		/// <summary>
		/// The format of the string for the log
		/// </summary>
		public string Format { get; set; }

		/// <summary>
		/// Writes a line to the log. 
		/// </summary>
		/// <param name="message">The message that will be written.</param>
		/// <param name="level">The level of the message.</param>
		/// <param name="values">The values that will be formatted into the string.</param>
		public void WriteLine(object message, object level, params object[] values)
		{
			
			//Console.WriteLine(string.Format(
			//        LogFormatter.Format(Format, level), string.Format(message.ToString(), values)));
		}

		/// <summary>
		/// Writes a line to the log. 
		/// </summary>
		/// <param name="message">The message that will be written.</param>
		/// <param name="level">The level of the message.</param>
		public void WriteLine(object message, object level)
		{
			//Console.WriteLine(string.Format(LogFormatter.Format(Format, level), message));
		}

		/// <summary>
		/// Writes a line to the log. 
		/// </summary>
		/// <param name="message">The message that will be written.</param>
		public void WriteLine(object message)
		{
			//Console.WriteLine(string.Format(LogFormatter.Format(Format, 1), message));
		}

		/// <summary>
		/// Writes a line to the log. 
		/// </summary>
		/// <param name="message">The message that will be written.</param>
		public void Write(object message)
		{
			//Console.Write(string.Format(LogFormatter.Format(Format, 1), message));
		}

		/// <summary>
		/// Writes the message to the log.
		/// </summary>
		/// <param name="message">The object message.</param>
		/// <param name="level">The level of the message.</param>
		/// <param name="values">The values that will be formatted into the message.</param>
		public void Write(object message, object level, params object[] values)
		{
			//Console.Write(string.Format(
			//    LogFormatter.Format(Format, level), string.Format(message.ToString(), values)));
		}

		/// <summary>
		/// Writes the message to the log.
		/// </summary>
		/// <param name="message">The object message.</param>
		/// <param name="level">The level of the message.</param>
		public void Write(object message, object level)
		{
			//Console.Write(string.Format(LogFormatter.Format(Format, level), message));
		}

		/// <summary>
		/// Writes the exception to the log.
		/// </summary>
		/// <param name="message">The <see cref="System.Exception"/>.</param>
		public void Write(Exception ex)
		{
			//Console.WriteLine(string.Format(LogFormatter.Format(Format, 3), ex));
		}

		#endregion

		#region IAppender Members

		public void Append(LogItem logItem)
		{
			throw new NotImplementedException();
		}

		public string Name
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
