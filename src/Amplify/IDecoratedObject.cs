//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify
{
    using System;
    using System.Collections.Generic;
    using System.Text;

	/// <summary>
	/// A contract that forces an object to be able to get/set properties using 
	/// an Indexer Property.  
	/// </summary>
	public interface IDecoratedObject
	{
		/// <summary>
		/// Gets or sets the <see cref="System.Object"/> with the specified property name.
		/// </summary>
		/// <value></value>
		object this[string propertyName] { get; set; }
	}
}
