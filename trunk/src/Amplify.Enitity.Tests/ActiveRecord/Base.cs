//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) CompanyName.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.ActiveRecord
{
	#region Using Statements
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Reflection;

	using MbUnit.Framework;

	using Describe = MbUnit.Framework.TestsOnAttribute;
	using InContext = MbUnit.Framework.DescriptionAttribute;
	using It = MbUnit.Framework.TestAttribute;
	using Should = MbUnit.Framework.DescriptionAttribute;
	using By = MbUnit.Framework.AuthorAttribute;
	using Tag = MbUnit.Framework.CategoryAttribute;
	#endregion

	[
		Describe(typeof(Base<>)),
		InContext("should perform its specified behavor."),
		Tag("Functional"),
		By("Michael Herndon", "mherndon@opensourceconnections.com", "www.amptools.net")
	]
	public class BaseObject : Spec
	{

		[It, Should(" have a public default constructor. ")]
		public void InvokeConstructor()
		{
			
		}

		[It, Should(" invoke the static contructor and provide type information. ")]
		public void InvokeTypes()
		{
			Type1.Call();
			Type1.Type.ShouldBe(typeof(Type1));

			Type3.Call();
			Type3.Type.ShouldBe(typeof(Type2));

			Type2.Call();
			Type2.Type.ShouldBe(typeof(Type2));

			
		}
	}


	public class Person : Base<Person>
	{

		[Column]
		public string Name
		{
			get { return (string)this.Get("Name"); }
			set { this.Set("Name", value); }
		}

		[Column]
		public int Age
		{
			get { return (int)this.Get("Age"); }
			set { this.Set("Age", value); }
		}

	}

	#region BaseMock with types
	public class BaseMock<T> : Base<T> where T : Base<T>
	{
		public static Type Type { get; set; }

		/**
		 * the static constructor of a generic type should be called 
		 * once for each type, because each type is compiled as its own
		 * unique type. However a method on the base class must be called
		 * before its invoked.
		 */
		static BaseMock()
		{
			InitializeOnce();
		}

		protected static void InitializeOnce() 
		{
			
			Type = typeof(T);
			object[] attr = Type.GetCustomAttributes(typeof(PolymorphicAttribute), true);
			foreach (PolymorphicAttribute poly in attr)
			{
					object obj = Activator.CreateInstance(poly.Type);
					MethodInfo method = poly.Type.GetMethod("InitializeOnce", BindingFlags.Static);

					method.Invoke(obj, null);
			}
		}

		public static void Call() 
		{
			return;
		}
	}

	public class Type1 : BaseMock<Type1>
	{

	}

	public class PolymorphicAttribute : System.Attribute
	{
		public Type Type { get; set; }

		public PolymorphicAttribute(Type type)
		{
			this.Type = type;
		}
	}

	[Polymorphic(typeof(Type3))]
	public class Type2 : BaseMock<Type2>
	{

	}

	public class Type3 : Type2
	{



		new protected static void InitializeOnce()
		{
			Type type = typeof(Type3);
			Console.WriteLine(Type);
			Type3.Type = type;
		}

	}
	#endregion 
}
