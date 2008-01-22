

namespace Amplify.ActiveRecord.Tests.ActiveRecord
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using MbUnit.Framework;

	[TestFixture, TestCategory("Functional")]
	public class UnitOfWork_Specification
	{

		[Test] 
		public void NewObjects_Should_Be_Valid_New_And_Saveable()
		{
			Person person = new Person() { FirstName = "Michael", Age = 27 };

			person.IsNew.ShouldBe(true);
			person.IsValid.ShouldBe(true);
			person.IsSaveable.ShouldBe(true);

		}

		[Test]
		public void FirstSave_Should_Insert_And_MarkOld()
		{
			Person person = new Person() { FirstName = "Michael", Age = 27 };
			person.Save().ShouldEqual(person);
			person.IsNew.ShouldBe(false);
			person.IsValid.ShouldBe(true);
			person.IsSaveable.ShouldBe(false);
			person.InsertCalled.ShouldBe(true);
			person.UpdateCalled.ShouldBe(false);
		}

		[Test]
		public void SecondSave_Should_Update()
		{
			Person person = new Person() { FirstName = "Michael", Age = 27 };
			person.Save();
			person.FirstName = "Bob";
			person.IsSaveable.ShouldBe(true);
			person.Save();
			person.UpdateCalled.ShouldBe(true);

		}


		#region Person

		public class Person : UnitOfWork 
		{
			public string FirstName
			{
				get { return (string)this["FirstName"]; }
				set { this["FirstName"] = value; }
			}

			public int Age
			{
				get { return (int)this["Age"]; }
				set { this["Age"] = value; }
			}

			protected override void Validate(string propertyName, object value)
			{
				this.ValidateCalled = true;
				base.Validate(propertyName, value);
			}

			internal bool DeleteCalled { get; set; }
			internal bool DeleteSelfCalled { get; set; }
			internal bool SaveCalled { get; set; }
			internal bool InsertCalled { get; set; }
			internal bool UpdateCalled { get; set; }
			internal bool ValidateCalled { get; set; } 

			protected override void DeleteSelf()
			{
				this.DeleteSelfCalled = true;
			}

			protected override void Insert()
			{
				this.InsertCalled = true;
			}

			protected override void Update()
			{
				this.UpdateCalled = true;
			}

			public override void Delete()
			{
				this.DeleteCalled = true;
				base.Delete();
			}
		}



		#endregion
	}
}
