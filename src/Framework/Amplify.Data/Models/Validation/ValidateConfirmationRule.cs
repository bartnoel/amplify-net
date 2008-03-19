using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Amplify.ComponentModel;
using Amplify.Linq;

namespace Amplify.Models.Validation
{
	public class ValidateConfirmationRule : ValidationRule, IValidateObject, IClone  
	{
		public ValidateConfirmationRule()
		{
			this.Message = "does not match confirmation";
		}

		public override bool Validate(object value)
		{
			Type type = value.GetType();
			PropertyInfo target = type.GetProperty(this.Name);
			PropertyInfo confirmation = type.GetProperty(this.Name + "Confirmation");

			if (target == null)
				throw new  InvalidOperationException("Property {0} does not exist".Inject(this.Name));
			if (confirmation == null)
				throw new InvalidOperationException("Property {0}Confirmation does not exist".Inject(this.Name));

			return target.GetValue(value, null).Equals(confirmation.GetValue(value, null));
		}

		public IValidationRule Clone()
		{
			ValidateConfirmationRule rule = new ValidateConfirmationRule();
			this.EachKey(key => rule[key] = this[key]);
			return rule;
		}
	}
}
