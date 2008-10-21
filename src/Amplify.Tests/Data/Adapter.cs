//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) CompanyName.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using MbUnit.Framework;
    using Gallio.Framework;

    [
        Describe(typeof(Adapter)),
        InContext("should perform its specified behavor."),
        Tag(Tags.Unit),
        By("Michael Herndon", "mherndon@opensourceconnections.com", "www.amptools.net")
    ]
    public class Adapter_Specification : Spec
    {

        [It, Should(" have a public default constructor. ")]
        public void InvokeConstructor()
        {
            // Adapter obj = new Adapter();
            //obj.ShouldNotBeNull();
        }
    }
}
