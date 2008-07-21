using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify.Data
{
	using Amplify.Linq;

	public class ColumnOptions
	{
		private Hash properties = new Hash();

		public int? Limit
		{
			get { return (int?)this.properties["Limit"]; }
			set { this.properties["Limit"] = value; }
		}

		public int? Scale
		{
			get { return (int?)this.properties["Scale"]; }
			set { this.properties["Scale"] = value; }
		}


		public int? Precision
		{
			get { return (int?)this.properties["Precision"]; }
			set { this.properties["Precision"] = value; }
		}

		public object Default
		{
			get { return this.properties["Default"]; }
			set { this.properties["Default"] = value; }
		}

		public ColumnDefinition Column
		{
			get { return this.properties["Column"] as ColumnDefinition; }
			set { this.properties["Column"] = value; }
		}

		internal Hash ToHash()
		{
			return this.properties; 
		}

	}
}
