//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) CompanyName.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.ActiveRecord
{
	#region Using Statements
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Reflection;

	using MbUnit.Framework;

	using Describe = MbUnit.Framework.TestsOnAttribute;
	using InContext = MbUnit.Framework.DescriptionAttribute;
	using It = MbUnit.Framework.TestAttribute;
	using Should = MbUnit.Framework.DescriptionAttribute;
	using By = MbUnit.Framework.AuthorAttribute;
	using Tag = MbUnit.Framework.CategoryAttribute;

	using Amplify.ObjectModel;
	using Amplify.Data;

#if LINQ
	using Amplify.Linq;
#endif
 
	#endregion

	[
		Describe(typeof(Base<>)),
		InContext("should perform its specified behavor."),
		Tag("Functional"),
		By("Michael Herndon", "mherndon@opensourceconnections.com", "www.amptools.net")
	]
	public class BaseGenericObject : Spec
	{

		[It, Should(" have a public default constructor. ")]
		public void InvokeConstructor()
		{
			
		}

		[It, Should(" create a new object and have the status of 'new'")]
		public void InvokeStaticNew()
		{
			Person person = Person.New();
			((IUnitOfWork)person).IsNew.ShouldBeTrue();
		}

		[It, Should(" create a new object with default values set ")]
		public void InvokeStaticNewWithValues()
		{
			Person person = Person.New(new Hash() { { "Name", "Michael" }, { "Age", 10 } });
			((IUnitOfWork)person).IsNew.ShouldBeTrue();
			person.Name.ShouldBe("Michael");
			person.Age.ShouldBe(10);
		}

	}


	public class Person : Base<Person>
	{

		[Column]
		public string Name
		{
			get { return (string)this.Get("Name"); }
			set { this.Set("Name", value); }
		}

		[Column]
		public int Age
		{
			get { return (int)this.Get("Age"); }
			set { this.Set("Age", value); }
		}

	}

 
}
