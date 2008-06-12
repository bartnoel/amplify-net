using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify.Models
{
	public interface IDeletable<T>
	{
		T Delete();
	}
}
