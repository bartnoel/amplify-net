using System;
using System.Collections.Generic;

using System.Text;

namespace Amplify.Data
{
	public interface IColumnDescriptor
	{
		bool IsPrimaryKey { get; set; }
		bool IsIndex { get; set; }
		string Name { get; set; }
		string Type { get; set; }
		object Default { get; set; }
		int? Limit { get; set; }
		int? Percision { get; set; }
		int? Scale { get; set; }
		bool Null { get; set; }
		string Table { get; set; }
		string PropertyName { get; set; }
		object DefaultValue { get; set; }
	}
}
