using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.ObjectModel
{
	public class GuidFactory : ValueFactory 
	{
		public override object CreateValue()
		{
			return Guid.NewGuid();
		}
	}
}
