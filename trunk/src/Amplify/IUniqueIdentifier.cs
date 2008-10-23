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
