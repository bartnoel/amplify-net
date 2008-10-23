//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
// C# Hash base upon Alex Henderdon & Andrey Shchekin
// http://blog.bittercoder.com/PermaLink,guid,206e64d1-29ae-4362-874b-83f5b103727f.aspx 
//-----------------------------------------------------------------------

namespace Amplify
{
	using System;
	using System.Collections.Generic;
	using System.Collections;
	using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
#if LINQ 
	using System.Linq;
	using System.Linq.Expressions;
#endif  
    using System.Runtime.Serialization;
	using System.Text;

	/// <summary>
	/// A hash object, similiar to a String/Object Dictionary.
	/// </summary>
	[DefaultBindingProperty("Keys"),Serializable]
    [SuppressMessage("Microsoft.Naming", "CA1710", Justification = "Its correctly named")]
	public class Hash : Dictionary<string,object> 
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Hash"/> class.
        /// </summary>
		public Hash() : base() 
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hash"/> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
		public Hash(IEqualityComparer<string> comparer) : base(comparer) 
        { 
        }

#if LINQ
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
#endif

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

#if LINQ
		/// <summary>
		/// Constructor that allows a ruby like hash notation
		/// </summary>
		/// <param name="funcs">The value pairs: i.e. Name => &quot;value&quot;, Age => 10</param>
		public Hash(params Func<object, object>[] funcs):base ()
		{
			this.AddRange(funcs);
		}
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="Hash"/> class.
        /// </summary>
        /// <param name="keyValues">The key values.</param>
        public Hash(params KV[] keyValues)
        {
            foreach (KV item in keyValues)
                this.Add(item.Key, item.Value);
        }

		/// <summary>
		/// Constructor that takes an IDictionary object and consumes it, using ToString() on each key.
		/// </summary>
		/// <param name="values">The <see cref="System.Collections.IDictionary"/> object</param>
		public Hash(IDictionary<string, object> values)
		{
			this.AddRange(values);
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="Hash"/> class.
        /// </summary>
        /// <param name="values">The values.</param>
        public Hash(IDictionary<object, object> values)
        {
            this.AddRange(values);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hash"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected Hash(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { 
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

#if LINQ
		/// <summary>
		/// Adds a range of value pairs to the hash.
		/// </summary>
		/// <param name="funcs">The value pairs: i.e. Name => &quot;value&quot;, Age => 10</param>
		public void AddRange(params Func<object, object>[] funcs)
		{
			foreach (Func<object, object> func in funcs)
				this.Add(func);
		}
#endif

		/// <summary>
		/// Adds a rane of value pairs to the hash.
		/// </summary>
		/// <param name="values">The <see cref="System.Collections.IDictionary"/> object</param>
		public void AddRange(IDictionary<string, object> values)
		{
			foreach (KeyValuePair<string, object> item in values)
				this.Add(item.Key.ToString(), item.Value);
		}

        /// <summary>
        /// Adds a rane of value pairs to the hash.
        /// </summary>
        /// <param name="values">The <see cref="System.Collections.IDictionary"/> object</param>
        public void AddRange(IDictionary<object, object> values)
        {
            foreach (KeyValuePair<object, object> item in values)
                this.Add(item.Key.ToString(), item.Value);
        }

#if LINQ
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
#endif

		

#if LINQ
		/// <summary>
		/// Creates a new hash and allows a ruby like hash notation
		/// </summary>
		/// <param name="funcs">The value pairs: i.e. Name => &quot;value&quot;, Age => 10</param>
		public static Hash New(params Func<object, object>[] funcs)
		{
			return new Hash(funcs);
		}
#endif

		/// <summary>
        /// Creates a new hash from the values in the IDictionary object.
		/// </summary>
		/// <param name="values">The <see cref="System.Collections.IDictionary"/> object</param>
		public static Hash New(IDictionary<string, object> values)
		{
			return new Hash(values);
		}

        /// <summary>
        ///  Creates a new hash from the values in the IDictionary object.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static Hash New(IDictionary<object, object> values)
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
