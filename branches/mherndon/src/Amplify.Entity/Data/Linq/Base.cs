

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
	using System.Reflection;
	using System.Text;

	using Amplify.ComponentModel;
	using Amplify.Data.Validation;
	using Amplify.Linq;
	using Amplify.ActiveRecord;

	public abstract class Base<T> : 
		Amplify.ActiveRecord.Base,
		Amplify.ObjectModel.IUniqueIdentifier 
		where T : Base<T>
	{
		private DataContext dataContext;

		public DataContext Db
		{
			get
			{
				if (dataContext == null)
					dataContext = Adapter.Get();
				return dataContext;
			}
		}

		static Base()
		{
			InitOnce(typeof(T));
		}

		protected static void InitOnce(Type type)
		{
			ModelMetaInfo info = ModelMetaInfo.Get(type);

			List<PropertyInfo> properties = new List<PropertyInfo>();
			properties.AddRange(type.GetProperties());

			Type validationType = type.GetInterfaces()
				.Where(item => item.IsSubclassOf(typeof(IValidateObject)))
				.SingleOrDefault();

			if (validationType != null)
				properties.AddRange(validationType.GetProperties());

			foreach (PropertyInfo property in properties)
			{
				object[] attributes =	property
					.GetCustomAttributes(typeof(ValidationAttribute), true);

				foreach (ValidationAttribute attribute in attributes)
				{
					attribute.PropertyName = property.Name;
					info.Rules.Add(attribute.Rule);
				}


			}

			Base<T> entity = (Base<T>)Activator.CreateInstance<T>();
			entity.AddRules();
		}


		protected virtual void AddRules()
		{

		}

		protected static Table<T> Table
		{
			get { return Adapter.Get().GetTable<T>(); }
		}

		public T Create()
		{
			T item = Activator.CreateInstance<T>();
			item.IsNew = true;
			item.Save();
			return item;
		}

		public T Create(IDictionary<string, object> values)
		{
			T item = New();
			item.Send(values);
			item.Save();
			return item;
		}

		public T Create(params Func<object, object>[] values)
		{
			T item = New(Hash.New(values));
			item.Save();
			return item;
		}

		public T Create(NameValueCollection nameValues)
		{
			T item = New();
			foreach (string key in nameValues.Keys)
				item[key] = nameValues[key];

			item.Save();
			return item;
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
			foreach (string key in nameValues.Keys)
				item[key] = nameValues[key];

			return item;
		}



		public static T Find(object id)
		{
			return (from o in Table where o["Id"] == id select o).SingleOrDefault();
		}

		public static IQueryable<T> Find()
		{
			return (from o in Table select o);
		}

		public static IEnumerable<T> Find(Func<T, bool> where)
		{
			return (from o in Table select o).Where(where);
		}

		public static IOrderedEnumerable<T> Find<TKey>(Func<T, bool> where, Func<T, TKey> orderBy, ListSortDirection direction)
		{
			if (direction == ListSortDirection.Ascending)
				return (from o in Table select o).Where(where).OrderBy(orderBy);
			return (from o in Table select o).Where(where).OrderByDescending(orderBy);
		}

		public static IEnumerable<T> Find(Func<T, bool> where, DataContext context)
		{
			return (from o in Table select o).Where(where);
		}

		public static DataContext LoadWith(params Expression<Func<T, object>>[] includes)
		{
			DataContext db = new DataContext(Adapter.GetString(null, true));
			DataLoadOptions options = new DataLoadOptions();
			foreach (Expression<Func<T, object>> include in includes)
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

		

		#endregion

		#region IUniqueIdentifier Members

		public abstract object UniqueId { get; }

		#endregion
	}
}
