//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MbUnit.Framework
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;

	[Metadata("Spec", "for derived tests."), TestFixture]
	public abstract class Spec 
	{

		protected string AssemblyLocation
		{
			get {
				return Path.GetDirectoryName(this.GetType().Assembly.Location); 
			}
		}

		[FixtureSetUp]
		public virtual void InvokeBeforeAll()
		{

		}

		[FixtureTearDown]
		public virtual void InvokeAfterAll()
		{

		}

		[SetUp] 
		public virtual void InvokeBeforeEach()
		{
			
		}

		[TearDown]
		public virtual void InvokeAfterEach()
		{

		}

	}
}
