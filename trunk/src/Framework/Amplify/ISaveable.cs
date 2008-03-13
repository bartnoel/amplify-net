using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify
{
	public interface ISaveable
	{
		bool IsSaveable { get; }
		object Save();	
	}
}
