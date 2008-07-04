using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.ActiveRecord
{
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
