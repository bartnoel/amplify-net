using System;
//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Security.Principal
{
	using System.Collections.Generic;
	using System.Text;
	using System.Security.Principal;

	/// <summary>
	/// Specialized IIdenity interface. 
	/// </summary>
	public interface IIdentity : System.Security.Principal.IIdentity 
	{
        Object UserId { get; }
	}
}
