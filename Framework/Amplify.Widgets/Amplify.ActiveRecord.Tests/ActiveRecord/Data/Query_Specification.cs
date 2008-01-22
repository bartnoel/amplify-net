
namespace Amplify.ActiveRecord.Data.Tests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using MbUnit.Framework;

	[TestFixture]
	public class Query_Specification
	{

		[TestFixtureSetUp]
		public void Setup()
		{
			Cfg.DefaultConnectionString = "";
			Cfg.DefaultAdapter = new Adapters.SqlAdapter(Cfg.DefaultConnectionString); 
		}

		[Test]
		public void Selection_Should_Output_SelectQuery()
		{
			Selection selection = new Selection().Of(new[] { "FirstName", "Age" }).From("Person");
			Console.WriteLine(selection.ToString());
			selection.ToString().ShouldBe("SELECT FirstName, Age FROM Person ");

			selection.Where("FirstName = 'Michael'");

			Console.WriteLine(selection.ToString());
			selection.ToString().ShouldBe("SELECT FirstName, Age FROM Person WHERE FirstName = 'Michael' ");

			selection.And.Where("Age = ?", 27);
			Console.WriteLine(selection.ToString());

		}


	}
}
