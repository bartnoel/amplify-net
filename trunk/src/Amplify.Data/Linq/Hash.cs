//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
// C# Hash base upon Alex Henderdon & Andrey Shchekin
// http://blog.bittercoder.com/PermaLink,guid,206e64d1-29ae-4362-874b-83f5b103727f.aspx 
//-----------------------------------------------------------------------


namespace Amplify.Linq
{
#if ! LINQ
	using System;
	using System.Collections.Generic;
	using System.Collections;
	using System.ComponentModel;
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
		public Hash(IEqualityComparer<string> comparer, IDictionary values) :
			base(comparer)
		{

			this.AddRange(values);
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
		/// Adds a rane of value pairs to the hash.
		/// </summary>
		/// <param name="values">The <see cref="System.Collections.IDictionary"/> object</param>
		public void AddRange(IDictionary values)
		{
			foreach (KeyValuePair<object, object> item in values)
				this.Add(item.Key.ToString(), item.Value);
		}

	


		/// <summary>
		/// Gets or sets a value by key.
		/// </summary>
		/// <param name="key">The string key.</param>
		/// <returns>The value of the key, or null if the value does not exist.</returns>
		new public object this[string key]
		{
			get
			{
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
#endif
}
