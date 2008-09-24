using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify.Diagnostics
{
	public class Logger
	{
		private static readonly object key = new object();

		static Logger()
		{
			lock (key)
			{

			}
		}

	}
}
