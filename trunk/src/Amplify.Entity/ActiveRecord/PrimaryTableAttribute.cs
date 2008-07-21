//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.ActiveRecord
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class PrimaryTableAttribute : TableAttribute
	{
		public override bool IsPrimary
		{
			get
			{
				return true;
			}

		}
	}
}
