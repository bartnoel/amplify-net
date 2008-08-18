using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify.ObjectModel
{
	public interface ISaveable<T>
	{
		T Save();
	}
}
