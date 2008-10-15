using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify
{
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
