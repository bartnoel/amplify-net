//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.ActiveRecord
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

	using Amplify.ActiveRecord.Data;

	//using Amplify.ComponentModel;
	using Amplify.ActiveRecord;

	public abstract class Base<T> : UnitOfWork, IDataErrorInfo, IChild 
		where T: Base<T>
	{
		private IParent parent;

		private static AdapterBase<T> adapter;

		internal protected static Data.AdapterBase<T> Adapter {
			get {
				if (adapter == null)
					adapter = new LinqAdapter<T>(Cfg.Context);
				return adapter;
			}
			set {
				adapter = value;
			}
		}
		internal protected abstract IEnumerable<string> Properties { get; }
		internal protected abstract IEnumerable<string> PrimaryKeys { get; }

		


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

		public static T Create()
		{
			T obj = New();
			obj.Save();
			return obj;
		}

		public static T Create(IDictionary values)
		{
			T obj = New(values);
			obj.Save();
			return obj;
		}

		

		

		public static IEnumerable<T> FindBySql(string sql, params object[] values)
		{
			return Adapter.ExecuteQuery(sql, values);
		}

		public static T Find(object id) 
		{
			return Adapter.Select(id);
		}

		public static IEnumerable<T> Find(IOptions options)
		{
			return Adapter.Select(options);
		}

		public static IEnumerable<T> Find()
		{
			return Adapter.SelectAll();
		}

		public static IEnumerable<T> Find(string where, params object[] values)
		{
			if (where.StartsWith("by_")) {
				string[] clause =	where.Replace("by_", "").Split("_".ToCharArray());
				int count = 0;
				where = "";
				foreach(string peice in clause) {
					if (!peice.ToLower().Contains("and") &&
							!peice.ToLower().Contains("or"))
					{
						where += peice + string.Format(" = @{0} ", count);
						count++;
					}
					else
						where += peice + " ";
				}
			}
			return Adapter.Select(new Options().Where(where, values));
		}

		public static IEnumerable<T> FindAll(IOptions options)
		{
			return Adapter.SelectAll(options);
		}

		public static IEnumerable<T> FindAll()
		{
			return Adapter.SelectAll();
		}

		public static T FindOne(IOptions options)
		{
			return Adapter.SelectOne(options);
		}

		public static T FindOrCreate(object id)
		{
			T item = Adapter.Select(id);
			if (item == null)
				return New();
			return item;
		}


		public Base()
		{
			this.Initialize();
		}

		internal protected Base(IDictionary values)
		{
			this.Initialize();
			this.Merge(values);
		}

		internal protected Base(object transferObject)
		{
			this.Initialize();
			if (transferObject is IDictionary)
				this.Merge((IDictionary)transferObject);
			if(transferObject is DecoratedInternalObject)
				this.Merge((DecoratedObject)transferObject);
		}


		internal protected virtual void Initialize() 
		{

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
	}
}
