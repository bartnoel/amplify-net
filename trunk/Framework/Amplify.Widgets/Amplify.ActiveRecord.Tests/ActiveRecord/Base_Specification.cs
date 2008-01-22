//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace Amplify.ActiveRecord.Tests.ActiveRecord
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using MbUnit.Framework;

	[TestFixture, TestCategory("Functional"), Author("Michael Herndon","mherndon@opensourceconnections.com")]
	public class Base_Specification
	{

		[Test]
		public void The_New_Method_Should_Create_A_New_Instance()
		{
			Person person = Person.New();
			person.IsNew.ShouldBe(true);
			person.Should(item => typeof(Person) == item.GetType(), "the object should be of type Person");
			person.FirstName.ShouldBeNull();
		}



		#region Person

		public class Person : Base<Person>
		{
			public string FirstName
			{
				get { return(string)this["FirstName"]; }
				set { this["FirstName"] = value; }
			}
		}

		#endregion
	}
}
