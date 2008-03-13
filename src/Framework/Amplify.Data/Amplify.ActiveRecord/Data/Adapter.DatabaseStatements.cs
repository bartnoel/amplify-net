

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
    using Amplify.Models;


	public abstract partial class Adapter
	{
		public abstract IDbConnection Connect();

		public virtual object BeginTransaction()
		{
			return 	Connect().BeginTransaction();
		}

		public virtual void Commit(object transaction)
		{
			if (transaction == null)
				return;
			if (transaction is IDbTransaction)
			{
				IDbTransaction tr = (IDbTransaction)transaction;
				tr.Commit();
				if (tr.Connection != null)
				{
					tr.Connection.Close();
					tr.Connection.Dispose();
				}
				tr.Dispose();
			}
		}

		public virtual void Rollback(object transaction)
		{
			if (transaction == null)
				return;
			if (transaction is IDbTransaction)
			{
				IDbTransaction tr = (IDbTransaction)transaction;
				try
				{
					tr.Rollback();
					if (tr.Connection != null)
					{
						tr.Connection.Close();
						tr.Connection.Dispose();
					}
					tr.Dispose();
				}
				catch
				{

				}
			}
		}

		protected virtual string ParameterPrefix { get { return "@"; } }

		public virtual IEnumerable<object> Select(string sql, bool isReadOnly, Type  type, params object[] values)
		{
			List<object> list = new List<object>();
			using (IDataReader dr = Select(sql, values))
			{
				while (dr.Read())
				{
					object item = Activator.CreateInstance(type);
					bool isDecorated = (item is IDecoratedObject);
					for (int i = 0; i < dr.FieldCount; i++)
					{
						object value = dr.GetValue(i);
						if (value == DBNull.Value)
							value = null;

						if(isDecorated) 
							item[dr.GetName(i)] = value;
						else {
							string propertyName = dr.GetName(i);
							PropertyInfo info = item.GetType().GetProperty(propertyName);
							if (info != null)
								info.SetValue(item, value);
						}
					}
					if(item is IExtendedUnitOfWork)
					{
						IExtendedUnitOfWork unit = (IExtendedUnitOfWork)item;
						unit.MarkOld();
						if (isReadOnly)
							unit.MarkReadOnly();
					}
					list.Add(item);
				}
			}
			return list;

		}

		public virtual IEnumerable<T> Select<T>(string sql, params object[] values) where T : IRelational, IExtendedUnitOfWork, new()
		{
			return Select<T>(sql, false, values);

		}

		



		public virtual IEnumerable<T> Select<T>(string sql, bool isReadOnly, params object[] values) where T : IRelational, IExtendedUnitOfWork, new()
		{
			List<T> list = new List<T>();
			using (IDataReader dr = Select(sql, values))
			{
				while (dr.Read())
				{
					T item = new T();
					if (isReadOnly)
						item.MarkReadOnly();
					for (int i = 0; i < dr.FieldCount; i++)
					{
						object value = dr.GetValue(i);
						if (value == DBNull.Value)
							value = null;

						item[dr.GetName(i)] = value;
					}
					item.MarkOld();
					list.Add(item);
				}
			}
			return list;
		}

		


		#region Insert 
		public virtual object Insert<T>(T obj, object transaction)where T: IRelational
        {
			ITableEntityDescriptor primary = obj.Tables.SingleOrDefault(item => item.IsPrimary == true);
			object id = null;
			string columns = "";
			string values = "";

			IColumnDescriptor primaryKey = primary.PrimaryKeys.First();
			

			primary.Columns.Each(delegate(IColumnDescriptor item)
			{
				columns += this.QuoteColumnName(item.Name) + ",";
				values += this.Quote(obj[item.PropertyName]) + ",";
			});

			id = Insert("INSERT INTO {0} ({1}) VALUES ({2})".Inject(
					this.QuoteTableName(primary.Name),
					columns.TrimEnd(",".ToCharArray()),
					values.TrimEnd(",".ToCharArray())
				), transaction);

			if (obj[primaryKey.PropertyName] is int && id is int)
				obj[primaryKey.PropertyName] = id;
			else
				id = obj[primaryKey.PropertyName];
			

			if (obj.Tables.Count() > 1)
			{
				obj.Tables.Each(delegate(ITableEntityDescriptor table)
				{
					if (table == primary || !table.AllowInserts || table.IsReadOnly)
						return;

					columns = "";
					values = "";

					table.Columns.Each(delegate(IColumnDescriptor item)
					{
						if (item.Name.ToLower() == primaryKey.Name.ToLower())
							return;
						columns += this.QuoteColumnName(item.Name) + ",";
						values += this.Quote(obj[item.PropertyName]) + ",";	
					});
					columns += primaryKey.Name;
					values += obj[primaryKey.PropertyName];

					this.Insert("INSERT INTO {0} ({1}) VALUES ({0})".Inject(
							this.QuoteTableName(primary.Name),
							columns,
							values
						), transaction);
				});
			}
			return id;		
		}

		
		

		public virtual object Insert(string sql, object transaction, params object[] values)
		{
			return this.ExecuteScalar(sql, (IDbTransaction)transaction, values);
		}

		public virtual object Insert(string sql, params object[] values)
		{
			return this.ExecuteScalar(sql, values);
		}
		#endregion

		#region Update
		public virtual int Update<T>(T obj, object transaction) where T:IRelational 
        {
			int count = 0;
			IColumnDescriptor primaryKey = obj.Tables.SingleOrDefault(o => o.IsPrimary).PrimaryKeys.First();
            
			foreach (TableAttribute table in obj.Tables)
            {
				if (!table.AllowUpdates || table.IsReadOnly)
					continue;

				List<string> set = new List<string>();

		
                table.Columns.Each(delegate(IColumnDescriptor item)
                {
                    if (item.Name.ToLower() != primaryKey.Name.ToLower())
                    {
                        set.Add("{0} = {1}".Inject(
							this.QuoteColumnName(item.Name),
                            this.Quote(obj[item.PropertyName])
                        ));
                    }
                });

				count = count + Update("UPDATE {0} SET {1} WHERE {2} = {3}".Inject(
						this.QuoteTableName(table.Name),
						set.Join(","),
						this.QuoteColumnName(primaryKey.Name),
						this.Quote(obj[primaryKey.PropertyName])
				), transaction);
            }
			return count;
        }

		public virtual int Update(string sql, params object[] values)
		{
			return this.ExecuteNonQuery(sql, values);
		}

		public virtual int Update(string sql, object transaction, params object[] values)
		{
			return this.ExecuteNonQuery(sql, (IDbTransaction) transaction, values);
		}
		#endregion

		#region Delete
		public virtual int Delete<T>(T obj, object transaction) where T: IRelational 
		{
			// if there is more than one table that makes up this object.
			int count = 0;
			string primaryKey = obj.Tables.SingleOrDefault(o => o.IsPrimary).PrimaryKeys.First().Name;
			obj.Tables.Each(delegate(ITableEntityDescriptor table)
			{
				if (table.IsReadOnly || !table.AllowDeletes)
					return;

				Delete("DELETE FROM {0} WHERE {1} = {2}".Inject(
					this.QuoteTableName(table.Name),
					this.QuoteColumnName(primaryKey),
					this.Quote(obj[primaryKey])
				), transaction);

				count++;
			});
			return count;
		}

		public virtual int Delete(string sql, params object[] values)
		{
			return this.ExecuteNonQuery(sql, values);
		}

		public virtual int Delete(string sql, object transaction, params object[] values)
		{
			return this.ExecuteNonQuery(sql, (IDbTransaction)transaction, values);
		}
		#endregion


		public IDataReader Select(string sql, params object[] values)
		{
			return this.ExecuteReader(sql, values);
		}

		public int ExecuteNonQuery(string sql, IDbTransaction transaction, params object[] values)
		{
			if(transaction == null)
				return this.ExecuteNonQuery(sql, values);

			try
			{
				IDbCommand command = transaction.Connection.CreateCommand();
				command.Transaction = transaction;
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
				transaction.Rollback();
				if (transaction.Connection != null)
				{
					transaction.Connection.Close();
					transaction.Connection.Dispose();
				}
				transaction.Dispose();
				transaction = null;
				throw;
			}
		}

		public object ExecuteScalar(string sql, IDbTransaction transaction, params object[] values)
		{
			if (transaction == null)
				return this.ExecuteScalar(sql, values);

			try
			{
				IDbCommand command = transaction.Connection.CreateCommand();
				command.Transaction = transaction;
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

				return command.ExecuteScalar();
			}
			catch
			{
				transaction.Rollback();
				if (transaction.Connection != null)
				{
					transaction.Connection.Close();
					transaction.Connection.Dispose();
				}
				transaction.Dispose();
				transaction = null;
				throw;
			}
		}

		public int ExecuteNonQuery(string sql, params object[] values)
		{
			int temp = 0;
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

					Log.Sql(command.CommandText);

					temp = command.ExecuteNonQuery();
					tr.Commit();
				}
				catch
				{
					tr.Rollback();
					throw;
				}
				finally
				{
					tr.Dispose();
					tr = null;
				}
				return temp;
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


				

				//Log.Debug("length {0}".Inject(items.Length));
			

				if(values != null)
				{
					values.Each(delegate(object value)
					{
						IDataParameter param = command.CreateParameter();
						//Log.Debug("value {0}".Inject(value));
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

					Log.Sql(command.CommandText);

					object o = command.ExecuteScalar();
					tr.Commit();
					return o;
				}
				catch
				{
					tr.Rollback();
					tr.Dispose();
					tr = null;
					throw;
				}
			}
		}
	}
}
