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
	public class AssemblyInitialization: AssemblySpecificationConfiguration
	{

		public override void BeforeRun()
		{
			base.BeforeRun();
			System.Configuration.ConfigurationManager.OpenExeConfiguration("Amplify.Tests.dll.config");
			
		
		
		}

	}
}
