//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Data.Validation
{
	/// <summary>
	/// The contract of a validation rule that can be consumed by the validation framework
	/// of the web UI and possibly other clients.
	/// </summary>
	public interface IValidationRule
	{
		/// <summary>
		/// The special name of the rule.
		/// </summary>
		string RuleName { get; }

		/// <summary>
		/// The name of the property to which the rule is being applied to.
		/// </summary>
		string PropertyName { get; }

		/// <summary>
		/// The description of the rule.
		/// </summary>
		string Message { get; set; }

		bool ValidateData(object obj, object data);

		/// <summary>
		/// 
		/// </summary>
		object Level { get; set; }
	}
}
