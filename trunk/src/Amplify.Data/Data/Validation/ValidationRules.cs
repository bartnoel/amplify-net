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
	

	public class ValidationRules : List<IValidationRule>, IService 
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

		public string GetErrors(string propertyName)
		{
			List<IValidationRule> list = GetBrokenRulesForProperty(propertyName);
			string message = "";
			list.ForEach(delegate(IValidationRule rule) {
				message += rule.Message;
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

		public virtual void Validate(object entity, string propertyName)
		{
			IEnumerable<IValidationRule> rules = this[propertyName];
			if (((List<IValidationRule>)rules).Count == 0)
				return;

			PropertyInfo property =	entity.GetType().GetProperty(propertyName);
			if (propertyName != null)
			{
				this.Validate(propertyName, property.GetValue(entity, null), rules);
			}
		}

		public virtual void Validate(IDecoratedObject entity, string propertyName)
		{
			IEnumerable<IValidationRule> rules = this[propertyName];
			if (((List<IValidationRule>)rules).Count == 0)
				return;

			this.Validate(propertyName, entity[propertyName], rules);
		}

		public virtual void Validate (string propertyName, object value) 
		{
			IEnumerable<IValidationRule> rules = this[propertyName];
			this.Validate(propertyName, value, rules);
		}


		protected void Validate(string propertyName, object value, IEnumerable<IValidationRule> rules)
		{
			foreach (IValidationRule rule in rules)
				if (!rule.ValidateData(value))
					this.BrokenRules.Add(rule);
				else
					this.BrokenRules.Remove(rule);
		}
	}
}
