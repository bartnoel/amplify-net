//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) CompanyName.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Data.SqlClient
{
	#region Using Statements
	using System;
	using System.Collections.Generic;
	using System.Deployment.Application;
	using System.Configuration;
	using System.Data;
	using System.Data.SqlClient;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using System.Text;

	using Amplify.Linq;

	using MbUnit.Framework;

	using Describe = MbUnit.Framework.TestsOnAttribute;
	using InContext = MbUnit.Framework.DescriptionAttribute;
	using It = MbUnit.Framework.TestAttribute;
	using Should = MbUnit.Framework.DescriptionAttribute;
	using By = MbUnit.Framework.AuthorAttribute;
	using Tag = MbUnit.Framework.CategoryAttribute;
	#endregion

	[
		Describe(typeof(SqlAdapter)),
		InContext("should perform its specified behavor."),
		Tag("Functional"), 
		By("Michael Herndon", "mherndon@opensourceconnections.com", "www.amptools.net")
	]
	public class SqlAdapterObject : Spec
	{
		private string key = "mssql_creation";

		public override void InvokeBeforeAll()
		{
			Adapter adapter = Adapter.Get(key);
			adapter.CreateDatabase();


			adapter.CreateTable(t =>
			{
				t.SetName("test")
				 .AddColumn("Name", DbTypes.String)
				 .AddColumn("Age",  DbTypes.Integer);
			});
		}

		public override void InvokeAfterAll()
		{
			base.InvokeAfterAll();
			Adapter adapter = Adapter.Get(key);
			adapter.DropDatabase();
		}

		[It, Should(" make an insert into the database ")]
		public void Insert()
		{
			Adapter adapter = Adapter.Get(key);
			adapter.Insert(insert => insert.Into("test").Columns("Id,Name,Age").Values(3,"Michael", 10));

			string query = "SELECT COUNT(*) FROM test";
			int count = 0;

			using (IDataReader dr = adapter.ExecuteReader(query))
			{
				while(dr.Read())
					count = dr.GetInt32(0);
			}
			count.ShouldBe(1);

			adapter.ExecuteNonQuery("DELETE FROM test");
		}

		[It, Should(" delete a record to the database."), DependsOn("Insert")]
		public void Delete()
		{
			Adapter adapter = Adapter.Get(key);
			object id = adapter.Insert(insert => insert.Into("test").Columns("Id,Name,Age").Values(3, "Michael", 10));

			string query = "SELECT COUNT(*) FROM test";
			int count = 0;

			using (IDataReader dr = adapter.ExecuteReader(query))
			{
				while (dr.Read())
					count = dr.GetInt32(0);
			}

			count.ShouldBe(1);

			int affected = adapter.Delete(q => q.From("test").Where(" Id = '3' "));

			using (IDataReader dr = adapter.ExecuteReader(query))
			{
				while (dr.Read())
					count = dr.GetInt32(0);
			}
			count.ShouldBe(0);
		}

		[It, Should(" update a record in the database."), DependsOn("Insert")]
		public void Update()
		{
			Adapter adapter = Adapter.Get(key);
			
			adapter.Insert(insert => 
				insert.Into("test")
				.Columns("Id,Name,Age")
				.Values(3, "Michael", 10)
			);

			int age = 0;
			string query = "SELECT Age FROM test WHERE Id = 3";

			using (IDataReader dr = adapter.ExecuteReader(query))
			{
				while (dr.Read())
					age = dr.GetInt32(0);
			}

			age.ShouldBe(10);

			adapter.Update(update => 
				update.From("test")
				.Columns("Id,Age")
				.Values(3, 20)
				.PrimaryKey("Id")
			);

			using (IDataReader dr = adapter.ExecuteReader(query))
			{
				while (dr.Read())
					age = dr.GetInt32(0);
			}
			age.ShouldBe(20);
			
		}
		

		
		#region HelperMethods

		private int Count(string tableName)
		{
			int count = 0;
			using(IDataReader dr = ExecuteReader("SELECT Count(*) FROM {0}".Fuse(tableName))) {
				while (dr.Read())
				{
					count = (int)dr[0];
				}
			}
			return count;
		}

		private IDataReader ExecuteReader(string text, params object[] values)
		{
			IDbConnection connection = null;
			try
			{
				connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["test"].ConnectionString);
				connection.Open();
				IDbCommand command = connection.CreateCommand();
				command.CommandType = CommandType.Text;
				command.CommandText = text;
				if (values != null && values.Length > 0)
				{
					int count = 0;
					foreach (object value in values)
					{
						IDbDataParameter param = command.CreateParameter();
						param.ParameterName = "@Parameter" + count.ToString();
						param.Value = value;
						command.Parameters.Add(param);
					}
				}
				return command.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch
			{
				if (connection != null)
				{
					connection.Close();
					connection.Dispose();
				}
				throw;
			}
		}

		private int ExecuteNonQuery(string text, params object[] values)
		{

			using (IDbConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["test"].ConnectionString))
			{
				connection.Open();
				IDbCommand command = connection.CreateCommand();
				command.CommandType = CommandType.Text;
				command.CommandText = text;
				if (values != null && values.Length > 0)
				{
					int count = 0;
					foreach (object value in values)
					{
						IDbDataParameter param = command.CreateParameter();
						param.ParameterName = "@Parameter" + count.ToString();
						param.Value = value;
						command.Parameters.Add(param);
					}
				}
				return command.ExecuteNonQuery();
			}
		}


		#endregion
	}
}
