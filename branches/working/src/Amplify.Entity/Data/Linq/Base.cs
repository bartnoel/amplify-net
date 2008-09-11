

namespace Amplify.Data.Linq
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Data.Linq;
	using System.Linq.Expressions;
	using System.Linq;
	using System.Text;

	using Amplify.Linq;

	public class Base<T> : Amplify.ActiveRecord.Base, Amplify.ObjectModel.IUniqueIdentifier  where T:Base<T>
	{
		private static DataContext s_dataContext;

		public static DataContext Db
		{
			get {
				if(s_dataContext == null)
					s_dataContext = Adapter.Get();
				return s_dataContext;
			}
		}

		protected static Table<T> Table
		{
			get { return Db.GetTable<T>(); }
		}

		public T New()
		{
			T item = Activator.CreateInstance<T>();
			item.IsNew = true;
			return item;
		}

		public T New(IDictionary<string, object> values)
		{
			T item = New();
			item.Send(values);
			return item;
		}

		public T New(params Func<object, object>[] values)
		{
			return New(Hash.New(values));
		}

		public T New(NameValueCollection nameValues)
		{
			T item = New();
			foreach(string key in nameValues.Keys)
				item[key] = nameValues[key];

			return item;
		}



		public static T Find(object id)
		{
			return (from o in Table where o.Id == id select o).SingleOrDefault();
		}

		public static  IQueryable<T> Find()
		{
			return (from o in Table select o);
		}

		public static  IEnumerable<T> Find(Func<T, bool> where)
		{
			return (from o in Table select o).Where(where);
		}

		public static IOrderedEnumerable<T> Find<TKey>(Func<T, bool> where, Func<T, TKey> orderBy, ListSortDirection direction)
		{
			if(direction == ListSortDirection.Ascending)
				return (from o in Table select o).Where(where).OrderBy(orderBy);
			return (from o in Table select o).Where(where).OrderByDescending(orderBy);
		}

		

		

		public static  IEnumerable<T> Find(Func<T, bool> where, DataContext context)
		{
			return (from o in context.GetTable<T>() select o).Where(where);
		}

		public static DataContext LoadWith(params Expression<Func<T, object>>[] includes)
		{
			DataContext db = new DataContext(Adapter.GetString(null, true));
			DataLoadOptions options = new DataLoadOptions();
			foreach(Expression<Func<T, object>> include in includes)
				options.LoadWith(include);
			db.LoadOptions = options;

			return db;
		}

		protected override void Set(string propertyName, object value, bool markChanged, bool checkType)
		{
			if (checkType)
			{
				if (markChanged)
					this.NotifyPropertyChanging(propertyName, value);
				
				Amplify.Linq.ExpressionVisitor.SetValue(this, propertyName, value);

				if (markChanged)
				{
					this.NotifyPropertyChanged(propertyName, value);
					this.IsModified = true;
				}
			}
			else
			{
				this.Set(propertyName, value, markChanged);
			}
		}

		public override object Save()
		{

			if (this.IsDeleted)
				Table.DeleteOnSubmit((T)this);

			base.Save();

			Db.SubmitChanges();
			return this;
		}

		protected override void Insert()
		{
			Table.InsertOnSubmit((T)this);
		}

		

		public override bool Delete()
		{
			Table.DeleteOnSubmit((T)this);
			Db.SubmitChanges();
			return true;
		}
	
		#region IUniqueIdentifier Members

		public abstract object  Id
		{
			get { return null;  }
		}

		#endregion
}
