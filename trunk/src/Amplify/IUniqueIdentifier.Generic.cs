using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify
{
	/// <summary>
	/// The generic contract for forcing an object to have an unique identifier. 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IUniqueIdentifier<T> : IUniqueIdentifier 
	{
		/// <summary>
		/// Gets the unique identifier of type T of object.
		/// </summary>
		/// <value>The id.</value>
		T Id { get; }
	}
}
