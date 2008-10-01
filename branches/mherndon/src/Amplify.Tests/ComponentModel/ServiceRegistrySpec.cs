//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Amplify.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.ComponentModel
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
		Describe("ServiceRegistry Specification"),
		InContext("interacting with the service registry")
	]
	public class ServiceRegistryObject : Spec 
	{

		private ServiceRegistry registry;

		public override void  InvokeBeforeEach()
		{
			this.registry = new ServiceRegistry();
		}

		[It, Should("only add objects that implement the IService Interface.")]
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
			//this.registry.GetService(typeof(object)).ShouldBeNull();
			this.registry.GetService(typeof(Service)).ShouldNotBeNull();
		}

		[It, Should("remove the specified service.")]
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

	
}

namespace Amplify
{

	public class Service : Amplify.ComponentModel.IService
	{

	}
}