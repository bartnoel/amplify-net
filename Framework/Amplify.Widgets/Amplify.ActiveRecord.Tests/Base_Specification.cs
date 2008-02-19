//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using MbUnit.Framework;

namespace Amplify.ActiveRecord
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Linq;
	using System.Text;

	

	[TestFixture, TestCategory("Functional"), Author("Michael Herndon","mherndon@opensourceconnections.com")]
	public class Base_Specification
	{

		[TestFixtureSetUp]
		public void Setup()
		{
			Cfg.Context = new Surge.CodeAccessoryDataContext(ConfigurationManager.ConnectionStrings["default"].ConnectionString);
		}

		

		[Test]
		public void The_New_Method_Should_Create_A_New_Instance()
		{

			Surge.CodeAccessoryDataContext db = new Surge.CodeAccessoryDataContext();
			var x = (from o in db.Projects where o.Description = "blah" select 0);
			List<Surge.Project> projects = x.ToList();


			Person person = Person.New();
			person.IsNew.ShouldBe(true);
			person.Should(item => typeof(Person) == item.GetType(), "the object should be of type Person");
			person.FirstName.ShouldBeNull();
		}

		[Test]
		public void FindAll_Should_Work()
		{

			var x = Surge.Client.FindAll();
			foreach (Surge.Client client in x)
			{
				Console.WriteLine(client.Name);
				foreach (Surge.Project project in client.Projects)
					Console.WriteLine("\t" + project.Name);
			}
		}

		[Test]
		public void FindByWhere_ShouldWork()
		{
			var x = Surge.Client.FindAll(new Data.Options().Where("it.Name.StartsWith(@0) || Name.StartsWith(@1)", "B", "A").SortBy("Name ASC"));
			foreach (Surge.Client client in x)
			{
				Console.WriteLine(client.Name);
				foreach (Surge.Project project in client.Projects)
					Console.WriteLine("\t" + project.Name);
			}
		}

		#region Person

		public class Person : Base<Person>
		{
			public string FirstName
			{
				get { return(string)this["FirstName"]; }
				set { this["FirstName"] = value; }
			}

			protected override IEnumerable<string> Properties
			{
				get { throw new NotImplementedException(); }
			}

			protected override IEnumerable<string> PrimaryKeys
			{
				get { throw new NotImplementedException(); }
			}
		}

		#endregion
	}
}
