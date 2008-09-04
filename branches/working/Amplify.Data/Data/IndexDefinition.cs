using System;
using System.Collections.Generic;

using System.Text;

namespace Amplify.Data
{
	using Amplify.Linq;

	public class IndexDefinition
	{
		private List<string> columns;

		public List<string> Columns {
			get {
				if (this.columns == null)
					this.columns = new List<string>();
				return this.columns;
			}
			internal set
			{
				this.columns = value;
			}
		}

		public bool IsUnique { get; set; }

		public string Name { get; set; }

		public string TableName { get; set; }

	}
}
