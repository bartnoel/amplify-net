using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data
{
	public static class MetaDataRegistry
	{
		private static readonly ServiceRegistry registry = new ServiceRegistry();

		public static void Add(Type type, IRelationalMetaData relationalMetaData)
		{
			registry.Add(type.FullName, relationalMetaData);
		}

		public static IRelationalMetaData  Get(Type type)
		{
			return (registry.GetService(type.FullName) as IRelationalMetaData);
		}

		public static bool Remove(Type type)
		{
			return registry.Remove(type.FullName);
		}

	}
}
