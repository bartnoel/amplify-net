using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data
{
	public class Join : QueryPart 
	{
		private string alias = "";

		public string Target { get; set; }

		public string On { get; set; }

		public string Alias 
		{ 
			get {
				if (string.IsNullOrEmpty(this.alias))
					return this.Target;
				return this.alias;
			}
			set { this.alias = value; } 
		}

		public override string ToString(Adapter adapter)
		{
			return string.Format(" JOIN {0} As {1} \n\t\tON {2}",
				adapter.QuoteTableName(this.Target),
				  this.Alias, this.On);
		}
	}
}
