using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.ActiveRecord
{
	public interface IChild
	{
		IParent Parent { get; }
		void SetParent(IParent parent);
	}
}
