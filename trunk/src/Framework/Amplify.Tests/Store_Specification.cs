using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gallio.Framework;
using MbUnit.Framework;
using NBehave.Spec.MbUnit;
using NBehave.Specs;
using NBehave;

using Context = MbUnit.Framework.TestFixtureAttribute;
using SpecifiesThat = MbUnit.Framework.TestAttribute;
using Concern = MbUnit.Framework.CategoryAttribute;
using Describe = MbUnit.Framework.DescriptionAttribute;
using It = MbUnit.Framework.DescriptionAttribute;


namespace Amplify
{
	[Context, Concern("Functional"), Author("Michael Herndon", "mherndon@opensourceconnections.com", "opensourceconnections.com"),
	Description("A string object store for key pair values")]
	public class StoreSpecification : NBehave.Spec.MbUnit.MbUnitSpecBase
	{
		private Store store;

		protected override void Before_each_spec()
		{
			this.store = new Store();
			base.Before_each_spec();
		}

		[SpecifiesThat, It("should have a constructor that allows pair values.")]
		public void Should_Initialize_With_Paired_Values()
		{
			this.store = new Store("Name", "Michael", "Age", 10);
			this.store.ShouldContainKey("Name");
			this.store.ShouldContainKey("Age");
			this.store["Name"].ShouldBe("Michael");
			this.store["Age"].ShouldBe(10);
		}

		[SpecifiesThat, It("should have an empty constructor.")]
		public void Should_Initialize_With_Default_Constructor()
		{
			this.store = new Store();
			this.store.ShouldNotBeNull();
		}

		[SpecifiesThat, It("should have an indexed property that has a string key.")]
		public void Should_Have_Indexer()
		{
			this.store.Add("Test", "test");
			this.store["Test"].ShouldBe("test");
		}
	}
}
