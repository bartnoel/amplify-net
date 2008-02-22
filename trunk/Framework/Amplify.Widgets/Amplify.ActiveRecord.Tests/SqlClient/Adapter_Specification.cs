//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace Amplify.Data.SqlClient
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;
	using System.Text;

	using Amplify.Diagnostics;
	using Amplify.Linq;

	using MbUnit.Framework;

	[TestFixture, TestCategory("Functional", "Database")]
	public class Adapter_Specification
	{
		private Adapter adapter;

		[TestFixtureSetUp]
		public void Setup()
		{
			string connection = System.Configuration.ConfigurationManager.ConnectionStrings["test"].ConnectionString;
			this.adapter = new SqlClient.SqlAdapter(connection);
		}

		[Test(Description="Expects the adapter to be able to get the table names of the database")]
		public void Expects_To_Get_Table_Names()
		{
			int count = this.adapter.GetTableNames().Count;
			this.CreateTable();
			List<string> names = this.adapter.GetTableNames();
			
			names.Count.ShouldBe(count + 1);
			names.Contains("amptools_Test").ShouldBe(true);

			this.DropTable();
			this.adapter.GetTableNames().Count.ShouldBe(count);
		}



		private void CreateTable()
		{
			this.adapter.CreateTable("amptools_Test", Hash.New(Force => true), delegate(TableDefinition t)
			{
				t.Column("Name", Adapter.@string, Default => "", Limit => 50);
				t.Column("Description", Adapter.text);
				t.Column("IsBit", Adapter.boolean, Default => true);
				t.Column("Image", Adapter.binary);
				t.Column("TestId", Adapter.guid);
				t.Column("StartedDate", Adapter.datetime);
			});
		}

		private void DropTable()
		{
			this.adapter.DropTable("amptools_Test");
		}

		[Test(Description="Expects the adapter to be able to create a table")]
		public void Expects_To_Create_Table()
		{
			this.CreateTable();

			string query = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
			List<string> names = new List<string>();
			using (IDataReader dr = this.adapter.ExecuteReader(query))
			{
				while (dr.Read())
				{
					names.Add(dr.GetString(0));
				}
			}

			names.Contains("amptools_Test").ShouldBe(true);

			this.DropTable();
			names.Clear();
			using (IDataReader dr = this.adapter.ExecuteReader(query))
			{
				while(dr.Read())
				{
					names.Add(dr.GetString(0));
				}
			}
			names.Contains("amptools_Test").ShouldBe(false);
		}


		[Test(Description="Expects the adapater to get the columns of a table")]
		public void Expects_To_Get_Columns()
		{
			this.CreateTable();
			List<Column> columns = this.adapter.GetColumns("amptools_Test").ToList();
			columns.Count.ShouldBe(7);

			bool hasPrimaryKey = false;
			foreach (SqlColumn column in columns)
				if (column.IsPrimaryKey)
					hasPrimaryKey = true;

			hasPrimaryKey.ShouldBe(true);
			columns.Find(item => item.Name == "IsBit").ShouldNotBeNull();
			this.DropTable();

		}

		[Test(Description="Expects to get a single value ")]
		public void Expects_To_Excecute_Scalar()
		{
			this.adapter.ExecuteScalar("SELECT 20").ShouldBe(20);
		}

		[Test(Description="Expects the adapter to be able to add a column to the specified table")]
		public void Expects_To_Add_Column()
		{
			this.CreateTable();
			this.adapter.AddColumn("amptools_Test", "Add", Adapter.@string);
			this.adapter.GetColumns("amptools_Test").ToList().Find(item => item.Name == "Add").ShouldNotBeNull();
			this.DropTable();
		}

		[Test(Description="Expects the adapter to be able to change a column of a table")]
		public void Expects_To_Change_Column()
		{
			this.CreateTable();
			this.adapter.ChangeColumn("amptools_Test", "Name", Adapter.@string, Limit => 35, Default => "test");
			Column column = this.adapter.GetColumns("amptools_Test").Single(item => item.Name == "Name");
			column.ShouldNotBeNull();

			Log.Debug("Default for column is " + column.Default.ToString());
			Log.Debug("Limit for column is " + column.Limit.ToString());
			column.Default.ShouldBe("test");
			column.Limit.ShouldBe(35); // nvarchar = twice the actual length
			this.DropTable();
		}

		[Test(Description = "Expects the adapter to be able to remove a column")]
		public void Expects_To_Remove_Column()
		{
			this.CreateTable();
			this.adapter.RemoveColumn("amptools_Test", "Name");
			bool columnFound = false;
			this.adapter.GetColumns("amptools_Test").Each(delegate(Column item)
			{
				if (item.Name == "Name")
					columnFound = true;
			});

			columnFound.ShouldBe(false);
			this.DropTable();
		}

		[Test(Description = "Expects the adapter to be able to add an index to the table")]
		public void Expects_To_Add_And_Remove_Index()
		{   
			this.CreateTable();
			this.adapter.AddIndex("amptools_Test", new[] { "Name" });
			this.adapter.RemoveIndex("amptools_Test", new[] { "Name" });
			this.DropTable();
		}

		[Test(Description="Expects the adapster to quote values and prevent sql injection")]
		public void Expects_To_Quote_Values()
		{
			string test = "Michael'; DROP TABLE Bob ";
			this.adapter.Quote(test, null).ShouldBe("'Michael''; DROP TABLE Bob '");
		}
	}
}
