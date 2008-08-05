using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DefyTheGrind.Web.UI.Views
{
	using WebControls;

	public interface IFormViewOnUserControlView : IDataBoundView
	{
		IFormView FormView { get; }
		
	}
}
