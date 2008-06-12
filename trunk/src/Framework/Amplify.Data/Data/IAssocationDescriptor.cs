using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace Amplify.Data
{
	public interface IAssocationDescriptor
	{
		AssocationType Type { get; set; }
		string PropertyName { get; set; }
		Type PropertyType { get; set; }
		string Through { get; set; }
		IEnumerable<object> Conditions { get; set; }
		string Order { get; set; }
		string ForeignKey { get; set; }
		IDictionary Include { get; set; }
		bool Polymorphic { get; set; }
	}

}
