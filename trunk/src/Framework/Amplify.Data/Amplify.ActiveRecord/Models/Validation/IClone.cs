using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Amplify.ComponentModel;

namespace Amplify.Models.Validation
{
	public interface IClone
	{
		IValidationRule Clone();
	}
}
