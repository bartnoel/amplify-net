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
