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
		Describe(typeof(IDecoratedObject)),
		InContext("should perform its specified behavor."),
		Tag(Tags.Unit),
		By("Michael Herndon", "mherndon@opensourceconnections.com", "www.amptools.net")
	]
	public class IDecoratedObject_Specification : Spec
	{

		public class Decorated : IDecoratedObject
		{
			private Dictionary<string, object> properties = new Dictionary<string, object>();

			public object this[string propertyName]
			{
				get { return this.properties[propertyName]; }
				set { this.properties[propertyName] = value;  }
			}
		}

		[It, Should(" be able to set internal properties and retrieve them. ")]
		public void SetAndRetrieve()
		{
			IDecoratedObject obj = new Decorated();
			obj["test"] = "test";
			obj["test"].ShouldBe("test");
		}
	}
}
