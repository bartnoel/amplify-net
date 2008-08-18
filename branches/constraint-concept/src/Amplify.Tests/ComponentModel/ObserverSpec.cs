//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.ComponentModel
{
	#region Using Statements
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using Gallio.Framework;
	using MbUnit.Framework;

	using It = MbUnit.Framework.TestAttribute;
	using Describe = MbUnit.Framework.CategoryAttribute;
	using InContext = MbUnit.Framework.DescriptionAttribute;
	using Should = MbUnit.Framework.DescriptionAttribute;
	#endregion

	[
		Describe("Observer Specification"),
		InContext("notifying listeners of changes."),
		Author("Michael Herndon", "mherndon@opensourceconnections.com", "opensourceconnections.com"),
	]
	public class ObserverObject : Spec
	{
		private Observer observer = new Observer();
		

	
		public override void InvokeBeforeEach()
		{
			this.observer = new Observer();
		}

		[It, Should("notify all subscribers that are listening.")]
		public void NotifySubscribers()
		{
			Listener listenerOne = new Listener(), listenerTwo = new Listener(),
				listenerThree = new Listener();

			this.observer.Subscribe(listenerOne);
			this.observer.Subscribe(listenerTwo);

			this.observer.NotifyListeners("message");
			listenerOne.Notification.ShouldBe("message");
			listenerTwo.Notification.ShouldBe("message");
			listenerThree.Notification.ShouldBeNull();

			listenerTwo.IsListening = false;

			this.observer.NotifyListeners("hi");
			listenerOne.Notification.ShouldBe("hi");
			listenerTwo.Notification.ShouldBe("message");

			this.observer.Subscribe(listenerThree);
			this.observer.Unsubscribe(listenerOne);
			this.observer.NotifyListeners("lastMessage");
			listenerOne.Notification.ShouldBe("hi");
			listenerTwo.Notification.ShouldBe("message");
			listenerThree.Notification.ShouldBe("lastMessage");

		}
		
	}

	public class Listener : IListener
	{
		public Listener()
		{
			this.IsListening = true;
		}
		public object Notification { get; private set; }

		#region IListener Members

		public void Listen(object notification)
		{
			this.Notification = notification;
		}

		public bool IsListening { get; internal set; }

		#endregion
	}
}
