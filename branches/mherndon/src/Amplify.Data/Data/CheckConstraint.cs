using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data
{
	public class CheckConstraint : ConstraintDefinition 
	{
		public override string Prefix
		{
			get { return "CK"; }
		}

		public string Value { get; set; }
	}
}
