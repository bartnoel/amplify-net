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
		Describe(typeof(IUniqueIdentifier)),
		InContext(" a basic example of how to use the IUniqueIdentifier contract ."),
		Tag(Tags.Instructional),
		By("Michael Herndon", "mherndon@opensourceconnections.com", "www.amptools.net")
	]
	public class IUniqueIdentifier_Specification : Spec
	{
		public class Unique : IUniqueIdentifier<Guid>
		{
			private Guid id = Guid.NewGuid();

			#region IUniqueIdentifier<Guid> Members

			Guid IUniqueIdentifier<Guid>.Id
			{
				get { return this.id; }
			}

			#endregion

			#region IUniqueIdentifier Members

			object IUniqueIdentifier.Id
			{
				get { return this.id; }
			}

			#endregion
		}


		[It, Should(" have a public default constructor. ")]
		public void CompareIdentities()
		{
			IUniqueIdentifier<Guid> item = (IUniqueIdentifier<Guid>) new Unique();
			IUniqueIdentifier<Guid> item2 = (IUniqueIdentifier<Guid>) new Unique();
			item.Id.ShouldNotBe(item2.Id);

			((IUniqueIdentifier)item).Id.ShouldNotBe(item2.Id);
		}
	}
}
