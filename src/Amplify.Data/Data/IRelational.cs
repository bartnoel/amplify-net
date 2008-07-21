using System;
using System.Collections.Generic;

using System.Text;

namespace Amplify.Data
{
	public interface IRelational : IDecoratedObject 
	{
		IEnumerable<ITableEntityDescriptor> Tables { get; }
		IEnumerable<AssociationAttribute> Assocations { get; }
	}

	public delegate IRelational CreateEntityCallback();
}
