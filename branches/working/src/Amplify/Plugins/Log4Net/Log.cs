using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify.Plugins.Log4Net
{
	public class Log : log4net.Core.LogImpl, Amplify.ILog 
	{
		public Log(log4net.Core.ILogger logger)
			:base(logger)
		{
			
		}


	}
}
