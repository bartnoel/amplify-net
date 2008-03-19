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
	Description("PropertyMapper Spec")]
	public class PropertyMapper_Specification : NBehave.Spec.MbUnit.MbUnitSpecBase
	{


		protected override void Before_each_spec()
		{
			
			base.Before_each_spec();
		}

		[SpecifiesThat, It("should .")]
		public void Should_Map_Properties_Of_Objects()
		{

		}


	}
}
