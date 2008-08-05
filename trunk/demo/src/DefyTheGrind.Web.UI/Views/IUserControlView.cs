

namespace DefyTheGrind.Web.UI.Views
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Web.UI;

	public interface IUserControlView : IView
	{
		event EventHandler Init;
		event EventHandler Load;
		event EventHandler PreRender;
		event EventHandler Unload;
		string ID { get; }
		string ClientID { get; }
		void DataBind();
		bool IsPostback { get; }
		ControlCollection Controls { get; }
		bool IsValid { get; }
		void Validate();
		void Validate(string validationGroup);
	}
}
