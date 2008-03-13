using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify.Data.Validation
{
	public interface IValidateEach
	{
		IEnumerable<string> Targets { get; }
		ComponentModel.ValidatePropertyCallback Callback { get; } 
	}
}
