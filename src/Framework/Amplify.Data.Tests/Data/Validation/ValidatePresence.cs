//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) CompanyName.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Data.Validation
{
	#region Using Statements
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using MbUnit.Framework;

	using Describe = MbUnit.Framework.TestsOnAttribute;
	using InContext = MbUnit.Framework.DescriptionAttribute;
	using It = MbUnit.Framework.TestAttribute;
	using Should = MbUnit.Framework.DescriptionAttribute;
	using By = MbUnit.Framework.AuthorAttribute;
	using Tag = MbUnit.Framework.CategoryAttribute;
	#endregion

	[
		Describe(typeof(ValidatePresence)),
		InContext("should perform its specified behavor."),
		Tag("Functional"),
		By("Michael Herndon", "mherndon@opensourceconnections.com", "www.amptools.net")
	]
	public class ValidatePresenceObject : Spec
	{

		[It, Should(" have a public default constructor. ")]
		public void InvokeConstructor()
		{
			ValidatePresence obj = new ValidatePresence();
			obj.ShouldNotBeNull();
			obj.RuleName.ShouldBe("ValidatePresence");
		}

		[It, Should(" validate the value passed to the object. ")]
		public void ValidateValue()
		{
			ValidatePresence obj = new ValidatePresence();
			obj.DefaultValue = "";
			obj.Validate("").ShouldBeFalse();
			obj.Validate("Has Value").ShouldBeTrue();

			obj.If = (value) => !string.IsNullOrEmpty((value as string));
			obj.Validate("").ShouldBeTrue();
		}

	}
}
