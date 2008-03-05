using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify
{ 
	public interface IDecoratedObject
	{
		object this[string propertyName] { get; set; }
	}
}
