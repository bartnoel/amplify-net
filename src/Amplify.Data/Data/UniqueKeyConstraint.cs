using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data
{
	public class UniqueKeyConstraint : KeyConstraint
	{
		public override string Prefix
		{
			get { return "UN"; }
		}
	}
}
