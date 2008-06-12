using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MbUnit.Framework
{

	[TestFixture] 
	public class Spec : IDecorated
	{
		private Dictionary<string, object> properties = new Dictionary<string, object>();


		protected string AssemblyLocation
		{
			get {
				return  Path.GetDirectoryName(this.GetType().Assembly.Location);
			}
		}

		public object this[string propertyName]
		{
			get { 
				if(this.properties.ContainsKey(propertyName))
					return this.properties[propertyName];
				return null;
			}
			set { this.properties[propertyName] = value; }
		}

		[FixtureSetUp]
		public virtual void BeforeAll()
		{

		}

		[SetUp] 
		public virtual void BeforeEach()
		{

		}


		[TearDown]
		public virtual void AfterEach()
		{

		}

		[FixtureTearDown]
		public virtual void AfterAll()
		{

		}
	}
}
