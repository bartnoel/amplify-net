//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Data.Validation
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Web.UI.WebControls;

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Constructor | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true)]
	public class ValidateComparisonAttribute : ValidationAttribute 
	{
		private ValidateComparison rule;

		public ValidateComparisonAttribute()
		{
			this.rule = new ValidateComparison();
		}

		public string PropertyToCompare
		{
			get { return this.rule.PropertyToCompare; }
			set { this.rule.PropertyToCompare = value; }
		}

		public IComparable ValueToCompare
		{
			get { return this.rule.ValueToCompare; }
			set { this.rule.ValueToCompare = value; }
		}

		public ValidationCompareOperator Operator
		{
			get { return this.rule.Operator; }
			set { this.rule.Operator = value; }
		}


		public override IValidationRule Rule
		{
			get
			{
				return this.rule;
			}
		}
	}
}
