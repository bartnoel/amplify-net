

namespace Amplify.Configuration
{
	using System;
	using System.Configuration;
	using System.Collections.Generic;
	using System.Text;

	public class ConfigBase
	{
		private static ApplicationSection s_appSection;
		private string applicationSection;

		public ConfigBase(string applicationSection)
		{
			this.applicationSection = applicationSection;
		}

		protected string ApplicationSectionName
		{
			get { return this.applicationSection; }
		}

		internal ApplicationSection App
		{
			get
			{
				if (s_appSection == null) s_appSection = (ApplicationSection)ConfigurationManager.GetSection(this.applicationSection);
				if (s_appSection == null) s_appSection = new ApplicationSection();
				return s_appSection;
			}
		}
	}
}
;