

namespace Amplify.ActiveRecord.Data
{
	using System;
	using System.Collections.Generic;
	using System.Data.Linq;
	using System.Linq.Dynamic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Text;

	using Amplify.Reflection;

	public class LinqAdapter<T> : AdapterBase<T>
		where T: Base<T>
	{
		protected DataContext Context { get; set; }
		protected Table<T> Table { get { return this.Context.GetTable<T>(); } }

		public LinqAdapter(DataContext datacontext)
		{
			this.Context = datacontext;
		}



		internal protected override IEnumerable<T> ExecuteQuery(string sql, params object[] values)
		{
			return this.Context.ExecuteQuery<T>(sql, values);
		}

		internal protected override T Select(object id)
		{
			return this.Context.GetTable<T>().Where(t => t[t.PrimaryKeys.First()] == id).Take(1).First();
		}

		

		internal protected override int Count()
		{
			return this.SelectAll().Count();
		}

		internal protected override int Count(IOptions options)
		{
			return this.SelectAll(options).Count();
		}

		internal protected override IEnumerable<T> Select(IOptions options)
		{
			var t = (from o in this.Table
					 select o);

			if (options.IsDistinct)
				t = t.Distinct();

			if (options.Conditions != null)
				t = t.Where(options.Conditions.Expression, options.Conditions.Values);

			if (options.OrderBy != null)
				t = t.OrderBy(options.OrderBy.Expression, options.OrderBy.Values);

			
			if (options.GroupBy != null)
				t = t.GroupBy(options.GroupBy.Expression, options.GroupBy.ElementSelector, options.GroupBy.Values);

			if (options.Limit > 0)
				t = t.Take(options.Limit);

			if (options.Offset > 0)
				t = t.Skip(options.Offset);

			return (IEnumerable<T>)t;
		}

		internal protected override IEnumerable<T> SelectAll(IOptions options)
		{
			return Select(options);
		}

		internal protected override IEnumerable<T> SelectAll()
		{
			return (from t in this.Table select t);
		}

		internal protected override T SelectOne(IOptions options)
		{
			return Select(options).First();
		}

		

		internal protected override void Delete(T item)
		{
			this.Table.DeleteOnSubmit(item);
			this.Table.Context.SubmitChanges();
		}

		internal protected override void Insert(T item)
		{
			this.Table.InsertOnSubmit(item);
			this.Table.Context.SubmitChanges();
		}

		internal protected override void Update(T item)
		{
			this.Table.InsertOnSubmit(item);
			this.Table.Context.SubmitChanges();
		}



		internal protected override void SaveList(IEnumerable<T> items, IEnumerable<T> deleteItems, System.Data.Common.DbTransaction transaction)
		{
			try
			{
				if (transaction != null)
					this.Context.Transaction = transaction;

				foreach (T item in items)
					if (item.IsNew)
						this.Table.InsertOnSubmit(item);
				this.Table.DeleteAllOnSubmit(deleteItems);

				this.Context.SubmitChanges();

				if (transaction != null)
					transaction.Commit();
			}
			catch
			{
				if(transaction != null)
					transaction.Rollback();
			}
			finally
			{
				if(transaction != null)
					transaction.Dispose();
			}

		}

		internal protected override void SaveList(IEnumerable<T> items, IEnumerable<T> deleteItems)
		{
			this.SaveList(items, deleteItems, this.Context.Connection.BeginTransaction());
		}

		

	}
}
