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
	/// A contract that enforces a delete command.  
	/// </summary>
	public interface IDeletable
	{
		/// <summary>
		/// Deletes this instance.
		/// </summary>
		/// <returns> returns true if the delete occured, otherwise false. </returns>
		bool Delete();
	}
}
