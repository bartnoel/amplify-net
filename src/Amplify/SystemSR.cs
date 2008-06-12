//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	/// <summary>
	/// This class is designed to get resources for the System.dll
	/// </summary>
	public class SystemSR : SR
	{

		/// <summary>
		/// Default Constructor for the class.
		/// </summary>
		public SystemSR()
		{
			type = typeof(System.Uri);
			SRtype = this.GetType();
			this.BaseName = "System";
			this.CreateResources();
		}
	}
}
