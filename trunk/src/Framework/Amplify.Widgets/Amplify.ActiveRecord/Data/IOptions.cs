using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data
{
	public interface IOptions
	{
		List<object> Conditions { get; set; }
		string Select { get; set; }
		bool Distinct { get; set; }
		string Group { get; set; }
		string Order { get; set; }
		int? Limit { get; set; }
		int? Offset { get; set; }
		bool ReadOnly { get; set; }
		string Join { get; set; }
		IDictionary<string, object> Include { get; set; }
		string From { get; set; }
		string Where { get; set; }
	}
	
}
