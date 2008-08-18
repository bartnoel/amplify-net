//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) CompanyName.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Data.SqlClientCe
{
	#region Using Statements
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using MbUnit.Framework;

	using Describe = MbUnit.Framework.TestsOnAttribute;
	using InContext = MbUnit.Framework.DescriptionAttribute;
	using It = MbUnit.Framework.TestAttribute;
	using Should = MbUnit.Framework.DescriptionAttribute;
	using By = MbUnit.Framework.AuthorAttribute;
	using Tag = MbUnit.Framework.CategoryAttribute;
	#endregion

	using System.Configuration;
	using System.Data;

	[
		Name("SqlCeAdapter"),
		Describe(typeof(Adapter)),
		InContext("should perform its specified behavor."),
		Tag("Functional"),
		By("Michael Herndon", "mherndon@opensourceconnections.com", "www.amptools.net")
	]
	public class AdapterObject : Spec
	{
		protected Adapter Adapter { get; set;}

		public override void  InvokeBeforeAll()
		{
 			base.InvokeBeforeAll();
			this.Adapter = Data.Adapter.Add(ConfigurationManager.ConnectionStrings["ce_test"]); 
		}


		[It, Should(" select values from the database. ")]
		public void SelectAll()
		{
			Dictionary<Guid, string> values = new Dictionary<Guid,string>();
			using(IDataReader dr = this.Adapter.Select("SELECT * FROM [SELECT]")) {
				
				while(dr.Read()) {
					values.Add(dr.GetGuid(0), dr.GetString(1));
				}
			}

			values.Count.ShouldBe(4);
			values.Values.First().ShouldBe("Value1");
		}

		
	}
}
