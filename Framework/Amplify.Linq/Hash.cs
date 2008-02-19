//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
// C# Hash base upon Alex Henderdon & Andrey Shchekin
// http://blog.bittercoder.com/PermaLink,guid,206e64d1-29ae-4362-874b-83f5b103727f.aspx 
//-----------------------------------------------------------------------


namespace Amplify.Linq
{
	using System;
	using System.Collections.Generic;
	using System.Collections;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Text;


	public class Hash : Dictionary<string, object> 
	{

		public Hash() : base() { }

		public Hash(IEqualityComparer<string> comparer) : base(comparer) { }


		public Hash(IEqualityComparer<string> comparer, params Func<object, object>[] funcs): 
			base(comparer)
		{
			this.AddRange(funcs);
		}

		public Hash(params object[] values)
		{
			int count = 1;
			string key = "";
			foreach (object value in values)
			{
				if (count % 2 == 0)
					this.Add(key, value);
				else
					key = value.ToString();
				count++;
			}
		}


		public Hash(params Func<object, object>[] funcs):base ()
		{
			this.AddRange(funcs);
		}

		public Hash(IDictionary values)
		{
			foreach (object key in values.Keys)
				if (key is string)
					this[key.ToString()] = values[key];
		}

		public void AddRange(params Func<object, object>[] funcs)
		{
			foreach (Func<object, object> func in funcs)
				this.Add(func);
		}

		public void Add(Func<object, object> func)
		{
			string key = func.Method.GetParameters()[0].Name;
			object value = func(null);
			this.Add(key, value);
		}


		new public object this[string key]
		{
			get {
				if (this.ContainsKey(key))
					return base[key];
				return null;
			}
			set
			{
				base[key] = value;
			}
		}


		public static Hash New(params Func<object, object>[] funcs)
		{
			return new Hash(funcs);
		}

		public static Hash New(IDictionary values)
		{
			return new Hash(values);
		}
	}	
}
