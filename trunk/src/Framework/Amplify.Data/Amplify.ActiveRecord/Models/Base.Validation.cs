using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Amplify.ComponentModel;
using Amplify.Data;
using Amplify.Linq;
using Amplify.Models.Validation;

namespace Amplify.Models
{
	public partial class Base<T> : IDataErrorInfo, IDataValidationInfo 
	{
		private bool isInitializingStaticRules = false;
		private ValidationRules rules;
		private List<IValidationRule> brokenRules;

		

		public IEnumerable<IValidationRule> BrokenValidationRules
		{
			get { return this.brokenRules; }
		}



		protected void StaticInitialize()
		{
			if (!initializeOnce)
			{
				isInitializingStaticRules = true;
				initializeOnce = true;
				this.rules = new ValidationRules();
				this.GetAttributesOfType();
				this.AddStaticRules();
				ValidationRegistry.Add(this.GetType(), this.rules);
				isInitializingStaticRules = false;
			}
		}

		internal protected virtual void Initialize()
		{
			this.rules = new ValidationRules( 
				ValidationRegistry.Get(this.GetType()));
			this.InitializeValues();
			this.AddRules();
			this.Validate();
		}

		protected virtual void InitializeValues()
		{

		}


		protected virtual void Validate()
		{
		
		}



		protected void AddValidationRule(ValidationRule rule)
		{
			this.rules.Add(rule);				
		}

		protected void AddValidationRule(ValidationRule rule, params string[] names)
		{
			if (rule is IClone)
				names.Each(delegate(string name)
				{
					ValidationRule validationRule = (ValidationRule)((IClone)rule).Clone();
					validationRule.Name = name;
					this.AddValidationRule(rule);
				});
			else
				throw new InvalidOperationException("Validation rule must support the IClone interface");
		}




		protected virtual void Validate(string propertyName, object value)
		{
			this.rules.Where(o => o.Name.ToLower() == propertyName).Each(
				delegate(IValidationRule rule)
				{
					bool pass = true;
					if (rule is IValidateObject)
						pass = rule.Validate(this);
					else
						pass = rule.Validate(value);

					if (!pass)
						this.brokenRules.Add(rule);
					else if (this.brokenRules.Contains(rule))
						this.brokenRules.Remove(rule);
				});
		}

		protected virtual void AddStaticRules()
		{
	
		}

		protected virtual void AddRules()
		{

		}

		#region IDataErrorInfo Members

		string IDataErrorInfo.Error
		{
			get {
				string error = "";
				this.brokenRules.Each(r => error += r.Message);
				return error;
			}
		}

		string IDataErrorInfo.this[string columnName]
		{
			get {
				IValidationRule rule = this.brokenRules.SingleOrDefault(r => r.Name.ToLower() == columnName.ToLower());
				if(rule != null)
					return  rule.Message;
				return "";
			}
		}

		#endregion

		#region IDataValidationInfo Members

		IEnumerable<IValidationRule> IDataValidationInfo.this[string propertyName]
		{
			get
			{
				return this.rules.Where(o => o.Name.ToLower() == propertyName.ToLower());
			}
		}

		#endregion
	}
}
