

namespace Amplify.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Text;

	public class PluginElement : ConfigurationElement
	{
		[ConfigurationProperty("name", IsRequired= true)]
		public virtual string Name
		{
			get { return this["name"].ToString(); }
			set { this["name"] = value; }
		}

		[ConfigurationProperty("type", IsRequired=true)]
		public virtual string Type
		{
			get { return this["type"].ToString(); }
			set { this["type"] = value; }
		}
	}
}
