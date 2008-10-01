using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.ActiveRecord
{
	public interface IEntityRef
	{
		object Entity { get; }
		bool IsSet { get; }
	}
}
