

namespace Amplify.ActiveRecord.Data
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;
	using System.Text;

	public class Options : IOptions 
	{
		public Clause Conditions { get; set; }
		public Clause OrderBy { get; set; }
		public GroupBy GroupBy { get; set; }
		public bool IsDistinct { get; set; }
		public int Limit { get; set; }
		public int Offset { get; set; }


		public Options()
		{

		}

		public Options SelectDistinct() 
		{
			this.IsDistinct = true;
			return this;
		}

		public Options Where(string expression, params object[] values)
		{
			this.Conditions = new Clause(expression, values);
			return this;
		}

		public Options SortBy(string expression, params object[] values)
		{
			this.OrderBy = new Clause(expression, values);
			return this;
		}

		public Options Group(string keySelector, string elementSelector, params object[] values)
		{
			this.GroupBy = new GroupBy(keySelector, elementSelector, values);
			return this;
		}

		public Options Take(int limit)
		{
			this.Limit = limit;
			return this;
		}

		public Options Page(int limit, int offset)
		{
			this.Limit = limit;
			this.Offset = offset;
			return this;
		}

		

	}
}
