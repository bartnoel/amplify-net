using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)] 
	public class HasAndBelongsToMany : AssociationAttribute 
	{
		public string JoinTable { get; set; }
		public string AssocationForeignKey { get; set; }
		public string FinderSql { get; set; }
		public string InsertSql { get; set; }
		public string DeleteSql { get; set; }
		public string Select { get; set; }
		public string Group { get; set; }
		public int? Limt { get; set; }
		public int? Offset { get; set;}
		public bool Unique { get; set; }

		public HasAndBelongsToMany()
		{
			this.Select = " * ";
		}
	}
}
