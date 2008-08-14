using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify.ObjectModel
{
	public interface IUnitOfWork : ISaveable, IDeletable 
	{
		bool IsNew { get; }
		bool IsModified { get; }
		bool IsDeleted { get; }
		bool IsValid { get; }
		bool IsSaveable { get; }
	}
}
