//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify
{
	#region Using Statements
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using Gallio.Framework;
	using MbUnit.Framework;

	using It = MbUnit.Framework.TestAttribute;
	using Describe = MbUnit.Framework.CategoryAttribute;
	using InContext = MbUnit.Framework.DescriptionAttribute;
	using Should = MbUnit.Framework.DescriptionAttribute;
	#endregion


	[
		Author("Michael Herndon", "mherndon@opensourceconnections.com", "opensourceconnections.com"),
		Describe("Store Specification"),
		InContext("manipulating the store by adding, removing, changing objects")
	]
	public class StoreObject : Spec
	{
		private Store store;

		public override void InvokeBeforeEach()
		{
			this.store = new Store();
		}

		[It, Should("have a constructor that allows pair values.")]
		public void InitializeStoreAndValidateValues()
		{
			this.store = new Store("Name", "Michael", "Age", 10);
			this.store.ShouldContainKey("Name");
			this.store.ShouldContainKey("Age");
			this.store["Name"].ShouldBe("Michael");
			this.store["Age"].ShouldBe(10);
		}

		[It, Should("have an empty constructor.")]
		public void InitializeStore()
		{
			this.store = new Store();
			this.store.ShouldNotBeNull();
		}

		[It, Should("have an indexed property that has a string key.")]
		public void CheckForIndexedProperty()
		{
			this.store.Add("Test", "test");
			this.store["Test"].ShouldBe("test");
		}
	}
}
