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
				ApplicationContext.ConnectionStringName = "test";
				ApplicationContext.ConnectionString = @"Server=.\sqlexpress;Integrated Security=true;AttachDbFilename=|DataDirectory|\amplify_test.mdf;User Instance=true;Database=amplify_test";
				ApplicationContext.ApplicationName = "Amplify.Data.Tests";
				DataSpec.Adapter = new Amplify.Data.SqlClient.SqlAdapter();
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
			try
			{
				using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(ApplicationContext.ConnectionString))
				{
					connection.Open();
					IDbCommand command = connection.CreateCommand();
					command.CommandType = CommandType.Text;
					command.CommandText = string.Format("USE MASTER \n ALTER DATABASE [{0}] \n SET SINGLE_USER; exec sp_detach_db '{0}', 'true';", "amplify_test");
					command.ExecuteNonQuery();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				Assert.Fail(ex.ToString() + "\n stack trace: " + ex.StackTrace);
			}
		}
	}
}
