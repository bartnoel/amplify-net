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
		Describe(typeof(ValidateFormat)),
		InContext("should perform its specified behavor."),
		Tag("Functional"),
		By("Michael Herndon", "mherndon@opensourceconnections.com", "www.amptools.net")
	]
	public class ValidateFormatObject : Spec
	{

		[It, Should(" have a public default constructor. ")]
		public void InvokeConstructor()
		{
			ValidateFormat obj = new ValidateFormat();
			obj.ShouldNotBeNull();
		}

		[It, Should(" validate the value is in the right format. ")]
		public void ValidateValue()
		{
			ValidateFormat obj = new ValidateFormat();
			obj.ShouldNotBeNull();
			obj.With = @"^(\d{5}-\d{4})|(\d{5})$";
			obj.Validate("22902").ShouldBeTrue();
			obj.Validate("218").ShouldBeFalse();
		}
	}
}
