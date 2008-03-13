using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data
{
	public interface IIndexDescriptor
	{
		string Name { get; set; }
		string TableName { get; set; }
		IEnumerable<string> Columns { get; set; }
	}
}
