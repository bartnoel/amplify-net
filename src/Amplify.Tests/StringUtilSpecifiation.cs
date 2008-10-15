//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) CompanyName.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify
{
	#region Using Statements
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;

	using MbUnit.Framework;
	using Gallio.Framework;


	using Describe = MbUnit.Framework.TestsOnAttribute;
	using InContext = MbUnit.Framework.DescriptionAttribute;
	using It = MbUnit.Framework.TestAttribute;
	using Should = MbUnit.Framework.DescriptionAttribute;
	using By = MbUnit.Framework.AuthorAttribute;
	using Tag = MbUnit.Framework.CategoryAttribute;
	
	
	#endregion

	[
		Describe(typeof(StringUtil)),
		InContext(" manipulation of string values. "),
		Tag("Unit"),
		By("Michael Herndon", "mherndon@opensourceconnections.com", "www.amptools.net")
	]
	public class StringUtilSpecifiation : Spec
	{

		[It, Should("grep and replace the regular expression with the value given")]
		public void Gsub()
		{
			StringUtil.Gsub("test this app", "test", "").ShouldBe(" this app");
			StringUtil.Gsub("TEST this app", "test", "", RegexOptions.IgnoreCase).ShouldBe(" this app");
			StringUtil.Gsub("test this app", "test", item => { return ""; }).ShouldBe(" this app");
			StringUtil.Gsub("TEST this app", "test", item => { return ""; }, RegexOptions.IgnoreCase).ShouldBe(" this app");
		}

		[It, Should("return the match from string")]
		public void Match()
		{
			StringUtil.Match("test this app", "test").Success.ShouldBeTrue();
			StringUtil.Match("TEST this app", "test", RegexOptions.IgnoreCase).Success.ShouldBeTrue();
		}


		[It, Should("determine if there was a match found")]
		public void IsMatch()
		{
			StringUtil.IsMatch("test this app", "test").ShouldBeTrue();
			StringUtil.IsMatch("TEST this app", "test").ShouldBeFalse();
			StringUtil.IsMatch("TEST this app", "test", RegexOptions.IgnoreCase).ShouldBeTrue();
		}

		[It, Should(" convert the string to an integer. ")]
		public void ToInt()
		{
			StringUtil.ToInt("123").ShouldBe(123);
			StringUtil.ToInt("hi").ShouldBe(0);
		}

		[It, Should(" conver the string to a date. ")]
		public void ToDateTime()
		{
			DateTime date = StringUtil.ToDateTime("12/08/2009 10:12:00 AM");
			date.Month.ShouldBe(12);
			date.Day.ShouldBe(8);
			date.Year.ShouldBe(2009);
			date.Hour.ShouldBe(10);
			date.Minute.ShouldBe(12);
			date.Millisecond.ShouldBe(0);
		}


		[It, Should(" convert the string to the date. ")]
		public void ToDate()
		{
			DateTime date = StringUtil.ToDateTime("12/08/2009 10:12:00 AM");
			date.Month.ShouldBe(12);
			date.Day.ShouldBe(8);
			date.Year.ShouldBe(2009);
		}

		[It, Should(" convert the datetime string to a timespan. ")]
		public void ToTime()
		{
			TimeSpan span = StringUtil.ToTime("12/08/2009 10:12:00 AM");
			span.Hours.ShouldBe(10);
			span.Minutes.ShouldBe(12);
		}

		[It, Should(" trim the string, using a string delimiter. ")]
		public void Trim()
		{
			StringUtil.Trim("huh", "h").ShouldBe("u");
		}
		
		[It, Should(" trim the start of the string, using a string delimiter. ")]
		public void TrimStart()
		{
			StringUtil.TrimStart("huh,", ",").ShouldBe("huh,");
			StringUtil.TrimStart("huh,", "h").ShouldBe("uh,");
		}

		[It, Should(" trim the end of the string, using a string delimiter. ")]
		public void TrimEnd()
		{
			StringUtil.TrimEnd("huh,", ",").ShouldBe("huh");
			StringUtil.TrimEnd("huh,", "h").ShouldBe("huh,");
			StringUtil.TrimEnd("huh,", "h,").ShouldBe("hu");
		}

		

		[It, Should(" split the string, using a string delimiter. ")]
		public void Split()
		{ 
			StringUtil.Split("test, value1, value2", ", ").Contains("test").ShouldBeTrue();
		}


		[It, Should(" iterate over the string and apply a handler to each segment. " )]
		public void Each()
		{
			string values = "";
			StringUtil.Each("a,a,a,a,a", ",", o => values += o + "b");

			values.ShouldBe("ababababab");
		}

	}
}
