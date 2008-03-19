using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Amplify.Models.Validation
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
	public class ValidateFormatAttribute : ValidationAttribute 
	{
		private ValidateFormatRule rule;

		
		public ValidateFormatAttribute()
		{
			this.rule = new ValidateFormatRule();
		}

		public string With
		{
			get { return this.rule.With; }
			set { this.rule.With = value; }
		}

		public RegexOptions Options
		{
			get { return this.rule.Options; }
			set { this.rule.Options = value; }
		}

		public override string If
		{
			get { return this.rule.If; }
			set { this.rule.If = value; }
		}

		public override string Message
		{
			get { return this.rule.Message; }
			set { this.rule.Message = value; }
		}

		public override Amplify.ComponentModel.IValidationRule Rule
		{
			get { return this.rule; }
		}

	}
}
