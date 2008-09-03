

namespace Amplify.Data
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	
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



		public IDataReader Select(string storedproc, string[] parameters, object[] values)
		{
			Hash hash = new Hash();
			for(int i = 0; i < parameters.Length; i++){
				hash.Add(parameters[i], values[i]);
			}

			return ExecuteReader(storedproc, hash);

		}

		public IDataReader Select(string storedproc, IDictionary<string, object> values)
		{
			return ExecuteReader(storedproc, values);
		}

		public object Execute(string storedproc, string[] parameters, object[] values)
		{
			return Execute(storedproc, parameters, values);
		}

		public object Execute(string storedproc, string[] parameters, object[] values, string returnValue)
		{

			IDbTransaction tr = GetTransaction();
			IDbConnection connection = (tr == null) ? this.Connect() : tr.Connection;


			try
			{
				IDbCommand command = connection.CreateCommand();
				command.Transaction = tr;
				command.CommandType = CommandType.StoredProcedure;
				if (values != null)
				{
					int index = 0;

					foreach (object value in values)
					{
						IDataParameter param = command.CreateParameter();
						string name = string.Format("{0}{1}", this.ParameterPrefix, parameters[index]);
						param.ParameterName = name;
						param.Value = value;
						if (returnValue != null && returnValue.ToLower() == parameters[index].ToLower())
							param.Direction = ParameterDirection.InputOutput;
						values.SetValue(name, index);
						command.Parameters.Add(param);
						index++;
					}
					command.CommandText = string.Format(storedproc, values);
				}
				else
					command.CommandText = storedproc;

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
				if (tr == null)
				{
					connection.Close();
					connection.Dispose();
					connection = null;
				}
			}

		}

		


		#region Insert 
		

		public virtual int Update(Action<SaveOptions> action)
		{
			SaveOptions options = new SaveOptions();
			action(options);

			return this.Update(options);
		}

		public virtual int Update(SaveOptions options)
		{
			return (int)this.TransactionalQuery(options, UpdateType.Update);
		}

		public virtual object Insert(Action<SaveOptions> action)
		{
			SaveOptions options = new SaveOptions();
			action(options);

			return this.Insert(options);
		}

		public virtual object Insert(SaveOptions options)
		{
			return this.TransactionalQuery(options, UpdateType.Insert);
		}


		public virtual int Delete(Action<SaveOptions> action)
		{
			SaveOptions options = new SaveOptions();
			action(options);

			return this.Delete(options);
		}

		public virtual int Delete(SaveOptions options)
		{
			return (int)this.TransactionalQuery(options, UpdateType.Delete);
		}

		protected virtual object TransactionalQuery(SaveOptions options, UpdateType type)
		{
			lock (this)
			{
				if (options.ColumnNames.Count != options.ColumnValues.Count)
					throw new InvalidOperationException("the number of columns and the number of values must be equal");

				IDbTransaction tr = GetTransaction();
				IDbConnection connection = (tr == null) ? this.Connect() : tr.Connection;
				try
				{
					IDbCommand command = connection.CreateCommand();
					command.Transaction = tr;
					command.CommandText = AddParameters(options, command, type);

					Log.Sql(command.CommandText);

					if (type == UpdateType.Insert)
						return command.ExecuteScalar();
					else
						return command.ExecuteNonQuery();

				}
				catch
				{
					this.Rollback();
					throw;
				}
				finally
				{
					if (tr == null)
					{
						connection.Close();
						connection.Dispose();
						connection = null;
					}
				}
			}
		}

		

		protected enum UpdateType
		{
			Insert,
			Update,
			Delete
		}

		protected virtual string AddParameters(SaveOptions options, IDbCommand command, UpdateType type )
		{
			string sql = "", valueParams = "";
			bool isText = true;
			object key = null;


			if (options.StoredProcedureName.Length > 0)
			{
				command.CommandType = CommandType.StoredProcedure;
				sql = options.StoredProcedureName;
				isText = false;
			}
			else
			{
				command.CommandType = CommandType.Text;

				string start = "";

				switch(type) {
					case UpdateType.Insert:
						start = "INSERT INTO {0} (";
						break;
					case UpdateType.Update:
						start = "UPDATE {0} SET  ";
						break;

					case UpdateType.Delete:
						start = "DELETE FROM {0} ";
						break;
				}
				
				sql = string.Format(start, this.QuoteTableName(options.TableName));

				CreateWhereClauseFromPrimaryKey(options, type);
			}

			for (int i = 0; i < options.ColumnNames.Count; i++)
			{
				string name = options.ColumnNames[i];
				object value = options.ColumnValues[i];
				IDataParameter param = command.CreateParameter();
				param.Value = value;
				param.ParameterName = this.ParameterPrefix + name;
				command.Parameters.Add(param);
				if (isText)
				{
					switch (type)
					{
						case UpdateType.Insert:
							sql += string.Format("{0},", name);
							valueParams += string.Format("{0},", param.ParameterName);
							break;
						case UpdateType.Update:
							sql += string.Format("{0} = {1},", name, param.ParameterName);
							break;
					}
				}
			}

			if (isText)
			{
				char delimiter = Char.Parse(",");
				sql = sql.TrimEnd(delimiter) + "";

				if (type != UpdateType.Insert)
					sql += " WHERE " + options.Conditions[0].ToString();
				else
					sql += ") VALUES (" + valueParams.TrimEnd(delimiter) + ") " + this.SelectIdentity();
			}

			return sql;
		}


		protected virtual string SelectIdentity()
		{
			return " SELECT @@Identity";
		}

		private static object CreateWhereClauseFromPrimaryKey(SaveOptions options, UpdateType type)
		{
			if (type != UpdateType.Insert && options.Conditions.Count == 0)
			{
				if (string.IsNullOrEmpty(options.PrimaryKeyName))
					throw new Exception("the primary key must be specified on an update if there are no conditions set");

				int count = options.ColumnNames.Count, pkIndex = 0;
				for (int i = 0; i < count; i++)
				{
					if (options.ColumnNames[i].ToLower().Equals(options.PrimaryKeyName.ToLower()))
					{
						pkIndex = i;
						break;
					}
				}

				object value = options.ColumnValues[pkIndex];

				options.ColumnNames.RemoveAt(pkIndex);
				options.ColumnValues.RemoveAt(pkIndex);

				options.Where(string.Format(" {0} = '{1}' ", options.PrimaryKeyName, value));

				return value;
			}
			return null;
		}

		protected virtual string AddInsertParameters(SaveOptions options, IDbCommand command)
		{
			string valueString = "",
				   sql = "";
			bool isText = true;

			if (options.StoredProcedureName.Length > 0) {
				command.CommandType = CommandType.StoredProcedure;
				sql = options.StoredProcedureName;
				isText = false;
			} else {
				command.CommandType = CommandType.Text;
				
			}

			for (int i = 0; i < options.ColumnNames.Count; i++)
			{
				string name = options.ColumnNames[i];
				object value = options.ColumnValues[i];
				IDataParameter param = command.CreateParameter();
				if (i == 0)
					param.Direction = ParameterDirection.InputOutput;
				param.Value = value;
				param.ParameterName = this.ParameterPrefix + name;
				command.Parameters.Add(param);
				if (isText)
				{
					sql += name + ",";
					valueString += this.ParameterPrefix + name + ",";
				}
			}

			if(isText) 
			{
				char delimiter = Char.Parse(",");
				sql = sql.TrimEnd(delimiter) + ") VALUES (";
				sql += valueString.TrimEnd(delimiter) + "); ";

				if(options.RetrieveIdentity)
					sql += " SELECT @@IDENTITY AS 'Identity' ";
			}

			return sql;
		}

		
		#endregion

		#region Update


		

		

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
					foreach(object value in values)
					{
						IDataParameter param = command.CreateParameter();
						string name = string.Format("{0}Parameter{1}", this.ParameterPrefix, command.Parameters.Count);
						param.ParameterName = name;
						param.Value = value;
						values.SetValue(name, command.Parameters.Count);
						command.Parameters.Add(param);
					}
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
					foreach(object value in values)
					{
						IDataParameter param = command.CreateParameter();
						string name = string.Format("{0}Parameter{1}", this.ParameterPrefix, command.Parameters.Count);
						param.ParameterName = name;
						param.Value = value;
						values.SetValue(name, command.Parameters.Count);
						command.Parameters.Add(param);
					}
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
					foreach(object value in values) {
						IDataParameter param = command.CreateParameter();
						string name = string.Format("{0}Parameter{1}", this.ParameterPrefix, command.Parameters.Count);
						param.ParameterName = name;
						param.Value = value;
						values.SetValue(name, command.Parameters.Count);
						command.Parameters.Add(param);
					}
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
