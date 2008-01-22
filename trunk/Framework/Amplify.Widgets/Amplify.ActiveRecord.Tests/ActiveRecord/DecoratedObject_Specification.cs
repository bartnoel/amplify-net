

namespace Amplify.ActiveRecord.Tests.ActiveRecord
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using MbUnit.Framework;

	[TestFixture, TestCategory("Functional")]
	public class DecoratedObject_Specification
	{

		[Test]
		public void Properties_Should_Call_GetSetProperty_Methods()
		{
			Person person = new Person();
			person.GetPropertyCalled.ShouldBe(false);
			person.SetPropertyCalled.ShouldBe(false);

			object value = person.FirstName;
			person.GetPropertyCalled.ShouldBe(true);
			value.ShouldBeNull();

			person.FirstName = "Michael";
			person.SetPropertyCalled.ShouldBe(true);
			value = person.FirstName;
			value.ShouldBe("Michael");
		}

		[Test]
		public void EachKey_Should_Iterate_Over_Keys()
		{
			Person person = new Person() { FirstName = "Michael", Age = 27 };
			int count = 0;
			bool firstNameFound = false;
			bool ageFound = false;

			person.EachKey(delegate(string key)
			{
				count++;
				if (key.Equals("FirstName"))
					firstNameFound = true;
				if (key.Equals("Age"))
					ageFound = true;
			});

			count.ShouldBe(2);
			firstNameFound.ShouldBe(true);
			ageFound.ShouldBe(true);
		}

		[Test]
		public void EachValue_Should_Iterate_Over_Values()
		{
			Person person = new Person() { FirstName = "Michael", Age = 27 }; 
			int count = 0;
			bool firstNameFound = false;
			bool ageFound = false;

			person.EachValue(delegate(object value)
			{
				count++;
				if (value.Equals("Michael"))
					firstNameFound = true;
				if (value.Equals(27))
					ageFound = true;
			});

			count.ShouldBe(2);
			firstNameFound.ShouldBe(true);
			ageFound.ShouldBe(true);
		}

		[Test]
		public void EachKeyPair_Should_Iterate_Over_KeyPair()
		{
			Person person = new Person { FirstName = "Michael", Age = 27 };
			int count = 0;
			bool firstNameFound = false;
			bool ageFound = false;

			person.Each(delegate(string key, object value)
			{
				count++;
				if (key.Equals("FirstName"))
				{
					person[key].ShouldBe("Michael");
					firstNameFound = true;
				}
				if (key.Equals("Age"))
				{
					person[key].ShouldBe(27);
					ageFound = true;
				}
			});

			count.ShouldBe(2);
			firstNameFound.ShouldBe(true);
			ageFound.ShouldBe(true);
		}


		#region Person
		public class Person : DecoratedObject
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

			public bool GetPropertyCalled { get; set; }

			public bool SetPropertyCalled { get; set; }

			protected override object GetProperty(string propertyName)
			{
				this.GetPropertyCalled = true;
				return base.GetProperty(propertyName);
			}

			protected override void SetProperty(string propertyName, object value)
			{
				this.SetPropertyCalled = true;
				base.SetProperty(propertyName, value);
			}
		#endregion

		}
	}
}
