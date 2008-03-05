﻿//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.ComponentModel
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	public interface IValidateProperty
	{
		bool Validate(string propertyName, object value); 
	}
}