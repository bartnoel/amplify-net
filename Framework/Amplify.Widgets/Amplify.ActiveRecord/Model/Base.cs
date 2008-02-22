//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Model
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data;
	using System.Data.Linq;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Text;
	using System.Reflection;

	using Amplify.Data;
	using Amplify.Linq;

	public abstract class Base<T> : UnitOfWork, IDataErrorInfo, IChild, IParent 
		where T: Base<T>, new()
	{
		private IParent parent;
		private bool isReadOnly = false;
		


		private static bool initializeOnce = false;
		private static bool isParent = false;
		private static string[] primaryKeys = new string[] { "Id" };
		private static List<ColumnAttribute> properties;
		private static List<AssociationAttribute> associations;
		private static TableAttribute table;

		private static Adapter adapter;


		public Base()
		{
			this.StaticInitialize();
			this.Initialize();
		}

		public virtual bool CascadeDelete
		{
			get { return true; }
		}

		public virtual string ModuleName
		{
			get { return "Application"; }
		}

		internal protected static Adapter Adapter {
			get {
				if (adapter == null)
					adapter = this.GetAdapter(ApplicationContext.ConnectionStringSettings.ProviderName);
				return adapter;
			}
			set {
				adapter = value;
			}
		}

		protected static Adapter GetAdapter(string provider)
		{
			switch (provider.ToLower())
			{
				case "system.data.sqlclient":
				default:
					return new Amplify.Data.SqlClient.SqlAdapter();
			}
		}

		internal protected abstract IEnumerable<ColumnAttribute> Properties
		{
			get { return properties; }
		}
		
		internal protected virtual IEnumerable<string> PrimaryKeys 
		{
			get { return primaryKeys; } 
		}

		public bool IsReadOnly
		{
			get { return this.isReadOnly; }
			internal protected set { this.isReadOnly = value; }
		}

		public static T New()
		{
			return Activator.CreateInstance<T>();
		}

		public static T New(IDictionary values)
		{
			T obj = New();
			obj.Merge(values);
			return obj;
		}

		public static T New(params Func<object, object>[] options)
		{
			return New(Hash.New(options));
		}

		public static T Create()
		{
			T obj = New();
			obj.Save();
			return obj;
		}

		public static T Create(params Func<object, object>[] options)
		{
			return Create(Hash.New(options));
		}

		public static T Create(IDictionary values)
		{
			T obj = New(values);
			obj.Save();
			return obj;
		}


		public static T FindInitial(Options options)
		{
			if (!string.IsNullOrEmpty(options.Include))
				options.Limit = 1;
			return FindEvery(options).First();
		}

		private static IEnumerable<T> FindEvery(Options options)
		{
			return  FindBySql(Adapter.ConstructFinderSql(options), options.Conditions);
		}

		public static IEnumerable<T> FindBySql(string sql, params object[] values)
		{
			return Adapter.Select<T>(sql, values);
		}

		public static T Find(int id)
		{
			string where = primaryKeys[0] + " = {0} ";
			return FindInitial(new Options(Conditions => (object)(new object[]{where, id} )));
		}

		public static T Find(Guid id)
		{
			return FindInitial(new Options(Conditions => (object) (new object[] { primaryKeys[0] + " = {0}", id })));
		}

		public static object Find(Scope scope, params Func<object, object>[] options)
		{
			if (scope == Scope.First)
				return (object)FindInitial(new Options(options));
			else
				return (object)FindEvery(new Options(options));
		}

		public static IEnumerable<T> Find(params Func<object, object>[] options)
		{
			return FindEvery(new Options(options));
		}

		public static T FindOrCreate(int id)
		{
			T item = Find(id);
			if (item == null)
				return New();
			return item;
		}

		public static T FindOrCreate(Guid id)
		{
			T item = Find(id);
			if(item == null)
				return New();
			return item;
		}


		

		internal protected Base(IDictionary values)
		{
			this.StaticInitialize();
			this.Initialize();
			this.Merge(values);
		}

		internal protected Base(object transferObject)
		{
			this.StaticInitialize();
			this.Initialize();
			if (transferObject is IDictionary)
				this.Merge((IDictionary)transferObject);
			if(transferObject is DecoratedInternalObject)
				this.Merge((DecoratedObject)transferObject);
		}


		protected void StaticInitialize() 
		{
			if (!initializeOnce)
			{
				initializeOnce = true;
				this.GetAttributesOfType();
				this.AddStaticRules();
			}
		}

		protected virtual void GetAttributesOfType()
		{
		
			PropertyInfo[] propertyInfos = typeof(T).GetProperties();
			properties = new List<ColumnAttribute>();
			associations = new List<AssociationAttribute>();
			List<string> keys = new List<string>();
			foreach(PropertyInfo info in propertyInfos)
			{
				object[] attributes = info.GetCustomAttributes(true);
				foreach (object attribute in attributes)
				{
					if (attribute is TableAttribute)
						table = (TableAttribute)attribute;

					if (attribute is ColumnAttribute)
					{
						ColumnAttribute column = (ColumnAttribute)attribute;
						column.PropertyName = info.Name;
						if (column.IsPrimaryKey)
							keys.Add(column.Name);
						properties.Add(column);
					}

					if (attribute is AssociationAttribute)
					{
						AssociationAttribute assocation = (AssociationAttribute)attribute;
						if (info.PropertyType.IsInstanceOfType(IEnumerable)) 
						{
							if (info.PropertyType.IsGenericType)
								assocation.Class = info.PropertyType.GetGenericArguments()[0];
							else {
								PropertyInfo prop = info.PropertyType.GetProperty("Items");
								if (prop != null)
									assocation.Class = prop.PropertyType;
							}
						}
						else
							assocation.Class = info.PropertyType;
						if (assocation.Type != AssocationType.BelongsTo)
							isParent = true;
						associations.Add((AssociationAttribute)attribute);
					}
				}
			}
		}

		protected virtual void InitializeValues()
		{
			foreach (ColumnAttribute attribute in Properties)
				this[attribute.Name] = attribute.DefaultValue;
		}

		protected virtual void AddStaticRules()
		{

		}

		protected virtual void AddRules()
		{

		}

		internal protected virtual void Initialize() 
		{
			this.InitializeValues();
			this.AddRules();
		}
		


		internal protected void Merge(IDictionary values) 
		{
			foreach (string key in values.Keys)
				this[key] = values[key];
		}


		internal protected void Merge(DecoratedObject obj)
		{
			this.MergeInternal(obj);
		}


		#region CRUD

		public override object Save()
		{
			if (this.IsParent)
			{
				using (IDbConnection connection = Adapter.Connect())
				{
					IDbTransaction tr = connection.BeginTransaction();
				
					object item = Save(tr);
					tr.Commit();
					return item;
				}
			}
			return base.Save();
		}



		public virtual object Save(IDbTransaction transaction)
		{
			if (this.IsDeletable)
			{
				this.DeleteSelf(transaction);
				this.DeleteChildren(transaction);
			}
			else
			{
				if (this.IsNew)
					this.Insert(transaction);
				else
					this.Update(transaction);
				this.SaveChildren(transaction);
			}
			return this;
		}

		protected override void Insert()
		{
			List<object> values = new List<object>();
			string statement = "";
			foreach (ColumnAttribute attr in properties)
			{
				statement += "{0},".Inject(attr.Name);
				object value = this[attr.Name];
				if (value == null)
					value = DBNull.Value;
				values.Add(value);
			}
			string sql = "";
			this[primaryKeys[0]] = Adapter.Insert(sql, values);
		}

		protected virtual void Insert(IDbTransaction transaction)
		{
			List<object> values = new List<object>();
			string statement = "";
			foreach (ColumnAttribute attr in properties)
			{
				statement += "{0},".Inject(attr.Name);
				object value = this[attr.Name];
				if (value == null)
					value = DBNull.Value;
				values.Add(value);
			}
			string sql = "INSERT INTO " +  +;
			this[primaryKeys[0]] = Adapter.Insert(sql, transaction, values);
		}

		protected virtual void DeleteChildren(IDbTransaction transaction)
		{
			if (this.CascadeDelete)
			{
				foreach (AssociationAttribute attr in associations)
				{
					if (attr.Type != AssocationType.BelongsTo)
					{
						if (attr.Class.IsInstanceOfType(IEnumerable))
						{
							if (attr.Class.IsInstanceOfType(IDeletable))
								((IDeletable)this[attr.Class.Name.Pluralize()]).Delete(transaction);
							else
								foreach (IDeletable item in this[attr.Class.Name.Pluralize()])
									item.Delete(transaction);
						}
						else if (attr.Class.IsInstanceOfType(IDeletable))
							((IDeletable)this[attr.Class.Name]).Delete(transaction);
					}
				}
			}
		}

		protected virtual void SaveChildren(IDbTransaction transaction)
		{
			foreach (AssociationAttribute attr in associations)
			{
				if (attr.Type != AssocationType.BelongsTo)
				{
					if (attr.Class.IsInstanceOfType(IEnumerable))
					{
						if (attr.Class.IsInstanceOfType(ISaveable))
							((ISaveable)this[attr.Class.Name.Pluralize()]).Save(transaction);
						else
							foreach (ISaveable item in this[attr.Class.Name.Pluralize()])
								item.Save(transaction);
					}
					else if (attr.Class.IsInstanceOfType(ISaveable))
						((ISaveable)this[attr.Class.Name]).Save(transaction);
				}
			}
		}

		protected override void Update()
		{
			base.Update();
		}

		protected virtual void Update(IDbTransaction transaction)
		{

		}

		public override bool Delete()
		{
			return base.Delete();
		}

		public override bool Delete(IDbTransaction transaction)
		{

		}

		protected override void DeleteSelf()
		{
			base.DeleteSelf();
		}

		protected virtual void DeleteSelf(IDbTransaction transaction)
		{

		}

		

		#endregion


		#region IDataErrorInfo Members

		string IDataErrorInfo.Error
		{
			get { throw new NotImplementedException(); }
		}

		string IDataErrorInfo.this[string columnName]
		{
			get { throw new NotImplementedException(); }
		}

		#endregion

		#region IDataValidationInfo Members

		//IEnumerable<IValidationRule> IDataValidationInfo.this[string propertyName]
		//{
		//    get { throw new NotImplementedException(); }
		//}

		#endregion

		#region IChild Members

		IParent IChild.Parent
		{
			get { return this.parent; }
		}

		void IChild.SetParent(IParent parent)
		{
			this.parent = parent;
		}

		#endregion

		#region IParent Members

		public virtual bool IsParent
		{
			get { return isParent; }
		}

		public virtual void RemoveChild(object child)
		{
			Type type = child.GetType();
			foreach (AssociationAttribute attr in associations)
				if (attr.Class == type)
					((IList)this[attr.Class.Name.Pluralize()]).Remove(child);
		}

		#endregion
	}
}
