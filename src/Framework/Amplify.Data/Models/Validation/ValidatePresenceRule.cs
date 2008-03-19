using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Amplify.ComponentModel;

namespace Amplify.Models.Validation
{
	public class ValidatePresenceRule : ValidationRule, IClone
	{

		public ValidatePresenceRule()
		{
			this.Message = "required";
			this.DefaultValue = "";
		}

		public object DefaultValue
		{
			get { return this["DefaultValue"]; }
			set { this["DefaultValue"] = value; }
		}

		public override bool Validate(object value)
		{
			return (this.DefaultValue == value);
		}

		public IValidationRule Clone()
		{
			ValidatePresenceRule rule = new ValidatePresenceRule();
			this.EachKey(key => rule[key] = this[key]);
			return rule;
		}
	}
}
