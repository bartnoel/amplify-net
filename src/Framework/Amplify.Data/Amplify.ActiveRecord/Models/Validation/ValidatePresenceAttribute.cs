﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Models.Validation
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)] 
	public class ValidatePresenceAttribute : ValidationAttribute 
	{
		private ValidatePresenceRule rule;

		public object DefaultValue 
		{
			get { return this.rule.DefaultValue; }
			set { this.rule.DefaultValue = value; }
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

		public override object TypeId
		{
			get { return this.rule.RuleName; }
		}

		public override Amplify.ComponentModel.IValidationRule Rule
		{
			get { return this.rule; }
		}

		public ValidatePresenceAttribute()
		{
			this.rule = new ValidatePresenceRule();	
		}

	}
}
