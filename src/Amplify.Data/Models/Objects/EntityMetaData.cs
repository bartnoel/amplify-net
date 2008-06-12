using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Objects;

namespace Amplify.Models.Objects
{
	public class EntityMetaData
	{
		public Type Type { get; set; }
		public string EntitySetName { get; set; }

		public static EntityMetaData Create<T>(ObjectContext context) where T: EntityBase<T> 
		{

		}
	}
}
