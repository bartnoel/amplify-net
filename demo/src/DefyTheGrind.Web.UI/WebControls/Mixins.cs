

namespace DefyTheGrind.Web.UI.WebControls
{
	using System;
	using System.Collections.Generic;
	using System.Collections;
	using System.Linq;
	using System.Text;
	using System.Web.UI.WebControls;
	

	public static class Mixins
	{

		public static void ResolveCssAdapter(this WebControl obj, Type adapterType)
		{
			IDictionary adapters = obj.Page.Request.Browser.Adapters;
			if (adapters != null && adapters[obj.GetType().AssemblyQualifiedName] == null)
				adapters.Add(obj.GetType().AssemblyQualifiedName, adapterType.ToString());
		}
	}
}
