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

	public class Observer : IService
	{
		private List<IListener> listeners = new List<IListener>();

		/// <summary>
		/// Subscribes the specified observer.
		/// </summary>
		/// <param name="observer">The observer which is interfaced with <see cref="Entry7.IObserver"/>.</param>
		public void Subscribe(IListener listener)
		{
			this.listeners.Add(listener);
		}

		/// <summary>
		/// Unsubscribes the specified observer.
		/// </summary>
		/// <param name="observer">The observer which is interfaced with <see cref="Entry7.IObserver"/>.</param>
		public void Unsubscribe(IListener listener)
		{
			this.listeners.Remove(listener);
		}

		/// <summary>Notifies the observers. </summary>
		/// <param name="notification">The notification object to send to all subscribed observers.</param>
		public void NotifyListeners(object notification)
		{
			foreach (IListener listener in listeners)
				if (listener.IsListening)
					listener.Listen(notification);
		}

		
	}
}
