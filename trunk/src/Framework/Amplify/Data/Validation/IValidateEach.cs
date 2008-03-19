

namespace Amplify.Data.Validation
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	using Amplify.ComponentModel;

	public interface IValidateEach : IValidationRule 
	{
		IEnumerable<string> Targets { get; }
		ValidatePropertyCallback Callback { get; } 
	}
}
