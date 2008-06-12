using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data
{
	public class HasManyAttribute : AssociationAttribute 
	{
		public HasManyAttribute()
			: base()
		{
			this.Dependent = DependentAction.Nullify;
			this.FinderSql = "";
			this.Includes = new string[] { };
			this.Group = "";
			this.Select = "*";
		}

		public DependentAction Dependent { get; set; }

		public string FinderSql { get; set; }

		public string[] Includes { get; set; }

		public string Group { get; set; }

		public int? Limit { get; set; }

		public int? Offset { get; set; }

		public string Select { get; set; }
	}
}
