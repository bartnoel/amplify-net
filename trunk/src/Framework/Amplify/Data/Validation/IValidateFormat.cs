

namespace Amplify.Data.Validation
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Text.RegularExpressions;

	using Amplify.ComponentModel;

	public interface IValidateFormat : IValidationRule

	{
		string Pattern { get; }
		RegexOptions Options { get; }
	}
}
