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

		

		static Base()
		{
			InitializeOnce();
		}


		private static void InitializeOnce() 
		{
		
			Type type = typeof(T);
			ModelMetaInfo info = ModelMetaInfo.Get(type);

			object[] attrs = type.GetCustomAttributes(typeof(TableAttribute), true);

			if (attrs != null && attrs.Length > 0)
			{
				info.PrimaryTable = (PrimaryTableAttribute)attrs.SingleOrDefault(
								t => ((TableAttribute)t).IsPrimary);

				info.Tables.AddRange(attrs.Cast<TableAttribute>());
			}

			if (info.PrimaryTable == null)
				info.PrimaryTable = new PrimaryTableAttribute()
				{
					TableName = type.Name.Pluralize()
				};

			PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly);
			foreach (PropertyInfo property in properties)
			{
				attrs = property.GetCustomAttributes(true);
				foreach (object attribute in attrs)
				{
					if (attribute is ColumnAttribute)
					{
						ColumnAttribute column = (ColumnAttribute)attribute;
						if (column.TableName.IsNullOrEmpty())
						{
							info.PrimaryTable.Columns.Add(column);
							column.TableName = info.PrimaryTable.TableName;
						}
						else
							info.Tables.Single(t => t.TableName == column.TableName).Columns.Add(column);

						column.Property = property;
						if (property.Name == "Id")
							column.IsPrimaryKey = true;

						info.Columns.Add(column); 
					}

					if (attribute is AssocationAttribute)
					{
						AssocationAttribute association = (AssocationAttribute)attribute;
						info.Associations.Add(association);
						association.Property = property;
					}
				}
			}
		}

	}
}
