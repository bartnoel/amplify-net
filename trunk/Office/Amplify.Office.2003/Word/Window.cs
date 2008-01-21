

namespace Amplify.Office.v2003.Word
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	
	using WI = Microsoft.Office.Interop.Word;

	public class Window : Amplify.Office.Word.IWindow 
	{
		private WI.Window window;

		public Window(WI.Window window)
		{
			this.window = window;
		}

		public int Index
		{
			get { return this.window.Index; }
		}

		#region IDisposable Members


		public void Close()
		{
			this.Close(Type.Missing, Type.Missing);
		}

		public void Close(object saveChanges, object routeDocument)
		{
			object save = saveChanges,
				   route = routeDocument;
			this.window.Close(ref save, ref route);
		}

		public void Dispose()
		{
			this.Close();
		}

		#endregion
	}
}
