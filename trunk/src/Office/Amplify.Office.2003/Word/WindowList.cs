

namespace Amplify.Office.v2003.Word
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	using WI = Microsoft.Office.Interop.Word;

	using Amplify.Office.Word;

	public class WindowList : List<IWindow>, IWindowList 
	{
		private WI.Application parent;

		public WindowList(WI.Application parent)
		{
			this.parent = parent;

			foreach (WI.Window window in parent.Windows)
				this.Add(new Window(window));
		}


		public IWindow FindByIndex(int index)
		{
			return Find(delegate(IWindow window) {
				return window.Index.Equals(index);
			});
		}

		#region IDisposable Members

		public void Dispose()
		{
			foreach (IWindow window in this)
				window.Dispose();
		}

		#endregion
	}
}
