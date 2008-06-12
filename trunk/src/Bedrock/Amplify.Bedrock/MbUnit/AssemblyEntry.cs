using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MbUnit.Framework
{
	[AssemblyFixture] 
	public class AssemblySpecificationConfiguration
	{
		[FixtureSetUp]
		public virtual void BeforeRun()
		{

		}


		[SetUp]
		public virtual void BeforeEachSpecification()
		{

		}

		[TearDown]
		public virtual void AfterEachSpecification()
		{

		}

		[FixtureTearDown]
		public virtual void AfterRun()
		{

		}
	}
}
