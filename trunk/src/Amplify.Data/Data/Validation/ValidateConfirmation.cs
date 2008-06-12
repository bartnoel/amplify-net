using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Amplify.ComponentModel;
using Amplify.Linq;

namespace Amplify.Data.Validation
{
	public class ValidateConfirmation : ValidationRule, IValidateObject
	{
		public ValidateConfirmation()
		{
			this.Message = "does not match confirmation";
		}

		public override bool ValidateData(object value)
		{
			Type type = value.GetType();
			PropertyInfo target = type.GetProperty(this.PropertyName);
			PropertyInfo confirmation = type.GetProperty(this.PropertyName + "Confirmation");

			if (target == null)
				throw new  InvalidOperationException("Property {0} does not exist".Fuse(this.PropertyName));
			if (confirmation == null)
				throw new InvalidOperationException("Property {0}Confirmation does not exist".Fuse(this.PropertyName));

			return target.GetValue(value, null).Equals(confirmation.GetValue(value, null));
		}

		public IValidationRule Clone()
		{
			ValidateConfirmation rule = new ValidateConfirmation();
			rule.Map(this);
			return rule;
		}
	}
}
