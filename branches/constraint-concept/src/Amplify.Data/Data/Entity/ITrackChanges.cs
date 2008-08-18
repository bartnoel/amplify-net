using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data.Entity
{
	public interface ITrackChanges
	{
		bool IsModified { get; }
		bool IsMarkedForDeletion { get; }
		bool IsNew { get; }
		bool IsValid { get; }
		bool IsEntityValid { get; }
		bool IsEntityModified { get; }
	}
}
