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

	public class GenericPrincipal : System.Security.Principal.GenericPrincipal, IPrincipal
	{
		private List<string> permissions;

		public GenericPrincipal(IIdentity identity, string[] roles, string[] permissions):base(
			(System.Security.Principal.IIdentity)identity, roles)
		{
			this.permissions = new List<string>(permissions);
			//System.Web.Security.
		}

		#region IPrincipal Members

		public new IIdentity Identity
		{
			get { return (IIdentity)base.Identity; }
		}

		public bool HasPermission(string permission)
		{
			return this.permissions.Contains(permission.ToLower());
		}

		#endregion
	}
}
