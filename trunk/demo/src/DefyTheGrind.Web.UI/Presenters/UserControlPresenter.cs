using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefyTheGrind.Web.UI.Presenters
{
	public class UserControlPresenter<V> : Presenter<V> 
		where V : Views.IUserControlView
	{

		public UserControlPresenter(V view)
		{
			this.View = view;
			this.View.Load += new EventHandler(Load);
			this.View.Unload += new EventHandler(Unload);
		}

		protected virtual void Unload(object sender, EventArgs e)
		{
			
		}

		protected virtual void Load(object sender, EventArgs e)
		{
			
		}
	}
}
