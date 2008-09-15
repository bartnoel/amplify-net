using System;
using System.Collections.Generic;

using System.Text;

namespace Amplify.Data
{
	using Amplify.Linq;

	public class IndexDefinition : ConstraintDefinition 
	{
		

		public bool IsUnique { get; set; }


		public override string Prefix
		{
			get { 
				return "IX"; 
			}
		}
	

	}
}
