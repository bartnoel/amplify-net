using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.ActiveRecord.Data
{
	public class GroupBy : Clause 
	{
		public string ElementSelector { get; set; }

		public GroupBy(string keySelector, string ElementSelector, params object[] values)
		{
			this.Expression = keySelector;
			this.ElementSelector = ElementSelector;
			this.Values = values;
		}

	}
}
