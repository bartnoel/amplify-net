//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify
{
	using System;
	using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
	using System.Text;

	/// <summary>
	/// A contract that enforces a delete command.  
	/// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1704")]
	public interface IDeleteable
	{
		/// <summary>
		/// Deletes this instance.
		/// </summary>
		/// <returns> returns true if the delete occured, otherwise false. </returns>
		bool Delete();
	}
}
