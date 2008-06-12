//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) CompanyName.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.ObjectModel
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
		Describe(typeof(EmptyGuidFactory)),
		InContext("should perform its specified behavor."),
		Tag("Functional"),
		By("Michael Herndon", "mherndon@opensourceconnections.com", "www.amptools.net")
	]
	public class EmptyGuidFactoryObject : Spec
	{

		[It, Should(" have a public default constructor. ")]
		public void InvokeConstructor()
		{
			EmptyGuidFactory obj = new EmptyGuidFactory();
			obj.ShouldNotBeNull();
		}

		[It, Should(" return an empty guid value. ")]
		public void CreateValue()
		{
			EmptyGuidFactory obj = new EmptyGuidFactory();
			obj.CreateValue().ShouldBe(Guid.Empty);
		}
	}
}
