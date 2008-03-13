using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data
{
	public interface ITableEntityDescriptor : ITableDescriptor 
	{
		bool IsReadOnly { get; set; }
		bool AllowUpdates { get; set; }
		bool AllowInserts { get; set; }
		bool AllowDeletes { get; set; }
		bool IsPrimary { get; set; }
	}
}
