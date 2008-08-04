using System;
using System.Collections.Generic;

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
			this.Message = "{0} must match {0} Confirmation";
		}

		public override string Message
		{
			get
			{
				return string.Format(base.Message,
						Inflector.Titleize(this.PropertyName));
			}
			set
			{
				base.Message = value;
			}
		}
		

		public override bool ValidateData(object entity, object value)
		{
			if (value is IDecoratedObject)
			{
				IDecoratedObject hash = (IDecoratedObject)entity;
				return hash[this.PropertyName].Equals(hash[this.PropertyName + "Confirmation"]);
			}
			else
			{
				Type type = entity.GetType();
				PropertyInfo confirmation = type.GetProperty(this.PropertyName + "Conffirmation");
				
				if (confirmation == null)
					throw new InvalidOperationException(string.Format("Property {0}Confirmation does not exist", this.PropertyName));

				return value.Equals(confirmation.GetValue(value, null));
			}
		}

		public IValidationRule Clone()
		{
			ValidateConfirmation rule = new ValidateConfirmation();
			rule.Map(this);
			return rule;
		}
	}
}
