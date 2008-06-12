using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Amplify.Data.Mappings
{
	public interface IDataEntity
	{
		string ClassName { get; set; }
		string EntityName { get; set; }
		Type Type { get; set; }
	}

	
}
