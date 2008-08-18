//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) CompanyName.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Data.SqlClientCe
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
		Describe(typeof(Migration)),
		InContext("should perform its specified behavor."),
		Tag("Functional"),
		By("Michael Herndon", "mherndon@opensourceconnections.com", "www.amptools.net")
	]
	public class MigrationObject : Spec
	{

		[It, Should(" have a public default constructor. ")]
		public void InvokeConstructor()
		{
			//Migration obj = new Migration();
			//obj.ShouldNotBeNull();
		}
	}
}
