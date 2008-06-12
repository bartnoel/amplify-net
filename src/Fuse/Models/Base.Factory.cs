using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuse.Models
{
	using Amplify.Linq;
	using System.Data.Common;



	public partial class Base<T> : IFill 
	{

		public static IEnumerable<T> Find(Options options)
		{
			if (options.Scope == Scope.One)
				return new T[] { FindOne(options) };
			else
				return FindAll(options);
		}


		public static IEnumerable<T> Find(params Func<object, object>[] funcs)
		{
			Options options = new Options();
			options.Map(Hash.New(funcs));
			if (options.Scope == Scope.One)
				return new T[] { FindOne(options) };
			else
				return FindAll(options);
		}

		public static IEnumerable<T> FindAll(Options options)
		{
			options.Scope = Scope.All;
			return FindByOptions<T>(options);
		}

		public static IEnumerable<T> FindAll(params Func<object, object>[] funcs)
		{
			Options options = new Options();
			options.Map(Hash.New(funcs));
			return FindAll(options);
		}


		public static T FindOne(Options options)
		{
			options.Scope = Scope.One;
			return FindByOptions<T>(options).FirstOrDefault();
		}

		public static T FindOne(params Func<object, object>[] funcs)
		{
			Options options = new Options();
			options.Map(Hash.New(funcs));
			return FindOne(options);
		}

		protected static IEnumerable<T> FindByOptions<T>(Options options) where T :Base<T>
		{
			ModelMetaInfo info = ModelMetaInfo.Repository[typeof(T)];
			Amplify.Data.IOptions ioptions = (Amplify.Data.IOptions)options;

			ioptions.From = info.PrimaryTable.TableName;
			ioptions.As = info.PrimaryTable.As;
			ioptions.Select = info.PrimaryTable.Selection;

			string query = adapter.ConstructFinderSql(options);

			return FindBySql<T>(query,  options.Include, options.Conditions);
		}

		public static IEnumerable<T> FindBySql<T>(string sql, IDictionary<string, object> includes, params object[] values) where T : Base<T>
		{
			List<T> list = new List<T>();
			
			using (System.Data.IDataReader dr = adapter.ExecuteReader(sql, values))
			{
				while (dr.Read())
				{
					T item = Activator.CreateInstance<T>();
					item.Fill(dr, includes);
					list.Add(item);
				}
			}
			return list;
		}


		void IFill.Fill(System.Data.IDataReader datareader, IDictionary<string, object> includes)
		{
			this.Fill(datareader, includes);
		}

		internal protected void Fill(System.Data.IDataReader datareader, IDictionary<string, object> includes)
		{
			System.Data.IDataReader dr = datareader;
			((IFill)this).IsDeferred = false;

			for (int i = 0; i < dr.FieldCount; i++)
			{
				string name = this.Info.PrimaryTable.Columns.Single(o => o.Name == dr.GetName(i)).Property.Name;
				object value = dr[i];
				if (value == DBNull.Value)
					this[name] = null;
				else
					this[name] = value;
			}

			List<AssocationAttribute> associations = new List<AssocationAttribute>(this.Info.Assocations);
			
			if (includes != null && includes.Keys.Count > 0)
			{
				foreach (string name in includes.Keys)
				{
					dr.NextResult();
					AssocationAttribute attr = this.Info.Assocations.SingleOrDefault(o => o.Property.Name == name);
					associations.Remove(attr);
					object item = Activator.CreateInstance(attr.Property.PropertyType);
					if (item is IFill)
						((IFill)item).Fill(dr, (includes[name] as IDictionary<string, object>));

					this[name] = item; 
				}
			}
			
			// if the associations properties where not included they are deferred.
			foreach (AssocationAttribute attr in associations)
			{
				if (attr is IHasAssocation)
				{
					object item = Activator.CreateInstance(attr.Property.PropertyType);
					if(item is IFill)
						((IFill)item).IsDeferred = true;
					this[attr.Property.Name] = item;
				}
			}
		}
	}
}
