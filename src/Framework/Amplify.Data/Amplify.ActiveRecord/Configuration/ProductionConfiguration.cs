

namespace Amplify.ActiveRecord.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Linq;
	using System.Text;
	



	public class ProductionConfiguration : BaseConfiguration 
	{

		[ConfigurationProperty()]
		public override string ConnectionStringName
		{
			
		}

	}
}
