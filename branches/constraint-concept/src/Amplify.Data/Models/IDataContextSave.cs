using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;

namespace Amplify.Models
{
	public interface IDataContextSave
	{
		void Save(DataContext context, bool attach);
		void MarkForDeletion();
	}
}
