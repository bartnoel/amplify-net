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
		Describe(typeof(IDeletable)),
		InContext("a basic example of how to use IDeletable."),
		Tag(Tags.Instructional),
		By("Michael Herndon", "mherndon@opensourceconnections.com", "www.amptools.net")
	]
	public class IDeletable_Specification : Spec
	{
		public class Deletable : IDeletable
		{
			public string Value { get; set; }

			public bool Delete()
			{
				if(!string.IsNullOrEmpty(this.Value))
				{
					this.Value = null;
					return this.Value == null;
				}
				return false;
			}

		}


		[It, Should(" delete a stored value or object. ")]
		public void Delete()
		{
			IDeletable obj = new Deletable() { Value = "test" };
			obj.Delete().ShouldBeTrue();
			obj.Delete().ShouldBeFalse();
		}
	}
}
