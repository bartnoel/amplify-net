//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Data.Validation
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	/// <summary>
	/// Provides the functionality to offer custom validation information that a user interface can bind to and
	/// create validation on the fly.
	/// </summary>
	public interface IDataValidationInfo
	{
		/// <summary>
		/// Indexed property that returns a list of validation constraints.
		/// </summary>
		/// <param name="propertyName"> The name of the property of the object that is being validated. </param>
		/// <returns> A list of validation constraints. </returns>
		IEnumerable<IValidationRule> this[string propertyName] { get; }
	}
}