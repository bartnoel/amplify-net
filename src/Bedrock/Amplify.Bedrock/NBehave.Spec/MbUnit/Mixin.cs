using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace NBehave.Spec.MbUnit
{
	public static class Mixin 
	{

		public static T ShouldContainKey<T>(this T obj, object value) where T : IDictionary
		{
			obj.Keys.ShouldContain(value);
			return obj;
		}

		public static T ShouldContainValue<T>(this T obj, object value) where T : IDictionary
		{
			obj.Values.ShouldContain(value);
			return obj;
		}

		public static T ShouldBe<T>(this T obj, object value)
		{
			 obj.ShouldEqual(value);
			 return obj;
		}
	}
}
