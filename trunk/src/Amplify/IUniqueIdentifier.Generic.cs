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
	/// The generic contract for forcing an object to have an unique identifier. 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IUniqueIdentifier<T> 
	{
		/// <summary>
		/// Gets the unique identifier of type T of object.
		/// </summary>
		/// <value>The id.</value>
		T Id { get; }
	}
}
