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
		Describe("PropertyMapper Specification"),
		InContext("mapping values to and from different objects"),
		Author("Michael Herndon", "mherndon@opensourceconnections.com", "opensourceconnections.com"),
	]
	public class PropertyMapperObject : Spec
	{
		[It, Should("")] 
		public void Run()
		{

		}
	}
}
