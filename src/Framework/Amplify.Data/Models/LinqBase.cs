using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;

using Amplify.Linq;

namespace Amplify.Models
{
	public class LinqBase<T> : Base<T>, IDataContextSave where T: LinqBase<T>
	{
		private static Hash associations;
		private static Hash columns;
		private bool isNew = false;
		private bool isModified = false;
		private bool isMarkedForDeletion = false;


		public LinqBase()
			: base()
		{
		}

		protected virtual bool IsNew
		{
			get { return this.isNew; }
		}

		protected virtual bool IsModified
		{
			get { return this.isModified; }
		}

		internal protected virtual bool IsMarkedForDeletion
		{
			get { return this.isMarkedForDeletion; }
		}

		internal protected bool CascadeDelete { get; set; }

		protected virtual void MarkForDeletion()
		{
			this.isMarkedForDeletion = true;
		}

		protected void MarkOld()
		{
			this.isNew = false;
			this.isModified = false;
		}

		protected override void OnPropertyChanged(string propertyName)
		{
			base.OnPropertyChanged(propertyName);
			this.isModified = true;
		}

		protected override void InitializeValues()
		{
			base.InitializeValues();
			this.OnCreate();
		}

		public static IEnumerable<T> Find(Expression<Func<T, bool>> conditionals)
		{
			using (DataContext db = CreateContext())
			{
				return db.GetTable<T>().Where(conditionals);
			}
		}

		public static IEnumerable<T> Find(DataContext db, Expression<Func<T, bool>> conditionals)
		{
			return db.GetTable<T>().Where(conditionals);
		}


		public static IEnumerable<T> Find(Action<IQueryable<T>> query, params Expression<Func<T, object>>[] includes)
		{
			using (DataContext db = CreateContext())
			{
				return Find(db, query, includes);
			}
		}

		public static IQueryable FindView(DataContext db, Action<IQueryable<T>> query)
		{
			IQueryable<T> table = db.GetTable<T>();
			query(table);
			return table;
		}

		public static IEnumerable<TSource> FindView<TSource>(DataContext db, Action<IQueryable<T>> query) where TSource: new()
		{
			return Find(db, query).Select(o => new TSource());
		}

		public static IEnumerable<T> Find(DataContext db, Action<IQueryable<T>> query, params Expression<Func<T, object>>[] includes)
		{
			if (includes != null)
			{
				DataLoadOptions options = new DataLoadOptions();
				foreach (var include in includes)
					options.LoadWith<T>(include);
				db.LoadOptions = options;
			}
			IQueryable<T> table = db.GetTable<T>();
			query(table);
			return table;
		}


		public static T New()
		{
			T item = New();
			item.isNew = true;
			return item;
		}
		
		public static T New(IDictionary values)
		{
			T item = New();
			item.Merge(values);
			return item;
		}

		public static T New(params Func<object, object>[] values)
		{
			T item = New();
			item.Merge(Hash.New(values));
			return item;
		}

		public static T New(object values)
		{
			T item = New();
			if (values != null)
			{
				PropertyInfo[] properties =	values.GetType().GetProperties();
				foreach (PropertyInfo property in properties)
					item[property.Name] = property.GetValue(values, null);
			}
			return item;
		}

		public static T Create()
		{
			return New().Save();
		}

		public static T Create(IDictionary<string, object>[] values)
		{
			return New(values).Save();
		}

		public static T Create(params Func<object, object>[] values)
		{
			return New(values).Save();
		}

		public static T Create(object values)
		{
			return New(values).Save();
		}


		protected virtual void OnCreate()
		{

		}

		protected internal override void GetPropertyAttributes(PropertyInfo property)
		{
			base.GetPropertyAttributes(property);
			associations = Hash.New();
			columns = Hash.New();

			property.GetCustomAttributes(typeof(AssociationAttribute), true).
				Cast<AssociationAttribute>().Each(delegate(AssociationAttribute association) {
					associations.Add(property.Name, association);
				});

			property.GetCustomAttributes(typeof(ColumnAttribute), true).Cast<ColumnAttribute>()
				.Each(delegate(ColumnAttribute column)
			{
				columns.Add(property.Name, column);
			});
		}

