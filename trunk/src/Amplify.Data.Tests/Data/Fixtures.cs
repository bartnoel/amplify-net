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
		Describe(typeof(Fixtures)),
		InContext("parsing an xml file to delete/update in the database."),
		Importance(Importance.Critical),
		By("Michael Herndon", "mherndon@opensourceconnections.com", "www.amptools.net")
	]
	public class FixturesObject : Spec
	{


		[
			It, Should("parse the xml specification file."), 
			Tag("Functional")
		]
		public void ParseXmlFile()
		{
			Fixtures fixtures = Fixtures.New(this.AssemblyLocation + "\\Fixtures", Adapter.Get());
			fixtures["TestList"].Name.ShouldBe("TestList");
			fixtures["TestList"].TableName.ShouldBe("TestList");
			fixtures["TestList"].Rows.Count.ShouldBe(2);
			fixtures["TestList"].Rows[0]["Name"].ShouldBe("'Blogging'");
			fixtures.Count.ShouldBe(2);
			fixtures["DummyFixtures"].Name.ShouldBe("DummyFixtures");
			fixtures["DummyFixtures"].TableName.ShouldBe("dbo.aspnet_DummyTable");
			fixtures["DummyFixtures"].Rows[1]["Url"].ShouldBe("'www.myspace.com'");
			fixtures["DummyFixtures"].Rows[1]["Text"].ShouldBe("'Blows Dust'");

		}

		[
			It, Should("remove and insert values from the fixture into the database."),
			Tag("Database")
		]
		public void UpdateDatabase()
		{
			Adapter adapter = new SqlClient.SqlAdapter();
			int count = -1;
			using (IDataReader dr = adapter.ExecuteReader("SELECT Count(*) FROM TestList"))
			{
				while (dr.Read())
				{
					count = dr.GetInt32(0);
				}
			}
			count.ShouldBe(0);

			Fixtures fixtures = Fixtures.New(this.AssemblyLocation + "\\Fixtures", adapter);
			fixtures["TestList"].RenewFixtures();

			using (IDataReader dr = adapter.ExecuteReader("SELECT Count(*) FROM TestList"))
			{
				while (dr.Read())
				{
					count = dr.GetInt32(0);
				}
			}
			count.ShouldBe(2);
		}
	}
}
