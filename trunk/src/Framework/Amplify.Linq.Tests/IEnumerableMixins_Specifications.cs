//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Linq
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using MbUnit.Framework;

	[TestFixture]
	public class IEnumerableMixins_Specificiations
	{
		[Test]
		public void Expects_Each_To_Apply_Action()
		{ 
			List<object> stash = new List<object>();
			Hash.New(Name => "Michael", Age => 28).Each(pair => stash.Add(pair.Key));

			stash.Count.ShouldBe(2);
			stash[0].ShouldBe("Name");
		}

		[Test]
		public void Expects_Join_To_Concat_Strings()
		{
			string test = Hash.New(Name => "Michael", Age => 28).Join(", ", item => item.Value.ToString());
			test.ShouldBe("Michael, 28");
		}

		[Test]
		public void Expects_To_Find_Values_Included()
		{
			string[] columns = new[] { "Id", "News", "Description" };
			columns.Includes("Id", "News").ShouldBe(true);
		}

		[Test]
		public void Expects_Collect_To_Get_Matching_Values()
		{
			object[] values = new object [] { 10, "Test", "Michael", DateTime.Now, 45f };

			var collected = values.Collect(item => item.GetType() == typeof(String));
			collected.Count().ShouldBe(2);
		}

		[Test]
		public void Expects_Merge_To_Merge_Values()
		{
			Hash user = Hash.New(Name => "LL cool J", Career => "Singer", Location => "Cali");
			user.Merge(Hash.New(Career => "Actor", Movie => "Halloween"));

			user["Name"].ShouldBe("LL cool J");
			user["Career"].ShouldBe("Actor");
			user["Movie"].ShouldBe("Halloween");
		}

		[Test]
		public void Expects_IsEmpty_To_Determine_When_IEnumerable_Is_Empty()
		{
			Hash.New().IsEmpty().ShouldBe(true);
			Hash.New(Name => "Name").IsEmpty().ShouldBe(false);
		}
	}
}
