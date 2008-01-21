//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.ActiveRecord
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	public interface IUnitOfWork : ISaveable, IDeletable 
	{
		bool IsNew { get; }
		bool IsModified { get; }
		bool IsValid { get; }
		bool IsDeletable { get; }
		bool IsSaveable { get; }
		object Save();
		bool Delete();
	}
}
