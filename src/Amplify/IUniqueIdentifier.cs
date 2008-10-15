using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify
{
	/// <summary>
	/// A contract for forcing and object to providing a unique identifer. 
	/// </summary>
	public interface IUniqueIdentifier
	{
		/// <summary>
		/// Gets the unique id.
		/// </summary>
		/// <value>The id.</value>
		object Id { get; }
	}
}
