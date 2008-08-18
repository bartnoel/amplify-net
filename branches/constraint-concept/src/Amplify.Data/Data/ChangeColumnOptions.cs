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
			get { return (int?)this.properties["limit"]; }
			set { this.properties["limit"] = value; }
		}

		public int? Scale
		{
			get { return (int?)this.properties["scale"]; }
			set { this.properties["scale"] = value; }
		}


		public int? Precision
		{
			get { return (int?)this.properties["precision"]; }
			set { this.properties["precision"] = value; }
		}

		public object Default
		{
			get { return this.properties["default"]; }
			set { this.properties["default"] = value; }
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
