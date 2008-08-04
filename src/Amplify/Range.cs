using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify
{
	/// <summary>
	/// A simple range object that has a minimum and maximum <see cref="System.Int32"/> value.
	/// </summary>
	public class Range
	{
		/// <summary>
		/// Gets or sets the maximum value. 
		/// </summary>
		public IComparable Maximum { get; set; }

		/// <summary>
		/// Gets or sets the minimum value. 
		/// </summary>
		public IComparable Minimum { get; set; }

		/// <summary>
		/// Default Constructor that sets the minimum value to int.MinValue and the
		/// maximum to int.MaxValue.
		/// </summary>
		public Range()
		{
			this.Minimum = int.MinValue;
			this.Maximum = int.MaxValue;
		}

		/// <summary>
		/// Constructor that forces the Minimum and Maximum values to be set.
		/// </summary>
		/// <param name="minimum">The mimimum value.</param>
		/// <param name="maximum">The maximum value.</param>
		public Range(IComparable minimum, IComparable maximum)
		{
			this.Minimum = minimum;
			this.Maximum = maximum;
		}
	}
}
