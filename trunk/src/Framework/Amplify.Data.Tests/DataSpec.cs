
namespace MbUnit.Framework 
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using Amplify.Data;
	using MbUnit.Framework;

	[DependsOn(typeof(FixturesObject))]
	public abstract class DataSpec : Spec
	{

		internal protected static Adapter Adapter { get; set; }

		internal protected static Fixtures Runner { get; set; }

		public DataSpec()
		{
			this.Initialize();
		}

		protected void Initialize()
		{
			if (Runner == null)
				Runner = Fixtures.New(this.AssemblyLocation + "\\Fixtures", Adapter);
		}
	
	}
}
