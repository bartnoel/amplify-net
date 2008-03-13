

namespace Amplify.Data
{
	using System;
	using System.Collections.Generic;
	using System.Collections;
	using System.Linq;
	using System.Text;
	using System.Reflection;

	using Amplify.Linq;

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)] 
	public class HasMany : HasOne 
	{
		public string FinderSql { get; set; }
		public string CounterSql { get; set; }
		public string Group { get; set; }
		public int? Limit { get; set; }
		public int? Offset { get; set; }
		public string Select { get; set; }
		public string Through { get; set; }
		public string Source { get; set; }
		public string SourceType { get; set; }
		public bool Uniq { get; set; }


		public HasMany(string associationId): this()
		{
			this.AssociationId = associationId;
		}

		public HasMany()
		{
			this.Select = " * ";
		}

	}
}
