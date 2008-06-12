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

    public class GenericIdentity : IIdentity 
    {
        private Object id;
        private System.Security.Principal.IIdentity identity;

        public GenericIdentity(System.Security.Principal.IIdentity identity, Guid id)
        {
            this.identity = identity;
            this.id = id;
        }

        #region IIdentity Members

        public Object UserId
        {
            get { return this.id; }
        }

        #endregion

        #region IIdentity Members

        public string AuthenticationType
        {
            get { return this.identity.AuthenticationType; }
        }

        public bool IsAuthenticated
        {
            get { return this.identity.IsAuthenticated; }
        }

        public string Name
        {
            get { return this.identity.Name; }
        }

        #endregion
    }
}
