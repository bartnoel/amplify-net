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
		Describe(typeof(PropertyProxy)),
		InContext("should perform its specified behavor."),
		Tag("Functional"),
		By("Michael Herndon", "mherndon@opensourceconnections.com", "www.amptools.net")
	]
	public class PropertyProxyObject : Spec
	{

		[It, Should(" register a decorated property. ")]
		public void RegisterProperty()
		{
			RegisteredPropertyList list = PropertyProxy.GetProperties(typeof(PropertyProxyObject));
			list.ShouldNotBeNull();
			list.Count.ShouldBe(0);

			DecoratedProperty name = PropertyProxy.Register(typeof(PropertyProxyObject), new DecoratedProperty("Name", typeof(String)));
			list = PropertyProxy.GetProperties(typeof(PropertyProxyObject));
			list.ShouldNotBeNull();
			list.Count.ShouldBe(1);
			list[0].ShouldBe(name);
		}
	}
}
