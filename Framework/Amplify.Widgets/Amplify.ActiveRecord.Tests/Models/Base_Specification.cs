

namespace Amplify.Models
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;
	using System.Text;

	using Amplify.Data;
	using Amplify.Data.SqlClient;

	using MbUnit.Framework;

	using Entities;

	[TestFixture]
	public class Base_Specification
	{
		private SqlAdapter adapter;

		[TestFixtureSetUp]
		public void Setup()
		{
			this.adapter = new SqlAdapter();
		}

		[Test]
		public void Expects_Static_New_Method_To_Create_New_Object()
		{
			Client client = Client.New();
			client.Name.ShouldBe("");
			client.Id.ShouldNotBeNull();
			client.CreatedOn.ShouldNotBeNull();
		}
		
		[Test]
		public void Expects_Base_To_Insert_New_Object_Into_Database()
		{
			int initial = 0;
			using (IDataReader dr = this.adapter.Select("SELECT Count(*) as Count FROM [Clients]"))
			{
				while (dr.Read())
				{
					initial = dr.GetInt32(0);
				}
			}


			Client client = Client.New();
			client.Name = "Booz Allen Hamilton";
			client.Save();
			

			client.Name.ShouldBe("Booz Allen Hamilton");
			client.IsNew.ShouldBe(false);

			int count = 0;
			using (IDataReader dr = this.adapter.Select("SELECT Count(*) as Count FROM [Clients]"))
			{
				while (dr.Read())
				{
					count = dr.GetInt32(0);
				}
			}
			count.ShouldBe(initial +1);
		}

		[Test]
		public void Expects_Base_To_Update_Object_Into_Database()
		{
			Client client = Client.Find(new Guid("29acff68-8166-4935-8790-54da7dccc665"));
			client.Id.ShouldBe(new Guid("29acff68-8166-4935-8790-54da7dccc665"));

			string value = client.Name;
			client.Name = client.Name + "a";
			client.Save();

			client = null;
			client = Client.Find(new Guid("29acff68-8166-4935-8790-54da7dccc665"));

			client.Name.ShouldBe(value + "a");
		}

		[Test]
		public void Expects_Base_To_Delete_Object()
		{
			Client client = Client.Find().Last();
			Guid id = client.Id;

			var x = Client.Find().SingleOrDefault(o => o.Id == id);

			x.ShouldNotBeNull();
			x.Id.ShouldBe(id);

			client.Delete();

			x = Client.Find().SingleOrDefault(o => o.Id == id);

			x.ShouldBeNull();
		}
	}
}
