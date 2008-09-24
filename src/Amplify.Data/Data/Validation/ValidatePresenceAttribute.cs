using System;
using System.Collections.Generic;

using System.Text;

namespace Amplify.Data.Validation
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)] 
	public class ValidatePresenceAttribute : ValidationAttribute 
	{
		private ValidatePresence rule;

		public object DefaultValue 
		{
			get { return this.rule.DefaultValue; }
			set { this.rule.DefaultValue = value; }
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

		public override object TypeId
		{
			get { return this.rule.RuleName; }
		}

		public override IValidationRule Rule
		{
			get { return this.rule; }
		}

		public ValidatePresenceAttribute()
		{
			this.rule = new ValidatePresence();	
		}

	}
}
