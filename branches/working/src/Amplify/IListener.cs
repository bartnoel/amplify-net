//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	/// <summary>
	///	 Supplys a contract that enforces an object to behave as an observer and recieve
	///  messages from an <see cref="T:Entry7.ObservationBroadcaster" /> object.
	/// </summary>
	public interface IListener
	{

		/// <summary> Updates the observer with the specified notification. </summary>
		/// <param name="notification">The notification object.</param>
		void Listen(object notification);

		/// <summary> Gets a value indicating whether this instance is listening to the <see cref="T:Entry7.ObservationBroadcaster" /> . </summary>
		/// <value>
		/// 	<c>true</c> if this instance is listening; otherwise, <c>false</c>.
		/// </value>
		bool IsListening { get; }
	}
}
