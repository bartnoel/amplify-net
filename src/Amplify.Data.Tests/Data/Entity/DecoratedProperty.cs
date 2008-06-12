//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) CompanyName.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Data.Entity
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
		Describe(typeof(DecoratedProperty)),
		InContext("should perform its specified behavor."),
		Tag("Functional"),
		By("Michael Herndon", "mherndon@opensourceconnections.com", "www.amptools.net")
	]
	public class DecoratedPropertyObject : Spec
	{

		[It, Should(" should have values passed to it from it's constructor.")]
		public void Test()
		{
			DecoratedProperty obj = new DecoratedProperty("Name", typeof(string), "");
			obj.Name.ShouldBe("Name");
			obj.Type.ShouldBe(typeof(string));
			obj.DefaultValue.ShouldBe("");
			obj.IsSealed.ShouldBe(false);
			obj.IsReadOnly.ShouldBe(false);

			obj = new DecoratedProperty("Id", typeof(Guid), new GuidFactory(), true, true);

			obj.Name.ShouldBe("Id");
			obj.Type.ShouldBe(typeof(Guid));
			obj.DefaultValue.ShouldNotBeNull();
			obj.DefaultValue.ShouldNotBe(Guid.Empty);
			obj.IsSealed.ShouldBe(true);
			obj.IsReadOnly.ShouldBe(true);
		}
	}
}
