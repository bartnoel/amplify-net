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
	using System.Reflection;

	using Amplify.ComponentModel;
	

	public class ValidationRules : List<IValidationRule>, IService, IWebFormValidation
	{
		private List<IValidationRule> brokenRules;


		public List<IValidationRule> BrokenRules
		{
			get {
				if (this.brokenRules == null)
					this.brokenRules = new List<IValidationRule>();
				return this.brokenRules;
			}
		}

		public string GetError(string propertyName)
		{
			List<IValidationRule> list = GetBrokenRulesForProperty(propertyName);
			string message = "";
			if (list.Count > 0)
				message = list[0].Message;
			return message;
		}

		public string GetErrors()
		{
			List<IValidationRule> list = this.BrokenRules;
			string message = "";
			list.ForEach(delegate(IValidationRule rule)
			{
				message += rule.Message + "\n";
			});
			return message;
		}

		public string GetErrors(string propertyName)
		{
			List<IValidationRule> list = GetBrokenRulesForProperty(propertyName);
			string message = "";
			list.ForEach(delegate(IValidationRule rule) {
				message += rule.Message + "\n";
			});
			return message;
		}

		public List<IValidationRule> GetBrokenRulesForProperty(string propertyName)
		{
			List<IValidationRule> list = new List<IValidationRule>();
			foreach (IValidationRule item in this.BrokenRules)
				if (item.PropertyName.ToLower() == propertyName.ToLower())
					list.Add(item);
			return list;
		}

		public bool IsValid
		{
			get { return this.BrokenRules.Count == 0; }
		}

		public ValidationRules()
			: base()
		{

		}

		public ValidationRules(IEnumerable<IValidationRule> items)
			: base(items)
		{

		}

		public IEnumerable<IValidationRule> this[string propertyName]
		{
			get {
				List<IValidationRule> list = new List<IValidationRule>();
				foreach (IValidationRule item in this)
				{
					if (item.PropertyName.ToLower() == propertyName.ToLower())
						list.Add(item);
				}
				return list;
			}
		}


		public virtual void Validate (object entity, string propertyName) 
		{
			IEnumerable<IValidationRule> rules = this[propertyName];
			object value = null;
			if (entity is IDecoratedObject)
				value = ((IDecoratedObject)value)[propertyName];
			else
				value = entity.GetType().GetProperty(propertyName).GetValue(entity, null);

			this.Validate(entity, value, rules);
		}


		protected void Validate(object entity, object value, IEnumerable<IValidationRule> rules)
		{
			foreach (IValidationRule rule in rules)
				if (!rule.ValidateData(entity, value))
					this.BrokenRules.Add(rule);
				else
					this.BrokenRules.Remove(rule);
		}

		#region IWebFormValidation Members

		public List<System.Web.UI.IValidator> GetValidators(string propertyName)
		{
			List<IValidationRule> list = this.FindAll(delegate(IValidationRule rule)
			{
				if(rule.PropertyName.ToLower() == propertyName.ToLower())
					return true;
				return false;
			});

			List<System.Web.UI.IValidator> validators = new List<System.Web.UI.IValidator>();
			foreach (IValidationRule rule in list)
			{
				if (rule is ValidationRule)
				{
					System.Web.UI.IValidator validator = ((ValidationRule)rule).GetValidator();
					if (validator != null)
						validators.Add(validator);
				}
			}
			return validators;
		}

		#endregion
	}
}
