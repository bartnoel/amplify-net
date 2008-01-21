using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify.Office.Word
{
	public interface IWindow : IDisposable 
	{
		int Index { get; }
		void Close(object save, object route);
		void Close();
	}
}
