using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify.Office.Word
{
	public interface IDocument: IDisposable 
	{
		string Name { get; } 
		void Close(CloseArgs args);
		void Close();
		void Save();
		void SaveAs(SaveAsArgs args);
	}
}
