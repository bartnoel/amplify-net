using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MbUnit.Framework
{
	public static class BddMixins
	{

		public static IDictionary<TKey, TValue> ShouldContainKey<TKey, TValue>(this  IDictionary<TKey, TValue> obj, TKey key)
		{
			Assert.IsTrue(obj.ContainsKey(key), "Dictionary should contain key {0}".Fuse(key.ToString()));
			return obj;
		}

		internal static string Fuse(this string obj, params object[] args)
		{
			return string.Format(obj, args);
		}

		public static object ShouldBe(this object obj, object value)
		{
			Assert.AreEqual(value, obj, "The value ({1}) should have equaled ({0}).".Fuse(value, obj));
			return obj;
		}

		public static T And<T>(this T obj)
		{
			return obj;
		}

		public static void ShouldBe(this object obj, object value, string message)
		{
			Assert.AreEqual(value, obj, message);
		}

		public static void ShouldBe(this int obj, int value)
		{
			Assert.AreEqual(value, obj, "The value ({1}) should have equaled ({0}).".Fuse(value, obj));
		}
		
		public static void ShouldBe(this int obj, int value, string message)
		{
			Assert.AreEqual(value, obj, message);
		}

		public static void ShouldBe(this double obj, double value)
		{
			Assert.AreEqual(value, obj, "The value ({1}) should have equaled ({0}).".Fuse(value, obj));
		}


		public static void ShouldBe(this double obj, double value, string message)
		{
			Assert.AreEqual(value, obj, message);
		}


		public static void ShouldBe(this float obj, float value)
		{
			Assert.AreEqual(value, obj, "The value ({1}) should have equaled ({0}).".Fuse(value, obj));
		}


		public static void ShouldBe(this float obj, float value, string message)
		{
			Assert.AreEqual(value, obj, message);
		}


		public static IComparable ShouldBeGreaterThan(this IComparable obj, IComparable value)
		{
			Assert.GreaterEqualThan(obj, value, "The value ({0}) should have been greater than ({1})".Fuse(obj, value));
			return obj;
		}

		public static void ShouldBeGreaterThan(this IComparable obj, IComparable value, string message)
		{
			Assert.Greater(obj, value, message);
		}


		public static void ShouldBeGreaterOrEqualTo(this IComparable obj, IComparable value)
		{
			Assert.GreaterEqualThan(obj, value, "The value ({0}) should have been greater or equal to ({1})".Fuse(obj, value));
		}

		public static void ShouldBeGreaterOrEqualTo(this IComparable obj, IComparable value, string message)
		{
			Assert.GreaterEqualThan(obj, value, message);
		}

		public static void ShouldBeLessThan(this IComparable obj, IComparable value)
		{
			Assert.LowerThan(obj, value, "The value ({0}) should have been less than ({1})".Fuse(obj, value)); 
		}

		public static void ShouldBeLessThan(this IComparable obj, IComparable value, string message)
		{
			Assert.LowerThan(obj, value);
		}

		public static void ShouldBeTrue(this bool obj)
		{
			Assert.IsTrue(obj, "The condition should have been true.");
		}

		public static void ShouldBeTrue(this bool obj, string message)
		{
			Assert.IsTrue(obj, message);
		}


		public static void ShouldBeFalse(this bool obj)
		{
			Assert.IsFalse(obj, "The condition should have been false.");
		}

		public static void ShouldBeFalse(this bool obj, string message)
		{
			Assert.IsFalse(obj, message);
		}

		public static void ShouldBeNull(this object obj)
		{
			Assert.IsNull(obj, "The value should have been null.");
		}

		public static void ShouldNotBeNull(this object obj)
		{
			Assert.IsNotNull(obj, "The value should have not been null.");
		}

		public static void ShouldBeNull(this object obj, string message)
		{
			Assert.IsNull(obj, message);
		}

		public static void ShouldNotBeNull(this object obj, string message)
		{
			Assert.IsNotNull(obj,  message); 
		}

		public static void ShouldBeIn(this object obj, System.Collections.IEnumerable enumerable)
		{
			Assert.In(obj, enumerable, "The value ({0}) should have been been found in the collection of items.".Fuse(obj));
		}

		public static void ShouldBeIn(this object obj, System.Collections.IEnumerable enumerable, string message)
		{
			Assert.In(obj, enumerable, message);  
		}

		public static string ShouldContain(this string obj, string value)
		{
			Assert.IsTrue(obj.Contains(value), "The string ({1}) should have contained ({0})".Fuse(obj, value));
			return obj;
		}

		public static string ShouldContain(this string obj, string value, string message)
		{
			Assert.IsTrue(obj.Contains(value), message);
			return obj;
		}

		public static object ShouldBeInstanceOf(this object obj, Type type, string message)
		{
			Assert.IsInstanceOfType(type, obj.GetType(), message);
			return obj;
		}

		public static object ShouldBeInstanceOf(this object obj, Type type)
		{
			Assert.IsInstanceOfType(type, obj.GetType(), "Object should have been of type {0}".Fuse(type.FullName));
			return obj;
		}

		public static object ShouldNotBe(this object obj, object value)
		{
			Assert.AreNotEqual(value, obj, "The value ({0}) should not be ({1}).", obj, value);
			return obj;
		}

		public static object ShouldNotBe(this object obj, object value, string message)
		{
			Assert.AreNotEqual(value, obj, message);
			return obj;
		}

	}
}
