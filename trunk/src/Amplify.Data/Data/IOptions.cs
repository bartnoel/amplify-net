using System;
using System.Collections.Generic;

using System.Text;

namespace Amplify.Data
{
	using Amplify.Linq;

	public interface IOptions
	{
		object[] Conditions { get; set; }
		string Select { get; set; }
		bool IsDistinct { get; set; }
		string Group { get; set; }
		string Order { get; set; }
		int? Limit { get; set; }
		int? Offset { get; set; }
		bool ReadOnly { get; set; }
		string Join { get; set; }
		Hash Include { get; set; }
		string As { get; set; }
		string From { get; set; }
		string Where { get; set; }
	}
	
}
