using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Amplify.ComponentModel;

namespace Amplify.Models.Validation
{
	public static class ValidationRegistry
	{
		private static readonly ServiceRegistry registry = new ServiceRegistry();


		public static void Add(Type type, ValidationRules rules) 
		{
			registry.Add(type.ToString(), rules);
		}

		public static ValidationRules Get(Type type)
		{
			return (ValidationRules)registry.GetService(type);
		}

		public static void Remove(Type type)
		{
			registry.Remove(type);
		}
	}
}
