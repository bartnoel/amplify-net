//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace Amplify.Reflection
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Reflection;

	public static class ReflectionMixins
	{
	
		public static bool HasProperty(this object obj, string propertyName)
		{
			PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);
			return (propertyInfo != null);
		}

		public static object GetPropertyValue(this object obj, string propertyName)
		{
			return GetPropertyValue(obj, propertyName, null);
		}

		public static object GetPropertyValue(this object obj, string propertyName, object[] index)
		{
			PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);
			return (propertyInfo == null) ? null : propertyInfo.GetValue(obj, index);
		}


		public static void SetPropertyValue(this object obj, string propertyName, object value)
		{
			SetPropertyValue(obj, propertyName, value, null);
		}

		public static void SetPropertyValue(this object obj, string propertyName, object value, object[] index)
		{
			obj.GetType().GetProperty(propertyName).SetValue(obj, value, index);
		}
	}
}
