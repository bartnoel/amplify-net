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
		Describe(typeof(ValidateLength)),
		InContext("should perform its specified behavor."),
		Tag("Functional"),
		By("Michael Herndon", "mherndon@opensourceconnections.com", "www.amptools.net")
	]
	public class ValidateLengthObject : Spec
	{

		[It, Should(" have a public default constructor. ")]
		public void InvokeConstructor()
		{
			ValidateLength obj = new ValidateLength();
			obj.ShouldNotBeNull();
			obj.RuleName.ShouldBe("ValidateLength");
		}

		[It, Should(" validate the value passed to it. ")]
		public void ValidateValue()
		{
			ValidateLength obj = new ValidateLength();
			obj.ShouldNotBeNull();
			obj.Minimum = 5;
			obj.Maximum = 100;
			obj.ValidateData("123").ShouldBeFalse();
			obj.ValidateData("12345").ShouldBeTrue();
		}

		[It, Should(" validate if the value does not exceed it's maximum allowed value. ")]
		public void ValidateMaximumValue()
		{
			ValidateLength obj = new ValidateLength();
			obj.Maximum = 7;
			obj.ValidateData("1234567").ShouldBeTrue();
			obj.ValidateData("12345678").ShouldBeFalse();
		}

		[It, Should(" validate if the value does not reach it's minimum allowed value. ")]
		public void ValidateMinimumValue()
		{
			ValidateLength obj = new ValidateLength();
			obj.Minimum = 3;
			obj.ValidateData("123").ShouldBeTrue();
			obj.ValidateData("12").ShouldBeFalse();
		}

		[It, Should(" validate if the value does not reach it's minimum allowed value. ")]
		public void ValidateSpecificRange()
		{
			ValidateLength obj = new ValidateLength();
			obj.Is = 3;
			obj.ValidateData("123").ShouldBeTrue();
			obj.ValidateData("12").ShouldBeFalse();
			obj.ValidateData("1234").ShouldBeFalse();
		}


	}
}
