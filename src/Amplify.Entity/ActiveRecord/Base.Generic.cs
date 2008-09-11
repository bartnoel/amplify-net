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
	using System.Data;
	
	using System.Reflection;
	using System.Text;

	using Amplify.Linq;
	using Amplify.ComponentModel;
	using Amplify.Data;
	using Amplify.Data.Validation;

	[Serializable]
	public abstract partial  class Base<T> : Base, IDataErrorInfo, IDataValidationInfo, IWebFormValidation 
		where T: Base<T> 
	{
		

		private ValidationRules rules;

		private static ModelMetaInfo s_modelMetaInfo = null;

		protected bool ValidateOnChange { get; set; }

		protected static string Mode { get; set; }

		protected virtual Adapter Adapter 
		{
			get { return Adapter.Get(Mode); }
		}

		static Base()
		{
			InitializeOnce();
		}

		public Base()
		{
			this.Initialize();
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

		protected ModelMetaInfo ModelInfo
		{
			get {
				if (s_modelMetaInfo == null)
					s_modelMetaInfo = ModelMetaInfo.Get(this.GetType());
				return s_modelMetaInfo;
			}
		}


		#region Get/Set Overrides 
		
	
		protected virtual object Get(string propertyName, bool checkType)
		{
			object value = base.Get(propertyName);
			if (value is IEntityRef)
			{
				IEntityRef entity = (IEntityRef)value;
				if (entity.IsSet)
					return entity.Entity;
				return null;
			}
			return value;
		}

		protected override void Set(string propertyName, object value, bool markChanged, bool checkType)
		{
			if (checkType)
			{
				ModelMetaInfo info = ModelInfo;
				foreach (ColumnAttribute column in info.Columns)
				{
					if (propertyName.ToLower() == column.Property.Name.ToLower())
					{
						if (value == null)
						{
							if (column.Default != null && column.GetDefaultValue() is ValueType)
								throw new Exception("A value type can not be null");
						}

						// attempt to change the property type.
						if (value.GetType() != column.Property.PropertyType)
							value = Convert.ChangeType(value, column.Property.PropertyType);
					}
				}
			}
			this.Set(propertyName, value, markChanged);
		}
		#endregion

		protected virtual void Initialize()
		{
			this.IsNew = true;
			this.ValidateOnChange = true;
			ModelMetaInfo info = ModelMetaInfo.Get(typeof(T));
			
			foreach (ColumnAttribute column in info.Columns)
				this.Set(column.Property.Name, column.GetDefaultValue());
			
			foreach (AssocationAttribute association in info.Associations)
				this.Set(association.Property.Name, Unset);
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
			info.Tables.Add(info.PrimaryTable);

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

						if (string.IsNullOrEmpty(column.ColumnName))
							column.ColumnName = property.Name;
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

		public override object Save()
		{
			if(!this.ValidateOnChange) {
				ModelMetaInfo info = ModelMetaInfo.Get(this.GetType());
				foreach (ColumnAttribute column in info.Columns)
					this.Rules.Validate(this, column.Property.Name);
			}

			bool start =  this.Adapter.StartTransaction();
			object value = base.Save();

			if (start)
				this.Adapter.Commit();

			return value;
		}

		protected override void Insert()
		{
			Type type = typeof(T);
			ModelMetaInfo info = ModelMetaInfo.Get(type);
			Hash temp = new Hash();
			string[] columns = new string[info.Columns.Count];
			object[] values = new object[info.Columns.Count];

			foreach(ColumnAttribute column in info.Columns)
				temp.Add(column.ColumnName, this[column.Property.Name]);
			
			temp.Keys.CopyTo(columns, 0);
			temp.Values.CopyTo(values, 0);

			object id =	this.Adapter.Insert(Inflector.Pluralize(info.Tables[0].TableName),
				columns, values);

			string primary = info.PrimaryKeys[0];
			object value = this[primary];
			if(value is int)
				this[primary] = id;

			this[info.PrimaryKeys[0].ToLower()] = value;
		}

		protected override void Update()
		{
			Type type = typeof(T);
			ModelMetaInfo info = ModelMetaInfo.Get(type);
			Hash temp = new Hash();

			string[] columns = new string[info.Columns.Count];
			object[] values = new object[info.Columns.Count];

			foreach(ColumnAttribute column in info.Columns)
				temp.Add(column.ColumnName, this[column.Property.Name]);
			
			temp.Keys.CopyTo(columns, 0);
			temp.Values.CopyTo(values, 0);

			this.Adapter.Update(Inflector.Pluralize(info.Tables[0].TableName),
				columns, values, string.Format(" WHERE {0} = '{1}'", info.PrimaryKeys[0], this[info.PrimaryKeys[0]]));

		}

		public override bool Delete()
		{
			Type type = typeof(T);
			ModelMetaInfo info = ModelMetaInfo.Get(type);

			bool start = this.Adapter.StartTransaction();
			int value = this.Adapter.Delete(Inflector.Pluralize(info.Tables[0].TableName), this[info.PrimaryKeys[0]]);
			
			this.DeleteChildren();
			
			if (start)
				this.Adapter.Commit();

			return (value > 0);
		}

		protected override void Fill(object state)
		{
			base.Fill(state);
			if (state is object[])
			{
				object[] pair = (object[])state;

				IDataReader dr = (IDataReader)pair[0];
				ModelMetaInfo info = ModelMetaInfo.Get(this.GetType());

				for (int i = 0; i < dr.FieldCount; i++)
				{
					string name = dr.GetName(i);
					foreach(ColumnAttribute column in info.Columns)
						if(column.ColumnName.ToLower() == name.ToLower())
							this[column.Property.Name] = dr[i];
				}

				if (pair.Length > 1)
					this.FillChildren((string[])pair[1], dr);
			}
		}

		protected virtual void FillChildren(string[] includes, object state) 
		{

		}

		protected override void SaveChildren()
		{
			
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
