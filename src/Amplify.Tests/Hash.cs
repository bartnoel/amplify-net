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
        Describe(typeof(Hash)),
        InContext("should perform its specified behavor."),
        Tag(Tags.Unit),
        By("Michael Herndon", "mherndon@opensourceconnections.com", "www.amptools.net")
    ]
    public class Hash_Specification : Spec
    {

        [It, Should(" have a public default constructor. ")]
        public void InvokeConstructor()
        {
            Hash obj = new Hash();
            obj.ShouldNotBeNull();

            obj = Hash.New();
            obj.ShouldNotBeNull();
        }

        [It, Should(" be able to add key pair values ")]
        public void AddKeyPairs()
        {
            Hash obj = new Hash() { { "test", 1 }, { "1", "yeah" } };
            obj.First().Key.ShouldBe("test");
            obj.First().Value.ShouldBe(1);

            obj["1"].ShouldBe("yeah");

            obj = new Hash(new KV("test", 1), new KV("1", "test2"));

            obj.First().Key.ShouldBe("test");
            obj.First().Value.ShouldBe(1);

            obj["1"].ShouldBe("test2");

            obj = new Hash(new Dictionary<string, object>() {
                {"test", 3}, {"test1", 5} 
            });

            obj.First().Key.ShouldBe("test");
            obj.First().Value.ShouldBe(3);

            obj["test1"].ShouldBe(5);
        }
    }
}
