using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify.Diagnostics
{
	public interface ILogApender
	{
		string Format { get; set; }
		void WriteLine(object message, object level);
		void WriteLine(object message, object level, params object[] values);
		void WriteLine(object message);
		void Write(object message);
		void Write(object message, object level);
		void Write(object message, object level, params object[] values);
		void Write(Exception ex);
	}
}
