

namespace Amplify.Data
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;
	using System.Text;

	using Amplify.Linq;

	public abstract partial class Adapter
	{
		protected abstract IDbConnection Connect();

		protected virtual string ParameterPrefix { get { return "@"; } }

		public IDataReader Select(string sql, params object[] values)
		{
			return this.ExecuteReader(sql, values);
		}

		public int ExecuteNonQuery(string sql, params object[] values)
		{
			using (IDbConnection connection = Connect())
			{
				IDbTransaction tr = connection.BeginTransaction();
				try
				{
					IDbCommand command = connection.CreateCommand();
					command.Transaction = tr;
					command.CommandType = CommandType.Text;
					if (values != null)
					{
						values.Each(delegate(object value)
						{
							IDataParameter param = command.CreateParameter();
							string name = string.Format("{0}Parameter{1}", this.ParameterPrefix, command.Parameters.Count);
							param.ParameterName = name;
							param.Value = value;
							values.SetValue(name, command.Parameters.Count);
							command.Parameters.Add(param);
						});
						command.CommandText = string.Format(sql, values);
					}
					else
						command.CommandText = sql;

					Console.WriteLine(command.CommandText);

					command.ExecuteNonQuery();
					tr.Commit();
				}
				catch
				{
					tr.Rollback();
					throw;
				}
				return 0;
			}
		}

		public IDataReader ExecuteReader(string sql, params object[] values)
		{
			IDbConnection connection = Connect();
			try
			{
				IDbCommand command = connection.CreateCommand();
				command.CommandType = CommandType.Text;
				if (values != null)
				{
					values.Each(delegate(object value)
					{
						IDataParameter param = command.CreateParameter();
						string name = string.Format("{0}Parameter{1}", this.ParameterPrefix, command.Parameters.Count);
						param.ParameterName = name;
						param.Value = value;
						values.SetValue(name, command.Parameters.Count);
						command.Parameters.Add(param);
					});
					command.CommandText = string.Format(sql, values);
				}
				else
					command.CommandText = sql;

				Console.WriteLine(command.CommandText);

				return command.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch
			{
				connection.Close();
				throw;
			}
		}

		public object ExecuteScalar(string sql, params object[] values)
		{
			using (IDbConnection connection = Connect())
			{
				IDbTransaction tr = connection.BeginTransaction();
				try
				{
					IDbCommand command = connection.CreateCommand();
					command.Transaction = tr;
					command.CommandType = CommandType.Text;
					if (values != null) 
					{
						values.Each(delegate(object value) {
							IDataParameter param = command.CreateParameter();
							string name = string.Format("{0}Parameter{1}", this.ParameterPrefix, command.Parameters.Count);
							param.ParameterName = name;
							param.Value = value;
							values.SetValue(name, command.Parameters.Count);
							command.Parameters.Add(param);
						});
						command.CommandText = string.Format(sql, values);
						
					}else
						command.CommandText = sql;

					Console.WriteLine(command.CommandText);

					object o = command.ExecuteScalar();
					tr.Commit();
					return o;
				}
				catch
				{
					tr.Rollback();
					throw;
				}
			}
		}
	}
}
