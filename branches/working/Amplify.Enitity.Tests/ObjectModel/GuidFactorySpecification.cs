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
		Describe(typeof(GuidFactory)),
		InContext("should perform its specified behavor."),
		Tag("Functional"),
		By("Michael Herndon", "mherndon@opensourceconnections.com", "www.amptools.net")
	]
	public class GuidFactoryObject : Spec
	{

		[It, Should(" have a public default constructor. ")]
		public void InvokeConstructor()
		{
			GuidFactory obj = new GuidFactory();
			obj.ShouldNotBeNull();
			
		}

		[It, Should(" return a new guid that is not an empty guid ")]
		public void CreateValue()
		{
			GuidFactory obj = new GuidFactory();
			obj.CreateValue().GetType().ShouldBe(typeof(Guid));
			obj.CreateValue().ShouldNotBe(Guid.Empty);
		}
	}
}
