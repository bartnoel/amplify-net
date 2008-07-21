using System;
using System.Collections.Generic;

using System.Text;

namespace Amplify.Data
{
	public interface ITableDescriptor
	{
		string Name { get; set; }
		IEnumerable<IColumnDescriptor> Columns { get; }
		IEnumerable<IColumnDescriptor> PrimaryKeys { get; }
		void AddColumn(IColumnDescriptor column);
		void RemoveColumn(IColumnDescriptor column);
	}
}
