using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data.Mappings
{
	public interface IDataAttribute
	{
		bool IsIdentifier { get; set; }
		string AttributeName { get; set; }
		string AttributeType { get; set; }
		string AttributeDefault { get; set; }
		string EntityName { get; set; }
		bool IsNullable { get; set; }
		int? Limit { get; set; }
		int? Percision { get; set; }
		int? Scale { get; set; }
	}
}
