using System;
using System.Collections.Generic;
using System.Linq;
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
			if(this.If == null || this.If(value))
				return (!this.DefaultValue.Equals(value));
			return true;
		}

		
	}
}
