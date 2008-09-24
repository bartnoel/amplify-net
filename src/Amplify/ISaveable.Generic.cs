using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify
{
	public interface ISaveable<T>
	{
		T Save();
	}
}
