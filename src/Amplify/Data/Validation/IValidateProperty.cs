//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Data.Validation
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	/// <summary>
	/// Contract for objects that will validate a property by its name and value.
	/// </summary>
	public interface IValidateProperty
	{
		/// <summary>
		/// Validates a property by name.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="value">The value of the property.</param>
		/// <returns>returns true if the property is valid.</returns>
		bool Validate(string propertyName, object value); 
	}
}
