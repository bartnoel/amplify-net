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
	Description("ServiceRegistry Specs")]
	public class ServiceRegistrySpecification : NBehave.Spec.MbUnit.MbUnitSpecBase
	{

		private ServiceRegistry registry;

		protected override void Before_each_spec()
		{
			base.Before_each_spec();
			this.registry = new ServiceRegistry();
		}

		[SpecifiesThat, It("should only add objects that implement the IService Interface.")]
		public void Should_Add_Only_Services()
		{
			Service service = new Service();

			if (this.registry == null)
				this.registry = new ServiceRegistry();

			this.registry.Add(service);
			this.registry.GetService("Amplify.Service").ShouldNotBeNull();
			this.registry.GetService("Amplify.Service").ShouldBe(service);

			this.registry = new ServiceRegistry();

			// calling get with a type will automatically add the type 
			// to the registry if it impliments IService
			this.registry.GetService(typeof(object)).ShouldBeNull();
			this.registry.GetService(typeof(Service)).ShouldNotBeNull();
		}

		[SpecifiesThat, It("should remove the specified service.")]
		public void Should_Remove_A_Service()
		{
			Service service = new Service();
			this.registry = new ServiceRegistry();

			this.registry.Add(service);

			this.registry.GetService("Amplify.Service").ShouldNotBeNull();

			this.registry.Remove("Amplify.Service");
			this.registry.GetService("Amplify.Service").ShouldBeNull();
		}

	}

	public class Service : IService
	{

	}
}
