using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify.Diagnostics.Logging
{
	public interface ILogBuilder : IDisposable 
	{
		void Append(LogItem logItem);

		string Name { get; set; }
	}
}
