using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Amplify.Models.Validation
{
	public class ValidateFormatRule : ValidationRule 
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

		public ValidateFormatRule()
		{
			this.Message = "invalid format";
			this.Options = RegexOptions.None;
		}

		public override bool Validate(object value)
		{
			return Regex.IsMatch(value.ToString(), this.With, this.Options);
		}
	}
}
