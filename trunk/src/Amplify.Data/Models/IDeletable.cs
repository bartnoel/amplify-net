

namespace Amplify.Model
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Text;

	public interface IDeletable
	{
		bool IsDeletable { get; }
		bool Delete();
		bool Delete(IDbTransaction transaction);
	}
}
