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
	using System.Data;
	using System.Data.SqlClient;
	using System.Linq;
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
		By("Michael Herndon", "mherndon@opensourceconnections.com", "www.amptools.net")
	]
	public class SqlAdapterObject : Spec
	{

		

		public override void InvokeAfterEach()
		{
			this.ExecuteNonQuery("DELETE FROM TestList");
		}

		[It, Should(" have a public default constructor. ")]
		public void InvokeConstructor()
		{
			SqlAdapter obj = new SqlAdapter();
			obj.ShouldNotBeNull();
		}

		[
			It, Should(" execute an insert statement using the Insert method. "),
			Row("Test 1st Insert"),
			Row("Test 2nd Insert")
		]
		public void ExecuteInsert(string name)
		{

			Guid id = Guid.NewGuid();
			DataSpec.Adapter.Insert("INSERT INTO TestList (Id,Name) VALUES({0},{1})", id, name);

			using (IDataReader dr = ExecuteReader("SELECT * FROM TestList"))
			{
				int count = 0;
				while (dr.Read())
				{
					dr.FieldCount.ShouldBe(2, "If there are more than columns for table TestList, tests need to modified");
					dr["Id"].ShouldBe(id);
					dr["Name"].ShouldBe(name);
					count++;
				}
				count.ShouldBe(1, 
					"The IDataReader should have found only one row, instead it found {0}".Fuse(count));
			}
		}


		[
			It, Should(" execute a delete statement using the Delete method. "),
			Tag("Database"),
			DependsOn("ExecuteInsert"),
			Row("First Test"),
			Row("Second Test")
		]
		public void ExecuteDelete(string name)
		{
			Guid id = Guid.NewGuid();
			DataSpec.Adapter.Insert("INSERT INTO TestList (Id,Name) VALUES({0},{1})", id, name);
			Count("TestList").ShouldBe(1);

			DataSpec.Adapter.Delete("DELETE FROM TestList");

			Count("TestList").ShouldBe(0);
		}


		[
			It, Should(" execute a select statement using the Select method. "),
			Tag("Database"),
			DependsOn("ExecuteInsert"),
			Row("First Test"),
			Row("Second Test")
		]
		public void ExecuteSelect(string name)
		{
			Guid id = Guid.NewGuid();
			DataSpec.Adapter.Insert("INSERT INTO TestList (Id,Name) VALUES({0},{1})", id, name);
			using (IDataReader dr = DataSpec.Adapter.Select("SELECT * FROM TestList"))
			{
				int count = 0;
				while (dr.Read())
				{
					dr["Id"].ShouldBe(id);
					dr["Name"].ShouldBe(name);
					count++;
				}
				count.ShouldBe(1);
			}
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
