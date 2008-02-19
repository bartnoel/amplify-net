using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.ActiveRecord.Data
{
	public interface  IOptions
	{
		Clause Conditions { get; set; }
		Clause OrderBy { get; set; }
		IEnumerable<Clause> Joins { get; set; }
		GroupBy GroupBy { get; set; }
		bool IsDistinct { get; set; }
		/*string Having { get; set; }*/
		int Limit { get; set; }
		int Offset { get; set; } 
	}
}
