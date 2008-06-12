using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data
{
	public interface IRelationalMetaData : IService 
	{
		IEnumerable<ITableEntityDescriptor> Tables { get; }
		Validation.ValidationRules ValidationRules { get; }
	}
}
