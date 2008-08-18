using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data.Mappings
{
	public interface IDataAssociation
	{
		string ForeignKey { get; set; }
		Type ForeignType { get; set; }
		string Key { get; set; }
		string EntityName { get; set; }
	}
}
