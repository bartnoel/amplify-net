

namespace Amplify.Data.Validation
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Text.RegularExpressions;

	public interface IValidateFormat : Amplify.ComponentModel.IValidationRule

	{
		string Pattern { get; }
		RegexOptions Options { get; }
	}
}
