//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) CompanyName.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MbUnit.Framework
{
	using System;

	public enum Tags
	{
		Functional,
		Unit, 
		Instructional,
		Prototype
	}

	public class DescribeAttribute : MbUnit.Framework.TestsOnAttribute
	{
		public DescribeAttribute(string typeName)
			:base(typeName)
		{

		}

		public DescribeAttribute(Type type)
			: base(type)
		{

		}
	}

	public class InContextAttribute : MbUnit.Framework.DescriptionAttribute
	{

		public InContextAttribute(string description)
			: base(description)
		{

		}
	}

	public class ItAttribute : MbUnit.Framework.TestAttribute
	{
		public ItAttribute()
			: base()
		{

		}

	}

	public class ShouldAttribute : MbUnit.Framework.DescriptionAttribute
	{
		public ShouldAttribute(string description)
			: base(description)
		{

		}
	}

	public class ByAttribute : MbUnit.Framework.AuthorAttribute
	{
		public ByAttribute(string authorName)
			: base(authorName)
		{

		}

		public ByAttribute(string authorName, string authorEmail)
			: base(authorName, authorEmail)
		{

		}

		public ByAttribute(string authorName, string authorEmail, string authorWebsite)
			: base(authorName, authorEmail, authorWebsite)
		{

		}
	}

	public class TagAttribute : MbUnit.Framework.CategoryAttribute
	{

		public TagAttribute(Tags tag)
			: base(tag.ToString())
		{

		}

		public TagAttribute(string categoryName) : base(categoryName)
		{

		}

	}
}
