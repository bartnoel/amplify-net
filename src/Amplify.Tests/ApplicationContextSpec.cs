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

	using Gallio.Framework;
	using MbUnit.Framework;

	using It = MbUnit.Framework.TestAttribute;
	using Describe = MbUnit.Framework.CategoryAttribute;
	using InContext = MbUnit.Framework.DescriptionAttribute;
	using Should = MbUnit.Framework.DescriptionAttribute;
	#endregion

	[
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

		[It, Should(" be able to get and set static properties on the fly. ")]
		public void GetAndSetProperties()
		{
			ApplicationContext.GetProperty("Test").ShouldBeNull();
			ApplicationContext.SetProperty("Test", "test");
			ApplicationContext.GetProperty("Test").ShouldBe("test");

		}

		
	}
}
