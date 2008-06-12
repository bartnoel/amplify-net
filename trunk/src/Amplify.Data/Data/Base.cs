

namespace Amplify.Data
{

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Reflection;

	using Validation;
	using Amplify.ComponentModel;

	public class Base : IDataValidationInfo 
	{
		private static bool s_initalizeOnce = false;
		private ValidationRules rules;

		public Base()
		{
			this.Initialize();
		}

		#region Properties 


		public virtual bool IsValid
		{
			get { return this.Rules.IsValid; }
		}

		public List<IValidationRule> BrokenRules
		{
			get { return this.Rules.BrokenRules; }
		}

		private ValidationRules Rules
		{
			get
			{
				if (this.rules == null)
					this.rules = new ValidationRules();
				return this.rules;
			}
			set
			{
				this.rules = value;
			}
		}		

		#endregion 

		#region Methods

		#region Initialization
		protected void Initialize()
		{
			this.InitializeOnce();
			this.AddRules();
			IRelationalMetaData meta = Base.GetMetaData(this.GetType());
			this.Rules.AddRange(meta.ValidationRules);
			this.OnCreated();
		}

		protected void InitializeOnce()
		{
			if (s_initalizeOnce == false)
			{
				IRelationalMetaData meta = Base.GetMetaData(this.GetType());
				this.AddStaticRules();
				meta.ValidationRules.AddRange(this.Rules);
				this.Rules = null;
			}
		}

		protected virtual void OnCreated()
		{

		}
		#endregion

		#region Validation Methods
		protected void AddRule(IValidationRule rule)
		{
			this.Rules.Add(rule); 
		}

		protected void AddRule(IEnumerable<string> propertyNames, ValidationRule rule)
		{
			if(!string.IsNullOrEmpty(rule.PropertyName))
				this.Rules.Add(rule);

			foreach (string propertyName in propertyNames)
			{
				ValidationRule newRule = rule.Clone();
				newRule.PropertyName = propertyName;
				this.Rules.Add(newRule);
			}
		}

		protected void AddStaticRules()
		{

		}

		protected void AddRules()
		{

		}

		protected virtual void ValidateProperty(string propertyName)
		{
			this.Rules.Validate(this, propertyName);
		}

		protected virtual void Validate()
		{
			Type type = this.GetType();
			IRelationalMetaData meta = Base.GetMetaData(type);
			foreach (ITableEntityDescriptor table in meta.Tables)
			{
				foreach (IColumnDescriptor column in table.Columns)
				{
					PropertyInfo property =	type.GetProperty(column.PropertyName);
					this.Rules.Validate(property.Name, property.GetValue(this, null));
				}
			}
		}

		

		#endregion

		#region MetaData
		protected virtual IRelationalMetaData CreateRelationalMetaData()
		{
			return new BaseMetaData(this.GetType());
		}
	
		public static IRelationalMetaData GetMetaData(Type type)
		{
			IRelationalMetaData value =	MetaDataRegistry.Get(type);
			if (value == null)
			{
				Base entity = (Activator.CreateInstance(type) as Base);
				
				if (entity == null)
					throw new InvalidCastException(
						string.Format(
							"Could not cast type {0} to {1}",
							type.FullName,
							typeof(Base).FullName));

				value = entity.CreateRelationalMetaData();
				MetaDataRegistry.Add(type, value);

				if (entity is IDisposable)
					((IDisposable)entity).Dispose();
			}
			return value;
		}
		#endregion


		#endregion


		#region IDataValidationInfo Members

		IEnumerable<IValidationRule> IDataValidationInfo.this[string propertyName]
		{
			get { return this.Rules[propertyName]; }
		}

		#endregion
	}
}
