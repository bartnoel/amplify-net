//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace DefyTheGrind.Models
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using Amplify.ObjectModel;

	public class DateTimeNowFactory : ValueFactory 
	{
		public override object CreateValue()
		{
			return DateTime.Now;
		}
	}
}
