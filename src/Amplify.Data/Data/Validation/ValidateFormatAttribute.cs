using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Amplify.Data.Validation
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
	public class ValidateFormatAttribute : ValidationAttribute 
	{
		private ValidateFormat rule;

		
		public ValidateFormatAttribute()
		{
			this.rule = new ValidateFormat();
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

		public override Predicate<object> If
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
