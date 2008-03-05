using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data
{
	public interface IExtendedUnitOfWork
	{
		void MarkOld();
		void MarkNew();
		void MarkReadOnly();
	}
}
