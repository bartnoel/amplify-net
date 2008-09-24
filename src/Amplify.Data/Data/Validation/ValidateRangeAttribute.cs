using System;
using System.Collections.Generic;

using System.Text;

namespace Amplify.Data.Validation
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
	public class ValidateRangeAttribute : ValidationAttribute 
	{
		private ValidateRange rule;

		public ValidateRangeAttribute() 
		{
			this.rule = new ValidateRange();			
		}

		public string TooShort
		{
			get { return this.rule.TooShort; }
			set { this.rule.TooShort = value; }
		}

		public string TooLong
		{
			get { return this.rule.TooLong; }
			set { this.rule.TooLong = value; }
		}

		public string WrongLength
		{
			get { return this.rule.WrongLength; }
			set { this.rule.WrongLength = value; }
		}

		public IComparable Maximum
		{
			get { return this.rule.Maximum; }
			set { this.rule.Maximum = value; }
		}

		public IComparable Minimum
		{
			get { return this.rule.Minimum; }
			set { this.rule.Minimum = value; }
		}

		public IComparable Is
		{
			get { return this.rule.Is; }
			set { this.rule.Is = value; }
		}

		public Range In
		{
			get { return this.rule.In; }
			set { this.rule.In = value; }
		}

		public override Predicate<object> If
		{
			get { return this.rule.If; }
			set { this.rule.If = value; }
		}

		public override IValidationRule Rule
		{
			get { return this.rule; } 
		}

		public override object TypeId
		{
			get
			{
				return this.rule.RuleName;
			}
		}
	}
}
