//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.ActiveRecord
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	
	using System.Reflection;
	using System.Text;

	using Amplify.Linq;
	using Amplify.ComponentModel;
	using Amplify.Data.Validation;

	public abstract partial  class Base<T> : Base, IDataErrorInfo, IDataValidationInfo, IWebFormValidation 
		where T: Base<T> 
	{

		private ValidationRules rules;

		protected bool ValidateOnChange { get; set; }

		protected static string Mode { get; private set; }

		

		static Base()
		{
			InitializeOnce();
		}

		protected ValidationRules Rules
		{
			get
			{
				if (this.rules == null)
					this.rules = new ValidationRules(ModelMetaInfo.Get(this.GetType()).Rules);
				return this.rules;
			}
		}

		#region Set Overrides 
		protected virtual void Set(string propertyName, object value, bool markChanged)
		{
			bool changed = !Object.Equals(Get(propertyName), value);
			if (changed)
			{
				if (markChanged)
					this.NotifyPropertyChanging(propertyName, value);
				
				base.Set(propertyName, value);

				if (markChanged)
				{
					this.IsModified = true;
					this.NotifyPropertyChanged(propertyName, value);
				}
			}
		}

		protected override void Set(string propertyName, object value)
		{
			this.Set(propertyName, value, true);
		}
		#endregion

		protected virtual void Initialize()
		{
			this.IsNew = true;
			this.ValidateOnChange = true;
			ModelMetaInfo info = ModelMetaInfo.Get(typeof(T));
			foreach (ColumnAttribute column in info.Columns)
				this.Set(column.Property.Name, column.GetDefaultValue());
		}


		private static void InitializeOnce() 
		{
			Mode = "production";
			if (ApplicationContext.IsDevelopment)
				Mode = "development";
			else if (ApplicationContext.IsTesting)
				Mode = "test";
		
			Type type = typeof(T);
			ModelMetaInfo info = ModelMetaInfo.Get(type);

			object[] attrs = type.GetCustomAttributes(typeof(TableAttribute), true);

			if (attrs != null && attrs.Length > 0)
			{
				foreach (TableAttribute table in attrs)
				{
					if (table is PrimaryTableAttribute)
						info.PrimaryTable = (PrimaryTableAttribute)table;
					info.Tables.Add(table);
				}
			}

			if (info.PrimaryTable == null)
				info.PrimaryTable = new PrimaryTableAttribute()
				{
					TableName = Inflector.Pluralize(type.Name)
				};

			PropertyInfo[] properties = type.GetProperties();
			foreach (PropertyInfo property in properties)
			{
				attrs = property.GetCustomAttributes(true);
				foreach (object attribute in attrs)
				{
					if (attribute is ValidationAttribute)
					{
						IValidationRule rule = ((ValidationAttribute)attribute).Rule;
						if (rule is ValidationRule)
						{
							ValidationRule validation = (ValidationRule)rule;
							validation.PropertyName = property.Name;
							validation.EntityType = type.Name;
						}
						info.Rules.Add(rule);
					}

					if (attribute is ColumnAttribute)
					{
						ColumnAttribute column = (ColumnAttribute)attribute;
						if (string.IsNullOrEmpty(column.TableName))
						{
							info.PrimaryTable.Columns.Add(column);
							column.TableName = info.PrimaryTable.TableName;
						}
						else
						{
							foreach (TableAttribute table in info.Tables)
								if (table.TableName == column.TableName)
									table.Columns.Add(column);
						}

						column.Property = property;
						if (property.Name == "Id")
							column.IsPrimaryKey = true;

						info.Columns.Add(column); 

						if(column.Property.PropertyType == typeof(Guid))
							column.Default = typeof(ObjectModel.EmptyGuidFactory);
					}

					if (attribute is AssocationAttribute)
					{
						AssocationAttribute association = (AssocationAttribute)attribute;
						info.Associations.Add(association);
						association.Property = property;
					}
				}
			}


			List<string> primaryKeys = new List<string>();
			foreach(ColumnAttribute column in info.PrimaryTable.Columns)
			{
				if(column.IsPrimaryKey) {
					primaryKeys.Add(column.Property.Name);
					if(column.Property.PropertyType == typeof(Guid))
						column.Default = typeof(ObjectModel.GuidFactory);
				}
			}

			info.PrimaryKeys = primaryKeys.ToArray();
		}


		#region IDataErrorInfo Members

		string IDataErrorInfo.Error
		{
			get { return this.Rules.GetErrors(); }
		}

		string IDataErrorInfo.this[string columnName]
		{
			get { return this.Rules.GetErrors(columnName); }
		}

		#endregion

		#region IDataValidationInfo Members

		IEnumerable<IValidationRule> IDataValidationInfo.this[string propertyName]
		{
			get { return this.Rules[propertyName]; }
		}

		#endregion

		#region IWebFormValidation Members

		List<System.Web.UI.IValidator> IWebFormValidation.GetValidators(string propertyName)
		{
			return this.Rules.GetValidators(propertyName);
		}

		#endregion
	}
}
