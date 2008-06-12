using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Amplify.Data.Mappings
{
	public interface IDataIndex
	{
		string IndexName { get; set; }
		string EntityName { get; set; }
		bool IsUnique { get; set; }
		IEnumerable<string> ColumnNames { get; set; }
		ListSortDirection Direction { get; set; }
	}
}
