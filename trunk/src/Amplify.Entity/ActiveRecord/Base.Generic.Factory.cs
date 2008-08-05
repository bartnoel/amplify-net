//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.ActiveRecord
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Text;

	using Amplify.Data;
	using Amplify.Linq;

	public abstract partial class Base<T> : IFetchService 
	{


		#region New

		public static T New()
		{
			T item = Activator.CreateInstance<T>();
			item.Initialize();
			return item;
		}

		public static T New(IDictionary<string, object> values)
		{
			T item = New();
			item.Send(values);
			return item;
		}

#if LINQ
		
		public static T New(params Func<object, object>[] values)
		{
			return New(Hash.New(values));
		}
#endif
		#endregion


		public static IEnumerable<T> Find(params object[] conditions)
		{
			Adapter adapter = Adapter.Get(Mode);
			
			object[] values = null;
			string where = "";

			if(conditions.Length > 0) {
				where = conditions[0].ToString();
				if(conditions.Length > 1) 
				{
					values = new object[conditions.Length -1];
					conditions.CopyTo(values, 1);
				}
			}
			ModelMetaInfo info = ModelMetaInfo.Get(typeof(T));
			
			string sql = string.Format("SELECT * FROM {0} ", adapter.QuoteTableName(info.Tables[0].TableName));
			if (!string.IsNullOrEmpty(where))
				sql += " WHERE " + where + " ";

			return FindBySql(sql, null, values);
		}

		public static IEnumerable<T> Find(IOptions options)
		{
			Adapter adapter = Adapter.Get(Mode);
			ModelMetaInfo info = ModelMetaInfo.Get(typeof(T));
			options.From = info.Tables[0].TableName;
			

			string sql = adapter.ConstructFinderSql(options);
			string[] includes = new string[options.Include.Keys.Count];
			options.Include.Keys.CopyTo(includes, 0);
			return FindBySql(sql, includes, options.Conditions);
		}

		public static IEnumerable<T> FindBySql(string sql, string[] includes, params object[] values)
		{
			List<T> list = new List<T>();

			Adapter adapter = Adapter.Get(Mode);
			using (IDataReader dr = adapter.Select(sql, values))
			{
				while (dr.Read())
				{
					T item = Activator.CreateInstance<T>();
					item.Fill(new object[] {dr, includes});
					list.Add(item);
				}
			}
			return list;
		}



		#region IFetchService Members

		

		#endregion

		#region IFetchService Members

		object IFetchService.Fetch(params object[] conditions)
		{
			return Find(conditions);
		}

		#endregion
	}
}
