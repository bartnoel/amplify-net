using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Web.UI.WebControls
{
	public class ViewControl<T> : System.Web.UI.UserControl
	{

		public T ViewData 
		{
			get {
				return this.GetViewData();
			}
		}

		protected virtual T GetViewData()
		{
			return null;
		}

	}
}
