//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Model
{
	using System.Data;
	/// <summary>
	/// A contract with an object that is saveable.
	/// </summary>
	public interface ISaveable
	{
		bool IsSaveable { get; }
		/// <summary>
		/// Saves the object into a datastore.
		/// </summary>
		/// <returns> The object that was saved. </returns>
		object Save();

		object Save(IDbTransaction transaction);
	}
}
