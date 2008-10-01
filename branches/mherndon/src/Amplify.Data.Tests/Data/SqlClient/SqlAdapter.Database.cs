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
		InContext(" the core database manipulation functionality like creating/dropping. "),
		Tag("Functional"),
		By("Michael Herndon", "mherndon@opensourceconnections.com", "www.amptools.net")
	]
	public class DatabaseSpecification : Spec
	{
		private string key = "mssql_creation";

		[It, Should(" be able to create a database automatically "
			+ " from the connection string. This is a core part to "
			+ " testing each database type. ")]
		public void AutoCreateDatabaseThenDrop()
		{
			string key = "mssql_creation",
					filename = "";

			Adapter.Get(key).CreateDatabase();

			SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(
				ConfigurationManager.ConnectionStrings[key + "_test"].ConnectionString);

			filename = builder.AttachDBFilename.ToLower().Replace("|datadirectory|", ApplicationContext.DataDirectory);
			File.Exists(filename).ShouldBeTrue();

			Adapter.Get(key).DropDatabase();
			File.Exists(filename).ShouldBeFalse();
		}

		[It, Should(" be able to recreate a database. ")]
		[DependsOn("AutoCreateDatabaseThenDrop")]
		public void RecreateDatabase()
		{
			Adapter adapter = Adapter.Get(key);
			Adapter master = Adapter.Add("master", "system.data.sqlclient", 
				@"Server=.\sqlexpress;Integrated Security=true;Database=master");

			adapter.CreateDatabase();
			master.GetDatabases().Contains(key).ShouldBeTrue();

			adapter.RecreateDatabase();
			master.GetDatabases().Contains(key).ShouldBeTrue();

			adapter.DropDatabase();
			master.GetDatabases().Contains(key).ShouldBeFalse();
		}

		[It, Should(" be able to create a table and drop the table. ")]
		[DependsOn("RecreateDatabase")]
		public void CreateDeleteTable()
		{
			Adapter adapter = Adapter.Get(key);
			adapter.CreateDatabase();
			try
			{
				string tableName = "test";

				adapter.CreateTable(tableName , null,(table) =>
				{
					table
						.AddColumn("name", DbTypes.String)
						.AddColumn("age", DbTypes.Integer, Hash.New(unique => true, isnull => false ));
				});

				adapter.GetTableNames().Contains(tableName).ShouldBeTrue();
				adapter.DropTable(tableName);
				adapter.GetTableNames().Contains(tableName).ShouldBeFalse();
			}
			finally
			{
				adapter.DropDatabase();
			}
		}

		[It, Should(" be able to add and remove indexes from the database. ")]
		[DependsOn("CreateDeleteTable")]
		public void AddRemoveIndexes()
		{
			Adapter adapter = Adapter.Get(key);
			adapter.CreateDatabase();
			try
			{
				string tableName = "Tests",
						columnName = "Name";


				adapter.CreateTable("Tests", table =>
				{
					table
						.AddColumn(columnName, DbTypes.String);
				});

				adapter.AddIndex(tableName, new[] {  columnName });
				List<IndexDefinition> list = adapter.GetIndexes(tableName);
				list.Count.ShouldBe(1);

				adapter.RemoveIndex(tableName, new[] { columnName });
				list = adapter.GetIndexes(tableName);
				list.Count.ShouldBe(0);
			}
			finally
			{
				adapter.DropDatabase();
			}

		}

		[It, Should(" be able to add and remove foreign keys from the database. ")]
		[DependsOn("CreateDeleteTable")]
		public void AddRemoveForeignKeys()
		{
			Adapter adapter = Adapter.Get(key);
			adapter.CreateDatabase();
			try
			{
				string tableName = "Tests1",
						referenceName = "Tests2",
						columnName = "tests1Id";

				adapter.CreateTable(tableName, null, t =>
				{
					
				});

				adapter.CreateTable(referenceName, null, t =>
				{
					t.AddColumn("tests1Id", DbTypes.Guid);
				});

				adapter.AddForeignKey(tableName, "Id", referenceName, columnName);

				adapter.GetForeignKeys(tableName).Count.ShouldBe(1);

				adapter.RemoveForeignKey(tableName, referenceName, columnName);

				adapter.GetForeignKeys(tableName).Count.ShouldBe(0);

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			finally
			{
				adapter.DropDatabase();
			}
		}




		#region HelperMethods

		private int Count(string tableName)
		{
			int count = 0;
			using (IDataReader dr = ExecuteReader("SELECT Count(*) FROM {0}".Fuse(tableName)))
			{
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
