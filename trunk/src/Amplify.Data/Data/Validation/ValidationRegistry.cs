using System;
using System.Collections.Generic;

using System.Text;

using Amplify.ComponentModel;

namespace Amplify.Data.Validation
{
	public static class ValidationRegistry
	{


		private static ServiceRegistry Registry
		{
			get {
				ServiceRegistry value = (ApplicationContext.Services.GetService("ValidationRegistry") as ServiceRegistry);
				if (value == null)
				{
					value = new ServiceRegistry();
					ApplicationContext.Services.Add("ValidationRegistry", value);
				}
				return value;
			}
		}


		public static void Add(Type type, ValidationRules rules) 
		{
			Registry.Add(type.FullName, rules);
		}

		public static ValidationRules Get(Type type)
		{
			return (ValidationRules)Registry.GetService(type.FullName);
		}

		public static bool Remove(Type type)
		{
			return Registry.Remove(type.FullName);
		}
	}
}
