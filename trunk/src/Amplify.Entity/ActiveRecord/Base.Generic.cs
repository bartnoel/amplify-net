//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.ActiveRecord
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Text;

	using Amplify.Linq;

	public abstract partial  class Base<T> : Base 
		where T: Base<T> 
	{

		private bool ValidateOnChange { get; set; }

		protected static string Mode { get; internal set; }

		static Base()
		{
			InitializeOnce();
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
					TableName = Inflector.Net.Inflector.Pluralize(type.Name)
				};

			PropertyInfo[] properties = type.GetProperties();
			foreach (PropertyInfo property in properties)
			{
				attrs = property.GetCustomAttributes(true);
				foreach (object attribute in attrs)
				{
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

	}
}
