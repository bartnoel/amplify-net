//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify
{
	#region Using Statements
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Reflection;

	using Gallio.Framework;
	using MbUnit.Framework;

	using It = MbUnit.Framework.TestAttribute;
	using Describe = MbUnit.Framework.CategoryAttribute;
	using InContext = MbUnit.Framework.DescriptionAttribute;
	using Should = MbUnit.Framework.DescriptionAttribute;
	#endregion

	[ TestFixture,
		Author("Michael Herndon", "mherndon@opensourceconnections.com", "opensourceconnections.com"),
		Describe("ApplicationContext Specification"), 
		InContext("using the context for managing application wide values and configuration")
	]
	public class ApplicationContextObject : Spec
	{
		
		[It, Should(" get the correct context of the application such as being in testing. ")]
		public void GetTheCorrectContext()
		{
			ApplicationContext.IsWebsite.ShouldBeFalse();
			ApplicationContext.IsTesting.ShouldBeTrue();
			ApplicationContext.IsDevelopment.ShouldBeTrue(); // system.web/compilation debug="true"

		}

		[It, Should("")]
		public void ReflectionTest()
		{
			Type type = typeof(Person);
			Type[] types = type.GetInterfaces();
			types.Length.ShouldBe(2);
			object[] attrs = null;
			foreach(Type itype in types)
				if(typeof(Base).IsAssignableFrom(itype)  && typeof(Base) != itype) {
					attrs = itype.GetProperty("Name").GetCustomAttributes(typeof(DescriptionAttribute), true);
					break;
				}

			
			attrs.Length.ShouldBe(1);
			foreach (DescriptionAttribute attr in attrs)
				Console.WriteLine(attr);
		}

		[It, Should(" be able to get and set static properties on the fly. ")]
		public void GetAndSetProperties()
		{
			ApplicationContext.GetProperty("Test").ShouldBeNull();
			ApplicationContext.SetProperty("Test", "test");
			ApplicationContext.GetProperty("Test").ShouldBe("test");

		}

		public class  Person : Bob {

			public string Name { get; set; }
		}

		public interface Base
		{

		}

		public interface Bob : Base 
		{
			[Description("hi")]
			string Name { get; set; }
		}
		
	}
}
