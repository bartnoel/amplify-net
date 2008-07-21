using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify.Diagnostics
{
	public interface ILogAppender
	{
		/// <summary>
		/// The format of the string for the log
		/// </summary>
		string Format { get; set; }

		/// <summary>
		/// Writes a line to the log. 
		/// </summary>
		/// <param name="message">The message that will be written.</param>
		/// <param name="level">The level of the message.</param>
		void WriteLine(object message, object level);

		/// <summary>
		/// Writes a line to the log. 
		/// </summary>
		/// <param name="message">The message that will be written.</param>
		/// <param name="level">The level of the message.</param>
		/// <param name="values">The values that will be formatted into the string.</param>
		void WriteLine(object message, object level, params object[] values);


		/// <summary>
		/// Writes a line to the log. 
		/// </summary>
		/// <param name="message">The message that will be written.</param>
		void WriteLine(object message);

		/// <summary>
		/// Writes the message to the log.
		/// </summary>
		/// <param name="message">The object message.</param>
		void Write(object message);

		/// <summary>
		/// Writes the message to the log.
		/// </summary>
		/// <param name="message">The object message.</param>
		/// <param name="level">The level of the message.</param>
		void Write(object message, object level);

		/// <summary>
		/// Writes the message to the log.
		/// </summary>
		/// <param name="message">The object message.</param>
		/// <param name="level">The level of the message.</param>
		/// <param name="values">The values that will be formatted into the message.</param>
		void Write(object message, object level, params object[] values);

		/// <summary>
		/// Writes the exception to the log.
		/// </summary>
		/// <param name="message">The <see cref="System.Exception"/>.</param>
		void Write(Exception ex);
	}
}
