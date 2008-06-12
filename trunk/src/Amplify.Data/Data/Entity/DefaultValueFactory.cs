using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data.Entity
{
	public abstract class DefaultValueFactory
	{
		public abstract object DefaultValue { get; }

		public abstract object CreateDefaultValue(IPropertyInfo info);
	}
}
