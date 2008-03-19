using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gallio.Framework;
using MbUnit.Framework;
using NBehave.Spec.MbUnit;
using NBehave.Specs;
using NBehave;

using Context = MbUnit.Framework.TestFixtureAttribute;
using SpecifiesThat = MbUnit.Framework.TestAttribute;
using Concern = MbUnit.Framework.CategoryAttribute;
using Describe = MbUnit.Framework.DescriptionAttribute;
using It = MbUnit.Framework.DescriptionAttribute;


namespace Amplify
{
	[Context, Concern("Functional"), Author("Michael Herndon", "mherndon@opensourceconnections.com", "opensourceconnections.com"),
	Description("describe")]
	public class ObserverSpecification : NBehave.Spec.MbUnit.MbUnitSpecBase
	{
		private Observer observer = new Observer();

		protected override void Before_each_spec()
		{
			base.Before_each_spec();
			this.observer = new Observer();
		}

		[SpecifiesThat, It("should notify all subscribers that are listening.")]
		public void Should_Notify_All_Listeners()
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
