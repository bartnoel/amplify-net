using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.ActiveRecord.Data
{
	public abstract class AdapterBase<T>
		where T: Base<T>
	{

		internal protected abstract int Count();

		internal protected abstract int Count(IOptions Options);

		internal protected virtual bool IsLinq { get { return false;} }

		internal protected abstract IEnumerable<T> ExecuteQuery(string sql, params object[] values);

		internal protected abstract IEnumerable<T> Select(IOptions options);

		internal protected abstract IEnumerable<T> SelectAll(IOptions options);

		internal protected abstract IEnumerable<T> SelectAll();

		internal protected abstract T SelectOne(IOptions options);

		internal protected abstract T Select(object id);

		internal protected abstract void Update(T item);

		internal protected abstract void Insert(T item);

		internal protected abstract void Delete(T item);

		internal protected abstract void SaveList(IEnumerable<T> items, IEnumerable<T> deleteItems, System.Data.Common.DbTransaction transaction);

		internal protected abstract void SaveList(IEnumerable<T> items, IEnumerable<T> deleteItems);

		
	}
}
