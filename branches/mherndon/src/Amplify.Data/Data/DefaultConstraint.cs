using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data
{
	public class DefaultConstraint : ConstraintDefinition 
	{
		public override string Prefix
		{
			get { return "DF"; }
		}

		public string Value { get; set; }
	}
}
