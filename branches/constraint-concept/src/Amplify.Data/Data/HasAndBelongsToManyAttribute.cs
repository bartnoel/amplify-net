using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data
{
	public class HasAndBelongsToManyAttribute : AssociationAttribute 
	{
		public HasAndBelongsToManyAttribute() :base() {

			this.Include = new string[] { };
			this.Select = "*";
		}

		public string FinderSql { get; set; }

		public string DeleteSql { get; set; }

		public string InsertSql { get; set; }

		public string[] Include { get; set; }

		public string Group { get; set; }

		public int? Limit { get; set; }

		public int? Offset { get; set; }

		public string Select { get; set; }
	}
}
