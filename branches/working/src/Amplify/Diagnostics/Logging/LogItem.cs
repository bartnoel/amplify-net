using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify.Diagnostics.Logging
{
	public class LogItem
	{
		private Dictionary<string, object> properties;

		public object Message { get; set; }

		public object Level { get; set; }

		public DateTime OccuredAt { get; set; }

		public string ThreadName { get; set; }

		public StackTraceInfo StackTraceInfo { get; set; }

		public Exception Exception { get; set; }

		public Dictionary<string, object> Properties
		{
			get {
				if (this.properties == null)
					this.properties = new Dictionary<string, object>();
				return this.properties;
			}
		}
	}
}
