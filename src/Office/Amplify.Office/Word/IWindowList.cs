using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify.Office.Word
{
	public interface IWindowList : IList<IWindow>, IDisposable 
	{
		IWindow FindByIndex(int index);
	}
}
