using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Amplify.Data;

namespace Amplify.Models
{
	public class AdoBase<T> : Base<T> where T: AdoBase<T>, new()
	{

		private static List<ITableEntityDescriptor> tables;
		private static List<IColumnDescriptor> columns;

		internal protected IEnumerable<ITableEntityDescriptor> Tables { get { return tables; } }
		internal protected IEnumerable<IColumnDescriptor> Columns { get { return columns; } }
	}
}
