using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify.Diagnostics
{
	public class ConsoleAppender : ILogApender 
	{
		#region ILogApender Members

		public ConsoleAppender()
		{
			this.Format = "[%l] @ (%d %t): {0}";
		}

		public string Format { get; set; }


		public void WriteLine(object message, object level, params object[] values)
		{
			Console.WriteLine(string.Format(
					LogFormatter.Format(Format, level), string.Format(message.ToString(), values)));
		}


		public void WriteLine(object message, object level)
		{
			Console.WriteLine(string.Format(LogFormatter.Format(Format, level), message));
		}

		public void WriteLine(object message)
		{
			Console.WriteLine(string.Format(LogFormatter.Format(Format, 1), message));
		}

		public void Write(object message)
		{
			Console.Write(string.Format(LogFormatter.Format(Format, 1), message));
		}

		public void Write(object message, object level, params object[] values)
		{
			Console.Write(string.Format(
				LogFormatter.Format(Format, level), string.Format(message.ToString(), values)));
		}

		public void Write(object message, object level)
		{
			Console.Write(string.Format(LogFormatter.Format(Format, level), message));
		}

		public void Write(Exception ex)
		{
			Console.WriteLine(string.Format(LogFormatter.Format(Format, 3), ex));
		}

		#endregion
	}
}
