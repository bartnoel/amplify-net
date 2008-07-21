using System;
using System.Collections.Generic;

using System.Text;
using System.Text.RegularExpressions;

namespace Amplify.Data.Validation
{
	public class ValidateFormat : ValidationRule 
	{
		public string With 
		{
			get { return (this["With"] as string); }
			set { this["With"] = value; }
		}

		public RegexOptions Options 
		{
			get { return (RegexOptions)this["Options"]; }
			set { this["Options"] = value; }
		}

		public ValidateFormat()
		{
			this.Message = "invalid format";
			this.Options = RegexOptions.ECMAScript;
		}

		public override bool ValidateData(object value)
		{
			if (value != null && (this.If == null || this.If(value)))
				return StringUtil.IsMatch(value as string, this.With, this.Options);
			return true;
		}
	}
}
