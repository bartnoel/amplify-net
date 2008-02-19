

namespace Amplify.Linq
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using MbUnit.Framework;


	[TestFixture]
	public class Hash_Specification
	{


		[Test]
		public void Expects_Static_New_To_Have_Ruby_Like_Instantiation()
		{
			Hash h = Hash.New(Name => "Michael Herndon", Age => 10, Url => "www.myspace.com");
			h["Name"].ShouldBe("Michael Herndon");
			h["Age"].ShouldBe(10);
			h["Url"].ShouldBe("www.myspace.com");

			Hash t = new Hash(Year => 2006);

			t["Year"].ShouldBe(2006);
		}

		[Test]
		public void Expects_Static_New_To_Consume_IDictionary()
		{
			Dictionary<string, object> session = new Dictionary<string, object>();
			session["user"] = new Dictionary<string, object>() {  
				{"Name", "Michael"}, 
				{"Age", 28}
			};

			Hash h = Hash.New((IDictionary)session["user"]);

			h["Name"].ShouldBe("Michael");
			h["Age"].ShouldBe(28);
		}

		[Test]
		public void Expects_Item_Index_To_Return_Null_If_Value_Not_Present()
		{
			Hash h = Hash.New(Name => "Michael");
			h["Name"].ShouldNotBeNull();
			h["Age"].ShouldBeNull();
		}

		[Test]
		public void Expects_Constructor_To_Take_String_Object_Params()
		{
			Hash h = new Hash("Name", "Michael", "Age", 28);

			h["Name"].ShouldBe("Michael");
			h["Age"].ShouldBe(28);

		}

		[Test]
		public void Expects_To_Be_Able_To_Add_Range()
		{
			Hash h = Hash.New();
			h.AddRange(Name => "New Person", Age => 10);

			h["Name"].ShouldBe("New Person");
			h["Age"].ShouldBe(10);
			h["Age"] = 20;
			h["Age"].ShouldBe(20);
		}
	}
}
