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
	using System.Web.UI;

	public interface IWebFormValidation
	{
		List<IValidator> GetValidators(string propertyName); 
	}
}
