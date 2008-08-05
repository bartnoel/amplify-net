using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefyTheGrind.Web.UI.Views
{
	public interface IDataBoundView : IUserControlView 
	{
		Csla.Web.CslaDataSource DataSource { get; set; }
		event EventHandler UpdateView;
	}
}
