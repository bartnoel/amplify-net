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
	public class ApplicationContextObject : Spec
	{
		
		[It, Should("lazy load or create the AmplifyConfiguration.")]
		public void GetAndValidationApplicationSection()
		{
			ApplicationContext.AmplifyConfiguration.ShouldNotBeNull();
			ApplicationContext.AmplifyConfiguration.ApplicationName.ShouldBe("Amplify.Net Application");
			ApplicationContext.AmplifyConfiguration.ConnectionStringName.ShouldBe("development");

			ApplicationContext.ApplicationName.ShouldBe("Amplify.Net Application");
			ApplicationContext.ConnectionStringName.ShouldBe("development");
		}

		[It, Should("have a valid connection string, if set up correctly.")]
		public void GetAndValidationConnectionStringSettings()
		{

			System.Configuration.Configuration c =
					System.Configuration.ConfigurationManager.OpenExeConfiguration(this.AssemblyLocation  + "\\Amplify.Tests.dll.config");

			// currently not testable.....

			//System.Configuration.ConfigurationManager.ConnectionStrings[
			//	"test"].ShouldNotBeNull();
			//System.Configuration.ConfigurationManager.ConnectionStrings[
			//	"development"].ConnectionString.Length.ShouldBeGreaterThan(0);


			//ApplicationContext.ConnectionString.ShouldNotBeNull();
			//ApplicationContext.ConnectionString.Length.ShouldBeGreaterThan(0);

		}

		
	}
}
