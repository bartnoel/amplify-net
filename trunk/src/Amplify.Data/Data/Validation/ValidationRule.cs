//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Company Name.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Data.Validation
{
	using System;
	using System.Collections.Generic;
	
	using System.Text;

	using Amplify.ComponentModel;

	public class ValidationRule : DecoratedObject, IValidationRule
	{
		public string PropertyName 
		{
			get { return (this["PropertyName"] as string); }
			internal set { this["PropertyName"] = value; }
		}

		public virtual string Message 
		{
			get { return (this["Message"] as string); }
			set { this["Message"] = value; }
		}

		public object Level 
		{
			get { return this["Level"]; }
			set { this["Level"] = value; }
		}

		public string RuleName
		{
			get { return this.GetType().Name; }
		}

		public Predicate<object> If 
		{
			get { return (this["If"] as Predicate<object>); }
			set { this["If"] = value; }
		}

		public ValidationRule()
		{
			this.Level = 0;
		}

		public virtual bool ValidateData(object value)
		{
			return true;
		}

		public ValidationRule Clone()
		{
			ValidationRule rule = (ValidationRule)Activator.CreateInstance(this.GetType());
			rule.Map(this);
			return rule;
		}

	}
}
