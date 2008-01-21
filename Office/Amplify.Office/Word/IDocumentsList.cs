

namespace Amplify.Office.Word
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	public interface IDocumentsList : IList<IDocument>, IDisposable 
	{
		IDocument this[string name] { get; }
		IDocument Open(string file);
		IDocument Open(string file, bool isReadonly, bool isVisible);
		IDocument Open(OpenArgs args);
	}
}
