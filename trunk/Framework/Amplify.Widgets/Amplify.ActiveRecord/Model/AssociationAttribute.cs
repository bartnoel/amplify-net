//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace Amplify.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using Amplify.Linq;

	[AttributeUsage(AttributeTargets.Property, AllowMultiple=false)] 
	public class AssociationAttribute : System.Attribute
	{
		public AssocationType Type { get; set; }
		public Type Class { get; set; }
		public string Through { get; set; }
		public IEnumerable<object> Conditions { get; set; }
		public string Order { get; set; }
		public string ForeignKey { get; set; }
		public Hash Include { get; set; }
		public bool Polymorphic { get; set; }

		private AssociationAttribute()
		{

		}

		public AssociationAttribute(AssocationType type)
		{
			this.Type = type;
		}
	}
}
