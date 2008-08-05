using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefyTheGrind.Web.UI.Presenters
{


	public class Presenter<V> 
		where V : Views.IView 
	{

		public V View { get; set; }
	}
}
