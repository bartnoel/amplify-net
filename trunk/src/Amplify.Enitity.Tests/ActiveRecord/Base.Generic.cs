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

		private bool propertyChanged = false;

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
		
		[It, Should(" automatically set the primary key if the type has an 'Id' property ")]
		public void GetPrimaryKey()
		{
			Person person = Person.New();
			ModelMetaInfo info = ModelMetaInfo.Get(typeof(Person));
			info.PrimaryTable.Columns.Count.ShouldBe(3);
			info.Columns.Count.ShouldBe(3);

			info.PrimaryKeys.Length.ShouldBe(1);
			info.PrimaryKeys[0].ShouldBe("Id");
		}

		
		[It, Should(" have default values provided by factories and rules ")]
		public void GetDefaultValues()
		{
			Person person = Person.New();
			person.Id.ShouldNotBe(Guid.Empty);
			person.Name.ShouldBe("");
			person.Age.ShouldBe(0);
		}

		[It, Should(" notify when a property is changed and mark it modified.")]
		public void FireNotifyPropertyChanged()
		{
			Person person = Person.New();
			person.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(person_PropertyChanged);

			person.Name = "Bob";
			this.propertyChanged.ShouldBe(true);
			((IUnitOfWork)person).IsModified.ShouldBe(true);
		}

		void person_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			this.propertyChanged = true;
		}
		 
		

	}


	public class Person : Base<Person>
	{
		[Column]
		public Guid Id
		{
			get { return (Guid)this.Get("Id"); }
		}

		[Column]
		public string Name
		{
			get { return (string)this.GetString("Name"); }
			set { this.Set("Name", value); }
		}

		[Column]
		public int Age
		{
			get { return this.GetInt32("Age"); }
			set { this.Set("Age", value); }
		}

	}

 
}
