

namespace Amplify.Configuration
{
	using System;
	using System.Configuration;
	using System.Collections.Generic;
	using System.Text;

	public class Config : DecoratedObject
	{
		private static ApplicationSection s_appSection;
		private string applicationSection;

		public Config(string applicationSection)
		{
			this.applicationSection = applicationSection;
		}

		protected string ApplicationSectionName
		{
			get { return this.applicationSection; }
		}

		internal ApplicationSection AppSection
		{
			get
			{
				if (this["ApplicationSection"] == null) this["ApplicationSection"] = (ApplicationSection)ConfigurationManager.GetSection(this.applicationSection);
				if (this["ApplicationSection"] == null) this["ApplicationSection"] = new ApplicationSection();
				return (ApplicationSection)this["ApplicationSection"];
			}
		}
	}
}
;