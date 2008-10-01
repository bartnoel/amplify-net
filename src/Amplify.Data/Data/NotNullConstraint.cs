using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data
{
	public class NotNullConstraint : ConstraintDefinition
	{

		public override string Prefix
		{
			get { return "NN"; }
		}

		public override string ToString()
		{
			return string.Format("CONSTRAINT {0} NOT NULL", this.Name);
		}
	}
}