		protected override void Validate()
		{
			columns.Each(o => this.Validate(o.Key, this[o.Key])); 
		}

		public  T Save()
		{
			using (DataContext context = CreateContext())
			{
				this.Save(context, true);
				context.SubmitChanges();
			}
			return (T)this;
		}

		public T Save(DataContext context)
		{
			T item = Save(context, false);
			context.SubmitChanges();
			return item;
		}

		public T Save(DataContext context, bool attach) 
		{
			if (!this.IsValid)
				throw new Exception("The model {0} must be valide before saving".Inject(this.GetType().Name));
			if (this.IsMarkedForDeletion)
				this.Delete(context);
			else 
			{
				if (this.IsNew)
					this.Insert(context);
				else
					this.Update(context, attach);
			}
			this.SaveChildren(context, attach);
			this.MarkOld();
			return (T)this;
		}

		public void Delete()
		{
			using (DataContext db = CreateContext())
			{
				this.MarkForDeletion();
				this.Save(db, true);
			}
		}

		public virtual void Delete(DataContext context)
		{
			context.GetTable<T>().InsertOnSubmit((T)this);
		}

		protected virtual void Insert(DataContext context)
		{
			context.GetTable<T>().InsertOnSubmit((T)this);
		}

		protected virtual void Update(DataContext context, bool attach)
		{
			if(attach && this.IsModified)
				context.GetTable<T>().Attach((T)this, true);
		}

		protected virtual void SaveChildren(DataContext context, bool attach)
		{
			if (this.IsMarkedForDeletion && this.CascadeDelete)
			{
				this.EachValue(delegate(object value)
				{
					if (value is IList)
					{	
						foreach (IDataContextSave item in (IList)value)
						{
							item.MarkForDeletion();
							item.Save(context, attach);
						}
					}
					else if (value is IDataContextSave)
					{
						IDataContextSave item = (IDataContextSave)value;
						item.MarkForDeletion();
						item.Save(context, attach);
					}
				});	
			}
			else
			{
				this.EachValue(delegate(object value)
				{
					if (value is IDataContextSave)
						((IDataContextSave)value).Save(context, attach);
					else if (value is IList)
						foreach (object item in (IList)value)
							if (item is IDataContextSave)
								((IDataContextSave)item).Save(context, attach);
				});
			}
		}

		public static bool Delete(Expression<Func<T, bool>> conditionals) 
		{
			using (DataContext db = CreateContext())
			{
				var x = db.GetTable<T>().Where(conditionals);
				if (x.Count() > 0)
				{
					db.GetTable<T>().DeleteAllOnSubmit(x);
					db.SubmitChanges();
					return true;
				}
				return false;
			}
		}


		public static DataContext CreateContext()
		{
			return new DataContext(ApplicationContext.ConnectionString);
		}

		public static T FindSingle(Expression<Func<T, bool>> conditionals, params Expression<Func<T, object>>[] includes)
		{
			using (DataContext db = CreateContext())
			{
				return FindSingle(db, conditionals, includes);
			}
		}


		public static T FindSingle(DataContext db, Expression<Func<T, bool>> conditionals, params Expression<Func<T, object>>[] includes)
		{
			if (includes != null)
			{
				DataLoadOptions options = new DataLoadOptions();
				foreach (Expression<Func<T, object>> x in includes)
					options.LoadWith<T>(x);
				db.LoadOptions = options;
			}
			var obj = db.GetTable<T>().Where(conditionals).FirstOrDefault();
			obj.MarkOld();
			return obj;
		}
	
		#region IDataContextSave Members

		void  IDataContextSave.Save(DataContext context, bool attach)
		{
 			this.Save(context, attach);
		}

		void  IDataContextSave.MarkForDeletion()
		{
 			this.MarkForDeletion();
		}

		#endregion
	}
}
