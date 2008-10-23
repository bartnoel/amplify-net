//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) CompanyName.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using MbUnit.Framework;
	using Gallio.Framework;

	[
		Describe(typeof(Adapter)),
		InContext("should handle the quoting of values."),
		Tag(Tags.Unit),
		By("Michael Herndon", "mherndon@opensourceconnections.com", "www.amptools.net")
	]
	public class AdapterQuoting_Specification : Spec
	{
		[It, Should(" quote values appropiately. ")]
		public void QuoteValues()
		{
			Adapter adapter = Adapter.Get();
			adapter.Quote("test").ShouldBe("'test'");

			Guid id = Guid.NewGuid();
			adapter.Quote(id).ShouldBe("'" + id.ToString().ToLowerInvariant() + "'");
			adapter.Quote(true).ShouldBe("1");
			adapter.Quote(false, DbType.String).ShouldBe("false");
			adapter.Quote(10).ShouldBe("'10'");
			adapter.QuoteColumnName("name").ShouldBe("[name]");
			adapter.QuoteTableName("table").ShouldBe("[table]");
		}
	}
}
