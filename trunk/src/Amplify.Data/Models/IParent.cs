using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Model
{
	public interface IParent
	{
		bool IsParent { get; }
		void RemoveChild(object child);
	}
}
