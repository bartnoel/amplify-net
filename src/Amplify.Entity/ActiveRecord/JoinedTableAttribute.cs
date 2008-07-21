using System;
using System.Collections.Generic;

using System.Text;

namespace Amplify.ActiveRecord
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class JoinedTable : TableAttribute
	{
		
		public string ForeignKey
		{
			get;
			set;
		}

		public override bool IsPrimary
		{
			get
			{
				return false;
			}
		}
	}
}
