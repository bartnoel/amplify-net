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

        [It, Should(" have a factory method that gets the default adapter. ")]
        public void Get()
        {
			// this test assumes that you have connection strings in the configuration.
            Adapter adapter = Adapter.Get();
            adapter.GetType().ShouldBe(typeof(Adapters.SqlAdapter));
            adapter.ShouldBe(Adapter.Get());
        }
		
		[It, Should(" add an adapter to current list of adapters ")]
		public void Add()
		{
			Adapter.Add("current", "system.data.sqlclient", "fakeconnectionstring");
			Adapter adapter = Adapter.Get("current", false);
			adapter.ShouldNotBeNull();
			adapter.ConnectionString.ShouldBe("fakeconnectionstring");
		}

		[It, Should(" throw an exception when trying to obtain an adapter with a bad key ")]
		[ExpectedArgumentException("the connection string key \"bad-key_production\" could not be found in the web/app.config")]
		public void TryToGetInvalidKey()
		{
			ArgumentException ex = null;
			try
			{
				Adapter.Get("bad-key");
			}
			catch (Exception exception)
			{
				if (exception is ArgumentException)
					ex = (ArgumentException)exception;
			}
			if (ex == null)
				Assert.Fail("an argument exception should have occured");
		}
    }
}
