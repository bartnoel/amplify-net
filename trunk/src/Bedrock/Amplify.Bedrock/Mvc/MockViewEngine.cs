using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace System.Web.Mvc
{
	public class MockViewEngine : IViewEngine  
	{  
		public ViewContext ViewContext { get; private set; }  
    
		public void RenderView(ViewContext viewContext)  
		{  
			ViewContext = viewContext;  
		}  
	}  
}
