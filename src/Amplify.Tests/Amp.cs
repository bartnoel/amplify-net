//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) CompanyName.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using MbUnit.Framework;
    using Gallio.Framework;

    [
        Describe(typeof(Amp)),
        InContext("should perform its specified behavor."),
        Tag(Tags.Unit),
        By("Michael Herndon", "mherndon@opensourceconnections.com", "www.amptools.net")
    ]
    public class Amp_Specification : Spec
    {

        [It, Should(" have static properties ")]
        public void GetProperties()
        {
            Amp.Properties.ShouldNotBeNull();
        }
    }
}
