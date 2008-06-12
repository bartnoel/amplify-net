using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify
{
	public class Range
	{
		public int Maximum { get; set; }
		public int Minimum { get; set; }

		public Range()
		{
			this.Minimum = int.MinValue;
			this.Maximum = int.MaxValue;
		}

		public Range(int minimum, int maximum)
		{
			this.Minimum = minimum;
			this.Maximum = maximum;
		}
	}
}
