

namespace Amplify.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Text;

	public class ConfigurationSectionGroup : System.Configuration.ConfigurationSectionGroup 
	{

		public T GetSection<T>()
		{
			return (T)this.GetSection(typeof(T));
		}

		public object GetSection(Type type)
		{
			foreach (ConfigurationSection section in this.Sections)
				if (section.GetType() == type)
					return section;
			return null;
		}

		
	}
}
