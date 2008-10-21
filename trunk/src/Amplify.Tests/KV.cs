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
        Describe(typeof(KV)),
        InContext("should populate a key value."),
        Tag(Tags.Unit),
        By("Michael Herndon", "mherndon@opensourceconnections.com", "www.amptools.net")
    ]
    public class KV_Specification : Spec
    {

        [It, Should(" have a public default constructor, that forces to create a key, value. ")]
        public void InvokeConstructor()
        {
            KV obj = new KV("key", "value");
            obj.ShouldNotBeNull();
            obj.Key.ShouldBe("key");
            obj.Value.ShouldBe("value");
        }
    }
}
