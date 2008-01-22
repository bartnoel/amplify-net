//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Linq;
	using System.Text;

	using MbUnit.Framework;

	public static class Mixin
	{

		public static object And(this object obj)
		{
			return obj;
		}

		public static object Should(this object obj, Func<object, bool> proc, string message)
		{
			
			Assert.IsTrue(proc(obj), message);
			return obj;
		}

		public static object Should<T>(this T obj, Func<T, bool> proc, string message)
		{
			Assert.IsTrue(proc(obj), message);
			return obj;
		}

		public static object ShouldEqual(this object obj, object value)
		{
			Assert.AreEqual(obj, value, "Object should have equaled " + GetValue(value));
			return obj;

		}

		public static object ShouldEqual(this object obj, object value, string message)
		{
			Assert.AreEqual(obj, value, message);
			return obj;
		}

		public static object ShouldBe(this object obj, object value)
		{
			Assert.IsTrue(obj.Equals(value),
				string.Format(
					"The object value should have been '{0}', but was '{1}'",
					GetValue(value),
					GetValue(obj)
					)
				);
			return obj;
		}

		public static object ShouldBe(this object obj, object value, string message)
		{
			Assert.IsTrue(obj == value, message);
			return obj;
		}

		public static object ShouldBeNull(this object obj)
		{
			Assert.IsNull(obj, "The object value should have been null.");
			return obj;
		}

		public static object ShouldBeNull(this object obj, string message)
		{
			Assert.IsNull(obj, message);
			return obj;
		}

		public static object ShouldNotBeNull(this object obj)
		{
			Assert.IsNotNull(obj, "The object value should have been null.");
			return obj;
		}

		public static object ShouldNotBeNull(this object obj, string message)
		{
			Assert.IsNotNull(obj, message);
			return obj;
		}

		public static object ShouldBeInstanceOf(this object obj, Type type)
		{
			Assert.IsInstanceOfType(
				type,
				obj,
				string.Format(
					"Object of type {0} should have been instance of {1}",
					GetType(obj),
					type.ToString()
				)
			);
			return obj;
		}


		public static object ShouldBeInstanceOf(this object obj, Type type, string message)
		{
			Assert.IsInstanceOfType(
				type,
				obj,
				message
			);
			return obj;
		}

		private static string GetValue(object value) 
		{
			if(value == null)
				return "null";
			return value.ToString();
		}

		private static string GetType(object value)
		{
			if (value == null)
				return "null";
			else
				return value.GetType().ToString();
		}

	}
}
