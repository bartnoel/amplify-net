using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuse.Helpers
{
	public class Button : System.Windows.Controls.Button 
	{
		public string Uri
		{
			get;
			set;
		}


		protected override void OnClick()
		{
			if (!string.IsNullOrEmpty(this.Uri))
				Controllers.Dispatcher.Call(this.Uri);
			else
				base.OnClick();
		}
	}
}
