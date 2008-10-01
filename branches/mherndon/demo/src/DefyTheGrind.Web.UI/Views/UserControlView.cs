using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefyTheGrind.Web.UI.Views
{
	public class UserControlView : System.Web.UI.UserControl, IUserControlView 
	{


		#region IUserControlView Members


		public bool IsPostback
		{
			get { return this.Page.IsPostBack; }
		}

		#endregion

		#region IUserControlView Members

		public bool IsValid { get { return this.Page.IsValid; } }

		public void Validate()
		{
			this.Page.Validate();
		}

		public void Validate(string validationGroup)
		{
			this.Page.Validate(validationGroup);
		}

		#endregion
	}
}
