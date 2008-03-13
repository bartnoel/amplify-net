using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify
{
	public interface IDeletable<T>
	{
		T Delete();
	}
}
