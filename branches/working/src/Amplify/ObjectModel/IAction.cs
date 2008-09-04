using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify.ObjectModel
{
	public interface IAction
	{
		void Do<T>(Action<T> action);
	}
}
