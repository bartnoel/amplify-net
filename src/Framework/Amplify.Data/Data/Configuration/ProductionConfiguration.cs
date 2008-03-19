

namespace Amplify.Data.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Linq;
	using System.Text;
	



	public class ProductionConfiguration : EnvironmentConfiguration  
	{

		[ConfigurationProperty("connectionStringName", DefaultValue="production")]
		public override string ConnectionStringName
		{
			get { return ""; }
		}

	}
}
