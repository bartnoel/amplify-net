//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) CompanyName.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Data
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

		


		[It, Should(" create a test table and then remove it. ")]
		public void InvokeInitialSchema()
		{
			Adapter adapter = DataSpec.Adapter;
			List<string> tables = adapter.GetTableNames();
			tables.Contains("TestTable").ShouldBeFalse();


			InitialSchema migration = new InitialSchema();
			migration.Adapter = adapter;
			migration.Up();

			tables = adapter.GetTableNames();
			tables.Contains("TestTable").ShouldBeTrue();


			migration.Down();
			tables = adapter.GetTableNames();
			tables.Contains("TestTable").ShouldBeFalse();

		}


		public class InitialSchema : Migration
		{

			public override void Up()
			{
				this.CreateTable("TestTable", delegate(TableDefinition t)
				{
					t.Column("Name",			@string,	Limit => 100,	Default => "No One");
					t.Column("Description",		@string,	Limit => 255);
				});
			}

			public override void Down()
			{
				this.DropTable("TestTable");
			}
		}

	}
}
