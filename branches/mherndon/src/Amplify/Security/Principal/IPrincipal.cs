//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Security.Principal
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Security.Principal;

	/// <summary>
	/// Specialized IPrincipal interface.
	/// </summary>
	public interface IPrincipal : System.Security.Principal.IPrincipal 
	{
		new IIdentity Identity { get; } 
		bool HasPermission(string permission);
	}
}
