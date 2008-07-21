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
	using System.ComponentModel;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Text;

	/// <summary>
	/// A hash object, similiar to a String/Object Dictionary.
	/// </summary>
	[DefaultBindingProperty("Keys")]
	public class Hash : Dictionary<string, object> 
	{

		public Hash() : base() { }

		public Hash(IEqualityComparer<string> comparer) : base(comparer) { }

		/// <summary>
		/// Constructor that allows for a comparer and ruby like hash notation
		/// </summary>
		/// <param name="comparer">The <see cref="System.Collections.Generic.IEqualityComparer<>"/> for a string key.</param>
		/// <param name="funcs"></param>
		public Hash(IEqualityComparer<string> comparer, params Func<object, object>[] funcs) : 
			base(comparer)
		{
			
			this.AddRange(funcs);
		}

		/*
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
		}*/


		/// <summary>
		/// Constructor that allows a ruby like hash notation
		/// </summary>
		/// <param name="funcs">The value pairs: i.e. Name => &quot;value&quot;, Age => 10</param>
		public Hash(params Func<object, object>[] funcs):base ()
		{
			this.AddRange(funcs);
		}

		/// <summary>
		/// Constructor that takes an IDictionary object and consumes it, using ToString() on each key.
		/// </summary>
		/// <param name="values">The <see cref="System.Collections.IDictionary"/> object</param>
		public Hash(IDictionary values)
		{
			this.AddRange(values);
		}

		/// <summary>
		/// Adds a range of value pairs to the hash.
		/// </summary>
		/// <param name="funcs">The value pairs: i.e. Name => &quot;value&quot;, Age => 10</param>
		public void AddRange(params Func<object, object>[] funcs)
		{
			foreach (Func<object, object> func in funcs)
				this.Add(func);
		}

		/// <summary>
		/// Adds a rane of value pairs to the hash.
		/// </summary>
		/// <param name="values">The <see cref="System.Collections.IDictionary"/> object</param>
		public void AddRange(IDictionary values)
		{
			foreach (KeyValuePair<object, object> item in values)
				this.Add(item.Key.ToString(), item.Value);
		}

		/// <summary>
		/// Adds value pairs to the hash
		/// </summary>
		/// <param name="funcs">The value pair: i.e. Name => &quot;value&quot;, Age => 10</param>
		public void Add(Func<object, object> func)
		{
			string key = func.Method.GetParameters()[0].Name;
			object value = func(null);
			this.Add(key, value);
		}


		/// <summary>
		/// Gets or sets a value by key.
		/// </summary>
		/// <param name="key">The string key.</param>
		/// <returns>The value of the key, or null if the value does not exist.</returns>
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

		/// <summary>
		/// Creates a new hash and allows a ruby like hash notation
		/// </summary>
		/// <param name="funcs">The value pairs: i.e. Name => &quot;value&quot;, Age => 10</param>
		public static Hash New(params Func<object, object>[] funcs)
		{
			return new Hash(funcs);
		}


		/// <summary>
		/// Creates a new hash from the values in the IDictionary object.
		/// </summary>
		/// <param name="values">The <see cref="System.Collections.IDictionary"/> object</param>
		public static Hash New(IDictionary values)
		{
			return new Hash(values);
		}

		/// <summary>
		/// Creates a new hash.
		/// </summary>
		/// <returns></returns>
		public static Hash New()
		{
			return new Hash();
		}
	}	
}
