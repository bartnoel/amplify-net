using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.ActiveRecord.Data
{
	public class Clause
	{
		public string Expression { get; set; }
		public object[] Values { get; set; }

		public Clause()
		{

		}

		public Clause(string expression, params object[] values)
		{
			this.Expression = expression;
			this.Values = values;
		}
	}
}
