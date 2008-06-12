using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using System.Linq.Expressions;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

using Amplify.Linq;

namespace Amplify.Models.Objects
{
	public class EntityBase<T> : EntityObject where T: EntityBase<T>, ISaveable<T> 
	{

		private static Dictionary<string, EdmScalarPropertyAttribute> columns;
		private static Dictionary<string, EdmRelationshipAttribute> associations;
		private bool isNew = false;
		private bool isMarkedForDeletion = false;


		protected bool IsMarkedForDeletion
		{
			get { return this.isMarkedForDeletion; }
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
			using (ObjectContext db = CreateContext())
			{
				return db.GetTable<T>().Where(conditionals);
			}
		}

		public static IEnumerable<T> Find(ObjectContext db, Expression<Func<T, bool>> conditionals)
		{
			return db.CreateQuery<T>().Where(conditionals);
		}


		public static IEnumerable<T> Find(Action<IQueryable<T>> query, params Expression<Func<T, object>>[] includes)
		{
			using (ObjectContext db = CreateContext())
			{
				return Find(db, query, includes);
			}
		}

		public static IQueryable FindView(ObjectContext db, Action<IQueryable<T>> query)
		{
			ObjectQuery<T> entity = db.CreateQuery<T>("");
			query(table);
			return table;
		}

		public static IEnumerable<TSource> FindView<TSource>(ObjectContext db, Action<IQueryable<T>> query) where TSource : new()
		{
			return Find(db, query).Select(o => new TSource());
		}

		public static IEnumerable<T> Find(ObjectContext db, Action<IQueryable<T>> query, params Expression<Func<T, object>>[] includes)
		{
			ObjectQuery<T> entity = db.CreateQuery<T>("[{0}]".Inject(typeof(T).Name));
			if (includes != null)
			{
				foreach (var include in includes)
					entity.Include(include);
			}
			
			query(entity);
			return entity;
		}


		public static T New()
		{
			T item = New();
			item.isNew = true;
			return item;
		}

		public static T New(IDictionary<string, object> values)
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
			item.Merge(values);
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

			property.GetCustomAttributes(typeof(EdmRelationshipAttribute), true).
				Cast<EdmRelationshipAttribute>().Each(delegate(EdmRelationshipAttribute association)
			{
				associations.Add(property.Name, association);
			});

			property.GetCustomAttributes(typeof(EdmScalarPropertyAttribute), true).Cast<EdmScalarPropertyAttribute>()
				.Each(delegate(EdmScalarPropertyAttribute column)
				{
					columns.Add(property.Name, column);
				});
		}

		protected override void Validate()
		{
			columns.Each(o => this.ValidateProperty(o.Key));
		}

		public static ObjectContext CreateContext()
		{
			return new ObjectContext(ApplicationContext.ConnectionString);
		}

		public T Save()
		{
			using (ObjectContext context = CreateContext())
			{
				this.Save(context, true);
			}
			return (T)this;
		}

		public T Save(ObjectContext context)
		{
			T item = Save(context, false);
			context.SaveChanges(true);
			return item;
		}

		public T Save(ObjectContext context, bool attach)
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
			using (ObjectContext db = CreateContext())
			{
				this.MarkForDeletion();
				this.Save(db, true);
			}
		}

		public virtual void Delete(ObjectContext context)
		{
			context.GetTable<T>().InsertOnSubmit((T)this);
		}

		protected virtual void Insert(ObjectContext context)
		{
			context.GetTable<T>().InsertOnSubmit((T)this);
		}

		protected virtual void Update(ObjectContext context, bool attach)
		{
			if (attach && this.IsModified)
				context.GetTable<T>().Attach((T)this, true);
		}

		protected virtual void SaveChildren(ObjectContext context, bool attach)
		{
			if (this.IsMarkedForDeletion && this.CascadeDelete)
			{
				this.EachValue(delegate(object value)
				{
					if (value is IList)
					{
						foreach (IObjectContextSave item in (IList)value)
						{
							item.MarkForDeletion();
							item.Save(context, attach);
						}
					}
					else if (value is IObjectContextSave)
					{
						IObjectContextSave item = (IObjectContextSave)value;
						item.MarkForDeletion();
						item.Save(context, attach);
					}
				});
			}
			else
			{
				this.EachValue(delegate(object value)
				{
					if (value is IObjectContextSave)
						((IObjectContextSave)value).Save(context, attach);
					else if (value is IList)
						foreach (object item in (IList)value)
							if (item is IObjectContextSave)
								((IObjectContextSave)item).Save(context, attach);
				});
			}
		}

		public static bool Delete(Expression<Func<T, bool>> conditionals)
		{
			using (ObjectContext db = CreateContext())
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


		public static ObjectContext CreateContext()
		{
			return new ObjectContext(ApplicationContext.ConnectionString);
		}

		public static T FindSingle(Expression<Func<T, bool>> conditionals, params Expression<Func<T, object>>[] includes)
		{
			using (ObjectContext db = CreateContext())
			{
				return FindSingle(db, conditionals, includes);
			}
		}


		public static T FindSingle(ObjectContext db, Expression<Func<T, bool>> conditionals, params string[] includes)
		{
			var obj = db.CreateQuery<T>("[0]".Inject(typeof(T).Name)).Where(conditionals).FirstOrDefault();
			obj.MarkOld();
			return obj;
		}



		protected virtual void ValidateProperty(string propertyName)
		{
			object value =	this.GetType().GetProperty(propertyName).GetValue(this, null);
			this.ValidateProperty(propertyName, value);
		}


		protected virtual void Merge(object value)
		{
			Type type = this.GetType();
			foreach(PropertyInfo property in value.GetType().GetProperties()) 
			{
				PropertyInfo info = type.GetProperty(property.Name);
				if (info != null)
					info.SetValue(this, property.GetValue(value));
			}
		}

		protected virtual void Merge(IDictionary<string, object> properties)
		{
			Type type = this.GetType();
			foreach (string propertyName in properties.Keys)
			{
				PropertyInfo property = type.GetProperty(propertyName);
				if(property != null)
					property.SetValue(this, properties[propertyName]);
			}
		}


		protected virtual void ValidateProperty(string propertyName, object value)
		{
						
		}

		protected override void OnPropertyChanged(string property)
		{
			this.ValidateProperty(property);
			base.OnPropertyChanged(property);
		}
	}
}
