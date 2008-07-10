

namespace Amplify.Data
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;
	using System.Text;
	using System.Reflection;

	using Amplify.Diagnostics;
	using Amplify.Linq;
	using System.Threading;


	public abstract partial class Adapter
	{
		public abstract IDbConnection Connect();

		private static Dictionary<Thread, System.Data.IDbTransaction> transactions;

		protected static Dictionary<Thread, System.Data.IDbTransaction> Transactions
		{
			get{
				
				if(transactions == null)
					transactions = new Dictionary<Thread,IDbTransaction>();
				return transactions;
				
			}
		}

		public virtual bool StartTransaction()
		{
			bool started = false;
			lock(Transactions) {
				started = !Transactions.ContainsKey(Thread.CurrentThread);
				if (started)
					Transactions.Add(Thread.CurrentThread, Connect().BeginTransaction());
			}
			return started;
		}

		public virtual IDbTransaction GetTransaction()
		{
			if (Transactions.ContainsKey(Thread.CurrentThread))
				return Transactions[Thread.CurrentThread];
			return null;
		}

		

		public virtual void Commit()
		{
			IDbTransaction tr =this.GetTransaction();
			if (tr != null)
			{ 
				lock(Transactions) {
					Transactions.Remove(Thread.CurrentThread);
				
					tr.Commit();
					if (tr.Connection != null)
					{
						tr.Connection.Close();
						tr.Connection.Dispose();
					}
					tr.Dispose();
				}
			}
		}

		public virtual void Rollback()
		{
		    IDbTransaction tr = this.GetTransaction(); 
			if (tr != null)
			{
				lock(Transactions) {
					Transactions.Remove(Thread.CurrentThread);
					tr.Rollback();
					IDbConnection connection = tr.Connection;
					tr.Dispose();
					if (connection != null)
					{
						connection.Close();
						connection.Dispose();
					}
				}
			}
		}

		protected virtual string ParameterPrefix { get { return "@"; } }



		public abstract IEnumerable<string> GetPrimaryKeys(string tableName);



		

		


		#region Insert 
		



		public virtual object Insert(string sql, params object[] values)
		{
			return this.ExecuteScalar(sql, values);
		}
		#endregion

		#region Update


		public virtual string GetUpdateSql(string tableName, string set, string primaryKey, object id) 
		{
			return "UPDATE {0} SET {1} WHERE {2} = {3}".Fuse(this.QuoteTableName(tableName), set, primaryKey, this.Quote(id));
		}

		public virtual string GetInsertSql(string tableName, string set, string values)
		{
			return "INSERT INTO {0} ({1}) VALUES ({2})".Fuse(this.QuoteTableName(tableName), set, values);
		}

		public virtual string GetDeleteSql(string tableName, string primaryKey, object id)
		{
			return "DELETE FROM {0} WHERE {1} = {2}".Fuse(this.QuoteTableName(tableName),
				this.QuoteColumnName(primaryKey), id);
		}

		public virtual int Update(string sql, params object[] values)
		{
			return this.ExecuteNonQuery(sql, values);
		}

		#endregion

		#region Delete
		

		public virtual int Delete(string sql, params object[] values)
		{
			return this.ExecuteNonQuery(sql, values);
		}

		
		#endregion


		public IDataReader Select(string sql, params object[] values)
		{
			return this.ExecuteReader(sql, values);
		}

		

		
		public int ExecuteNonQuery(string sql, params object[] values)
		{
			IDbTransaction tr = GetTransaction();
			IDbConnection connection = (tr == null) ? this.Connect() : tr.Connection;
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

				Log.Sql(command.CommandText);

				return command.ExecuteNonQuery();
			}
			catch
			{
				this.Rollback();
				throw;
			} 
			finally 
			{
				if(tr == null) 
				{
					connection.Close();
					connection.Dispose();
					connection = null;
				}
			}
			
			
		}

		public IDataReader ExexuteReader(string storedProceedure, System.Collections.IDictionary parameters)
		{
			IDbConnection connection = Connect();
			try
			{
				IDbCommand command = connection.CreateCommand();
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = storedProceedure;
				
				foreach (string key in parameters)
				{
					IDbDataParameter parameter = command.CreateParameter();
					parameter.ParameterName = key;
					parameter.Value = parameters[key];
					command.Parameters.Add(parameter);
				}
				Log.Sql(command.CommandText);

				return command.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch
			{
				connection.Close();
				throw;
			}
		}

		public IDataReader ExecuteReader(string sql, params object[] values)
		{
			IDbConnection connection = Connect();
			try
			{
				IDbCommand command = connection.CreateCommand();
				command.CommandType = CommandType.Text;

				if(values != null)
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

				Log.Sql(command.CommandText);

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
			IDbTransaction tr = this.GetTransaction();
			IDbConnection connection = (tr == null)? this.Connect() : tr.Connection;
		
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

				Log.Sql(command.CommandText);

				return command.ExecuteScalar();
			}
			catch
			{
				this.Rollback();
				throw;
			}
			finally 
			{
				if(tr == null) {
					connection.Close();
					connection.Dispose();
					connection = null;
				}
			}
			
		}
	}
}
