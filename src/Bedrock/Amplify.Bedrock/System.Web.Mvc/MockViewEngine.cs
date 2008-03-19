using System;

namespace System.Web.Mvc
{
	public class MockController : IViewEngine
	{

		protected ViewContext ViewContext { get; private set; }

		
		#region IViewEngine Members

		public void RenderView(ViewContext viewContext)
		{
			this.ViewContext = viewContext;
		}

		#endregion

	}
}