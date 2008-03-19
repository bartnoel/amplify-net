using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Amplify.ComponentModel;

namespace Amplify.Models.Validation
{
	public class ValidationRules : List<IValidationRule>, IService 
	{
		public ValidationRules()
			: base()
		{

		}

		public ValidationRules(IEnumerable<IValidationRule> items)
			: base(items)
		{

		}
	}
}
