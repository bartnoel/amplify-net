using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Models.Validation
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter )]
	public class ValidateConfirmationAttribute : ValidationAttribute 
	{
		private ValidateConfirmationRule rule;

		public ValidateConfirmationAttribute() 
		{
			this.rule = new ValidateConfirmationRule();
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

		public override object TypeId
		{
			get { return this.rule.RuleName; }
		}
	}
}
