using System;
using System.Collections.Generic;

using System.Text;

using Amplify.ComponentModel;

namespace Amplify.Data.Validation
{
	public class ValidatePresence : ValidationRule
	{

		public ValidatePresence()
		{
			this.Message = "required";
			this.DefaultValue = "";
		}

		public object DefaultValue
		{
			get { return this["DefaultValue"]; }
			set { this["DefaultValue"] = value; }
		}

		public override bool ValidateData(object value)
		{
			if (this.If == null || this.If(value))
			{
				if (value != null)
				{
					if (value is DateTime)
						return !value.Equals(DateTime.MinValue);
					if (value is int)
						return !value.Equals(-1);
				}
				if (value is string || this.DefaultValue is string)
					return !string.IsNullOrEmpty(value as string);
				return (!Object.Equals(value, this.DefaultValue));
			}
			return true;
		}

		
	}
}
