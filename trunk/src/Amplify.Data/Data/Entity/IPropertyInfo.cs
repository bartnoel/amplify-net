using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data.Entity
{
	public interface IPropertyInfo
	{
		bool IsSealed { get; set; }
		bool IsReadOnly { get; set; }
		string PropertyName { get; set; }
		object DefaultValue { get; set; }
		Type PropertyType { get; set; }
	}
}
