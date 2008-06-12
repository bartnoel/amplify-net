//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify
{
	using System;
	using System.Data;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Text;
	using System.Reflection;

	/// <summary>
	///  Maps Properties of the same name from one object to another.
	/// </summary>
	/// <remarks>
	///  <para>
	///		This class is heavily based upon the work done in Rock Lhokta's CSLA's DataMapper Class. 
	///  </para>
	/// </remarks>
	public static class PropertyMapper
	{

		public static void Map(object source, IDbCommand cmd)
		{
			Map(source, cmd, "@", new string[] { });
		}

		public static void Map(object source, IDbCommand cmd, params string[] ignoreList)
		{
			Map(source, cmd, "@", ignoreList);
		}

		public static void Map(
			object source,
			IDbCommand cmd,
			string token,
			params string[] ignoreList)
		{
			List<string> ignore = new List<string>(ignoreList);
			PropertyInfo[] sourceProperties =
			  GetSourceProperties(source.GetType());
			foreach (PropertyInfo fieldInfo in sourceProperties)
			{
				string propertyName = fieldInfo.Name;
				if (!ignore.Contains(propertyName))
				{
					object value = fieldInfo.GetValue(source, null);

					IDbDataParameter param = cmd.CreateParameter();
					param.ParameterName = token + propertyName;
					if (value != null)
						param.Value = value;
					else
						param.Value = DBNull.Value;

					cmd.Parameters.Add(param);
				}
			}
		}

		public static void Map(IDataReader dr, object target)
		{
			Map(dr, target, false, new string[] { });
		}

		public static void Map(IDataReader dr, object target, params string[] ignoreList)
		{
			Map(dr, target, false, ignoreList);
		}


		public static void Map(
		  IDataReader dr,
		  object target, bool suppressExceptions,
		  params string[] ignoreList)
		{
			List<string> ignore = new List<string>(ignoreList);
			Type targetType = target.GetType();
			for (int i = 0; i < dr.FieldCount; i++)
			{
				string propertyName = dr.GetName(i);
				if (!ignore.Contains(propertyName))
				{
					try
					{
						PropertyInfo info = targetType.GetProperty(propertyName);
						if (info != null)
						{
							object value = dr.GetValue(i);
							if (value == DBNull.Value)
								return;
							SetPropertyValue(target, propertyName, value);
						}
					}
					catch (Exception ex)
					{
						if (!suppressExceptions)
							throw new ArgumentNullException(
								string.Format("{0} ({1})",
								"Copying a property failed, property name: ", propertyName), ex);
					}
				}
			}
		}


		public static void Map(object source, object target)
		{
			Map(source, target, false, new string[] { });
		}

		public static void Map(object source, object target, params string[] ignoreList)
		{
			Map(source, target, false, ignoreList);
		}

		public static void Map(
		  object source, object target,
		  bool suppressExceptions,
		  params string[] ignoreList)
		{
			List<string> ignore = new List<string>(ignoreList);
			PropertyInfo[] sourceFields = GetSourceProperties(source.GetType());
			foreach (PropertyInfo propertyInfo in sourceFields)
			{
				string fieldName = propertyInfo.Name;
				if (!ignore.Contains(fieldName))
				{
					try
					{
						SetPropertyValue(
						  target, fieldName,
						  propertyInfo.GetValue(source, null));
					}
					catch (Exception ex)
					{
						if (!suppressExceptions)
							throw new ArgumentException(
							  String.Format("{0} ({1})",
							  "Copying a property failed, the property name: ", fieldName), ex);
					}
				}
			}
		}

		public static void SetPropertyValue(
		  object target, string fieldName, object value)
		{
			PropertyInfo propertyInfo =
			  target.GetType().GetProperty(fieldName);
			if (value == null)
				propertyInfo.SetValue(target, value, null);
			else
			{
				Type pType =
					GetPropertyType(propertyInfo.PropertyType);
				Type vType =
					GetPropertyType(value.GetType());

				if (pType.Equals(vType))
				{
					// types match, just copy value
					propertyInfo.SetValue(target, value, null);
				}
				else
				{
					// types don't match, try to coerce
					if (pType.Equals(typeof(Guid)))
						propertyInfo.SetValue(
						  target, new Guid(value.ToString()), null);
					else if (pType.IsEnum && vType.Equals(typeof(string)))
						propertyInfo.SetValue(target, Enum.Parse(pType, value.ToString()), null);
					else
						propertyInfo.SetValue(
						  target, Convert.ChangeType(value, pType), null);
				}
			}
		}


		private static PropertyInfo[] GetSourceProperties(Type sourceType)
		{
			List<PropertyInfo> result = new List<PropertyInfo>();
			PropertyDescriptorCollection props =
			  TypeDescriptor.GetProperties(sourceType);
			foreach (PropertyDescriptor item in props)
				if (item.IsBrowsable)
					result.Add(sourceType.GetProperty(item.Name));
			return result.ToArray();
		}


		public static Type GetPropertyType(Type fieldType)
		{
			Type type = fieldType;
			if(IsNullable(type))
				return Nullable.GetUnderlyingType(type);
			return type;
		}


		/// <summary>
		/// Gets the value of the property using reflection.
		/// </summary>
		/// <param name="target"> The object that you need the value from. </param>
		/// <param name="propertyName"> The name of the property that corrsponds to the value needed. </param>
		/// <returns> The value of the specified property. </returns>
		public static object GetValue(object target, string propertyName)
		{
			return GetValue(target, propertyName, null);
		}

		/// <summary>
		/// Gets the value of the property using reflection.
		/// </summary>
		/// <param name="target"> The object that you need the value from. </param>
		/// <param name="propertyName"> The name of the property that corrsponds to the value needed. </param>
		/// <param name="index"> The index of the value if this happens to be an indexed property. </param>
		/// <returns> The value of the specified property. </returns>
		public static object GetValue(object target, string propertyName, object[] index)
		{
			return target.GetType().GetProperty(propertyName).GetValue(target, index);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="target"></param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public static Type GetType(object target, string propertyName)
		{
			return target.GetType().GetProperty(propertyName).GetType();
		}
	
		/// <summary>
		/// Determines if the type is nullable, must use 'typeof', as using 'GetType'
		/// will not work.
		/// </summary>
		/// <param name="type"> The object type. </param>
		/// <returns> True if the type is nullable, false if not. </returns>
		public static bool IsNullable(Type type)
		{
			return (type.IsGenericType && 
				type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
		}
	}
}
