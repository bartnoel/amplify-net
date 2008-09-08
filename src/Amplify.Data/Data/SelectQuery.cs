

namespace Amplify.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using Amplify.Linq;

	public class SelectQuery
	{
		private Hash properties;
		

		public SelectQuery(Adapter adapter) 
		{
			this.Adapter = adapter;
		}


		protected Adapter Adapter { get; set; }
		

		public object this[string propertyName]
		{
			get { return this.properties[propertyName]; }
			set { this.properties[propertyName] = value; }
		}

		public string Selection
		{
			get { return this["selection"] as string; }
			set { this["selection"] = value;  }
		}

		public string Target
		{
			get { return this["target"] as string; }
			set { this["target"] = value; }
		}

		public string Clause
		{
			get { return this["clause"] as string; }
			set { this["clause"] = value; }
		}

		public List<object> Values
		{
			get {
				List<object> list = this["values"] as List<object>;
				if (list == null) 
				{
					list = new List<object>();
					this["values"] = list;
				}
				return list;
			}
		}

		public Select From(string target)
		{
			this.Target = target;
			return this;
		}

		public Select Where(string clause, params object[] values)
		{
			this.Clause += clause;
			return this;
		}

		public Select Column(string column)
		{
			this.Clause += string.Format(" {0} ", column);
			return this;
		}



		public Select Equals(object value)
		{
			this.Clause += string.Format(" = ", 
		}

	}
}
