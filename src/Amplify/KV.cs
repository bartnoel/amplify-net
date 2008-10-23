//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify
{
    using System;
    using System.Collections.Generic;
    using System.Collections;
    using System.Text;


    /// <summary>
    /// KV (key value) pair used to help instantiated a hash object for
    /// .NET 2.0. 
    /// </summary>
    public class KV 
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; protected set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public object Value { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KV"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public KV(string key, object value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}
