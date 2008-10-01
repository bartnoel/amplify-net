using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data
{
	public class PrimaryKeyConstraint : KeyConstraint
	{
		private string name;


		public override string Prefix
		{
			get { return "PK"; }
		}

		public override string Name
		{
			get
			{
				if (string.IsNullOrEmpty(this.name))
					name = string.Format("{0}_{1}", this.Prefix, this.TableName);
				return name;
			}
			set { this.name = value; }
		}
	}
}
