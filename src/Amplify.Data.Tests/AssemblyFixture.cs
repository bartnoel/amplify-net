//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) CompanyName.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify
{
	#region Using Statements
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;
	using System.Text;

	using MbUnit.Framework;

	using Describe = MbUnit.Framework.TestsOnAttribute;
	using InContext = MbUnit.Framework.DescriptionAttribute;
	using It = MbUnit.Framework.TestAttribute;
	using Should = MbUnit.Framework.DescriptionAttribute;
	using By = MbUnit.Framework.AuthorAttribute;
	using Tag = MbUnit.Framework.CategoryAttribute;

	using Amplify.Data;
	#endregion

	[
		AssemblyFixture,
		InContext("should perform its specified behavor."),
		Tag("Functional"),
		By("Michael Herndon", "mherndon@opensourceconnections.com", "www.amptools.net")
	]
	public class AssemblyFixtureObject 
	{

		[FixtureSetUp]
		public void InvokeOnStartUp()
		{
			try
			{
				ApplicationContext.IsTesting = true;
				//DataSpec.Adapter = Adapter.Get();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				Assert.Fail(ex.ToString() + "\n stack trace: " + ex.StackTrace);
			}
		}

		[FixtureTearDown]
		public void InvokeOnTearDown()
		{
			
		}
	}
}
