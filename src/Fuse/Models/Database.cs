using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuse.Models
{
	using Fuse.Properties;
	using Amplify.Data;
	using Amplify.Data.SqlClient;

	public partial class Database
	{
		public static IEnumerable<string>  GetDatabaseNames() {
			Adapter adapter = new SqlAdapter(Settings.Default.CurrentConnection);
			return adapter.GetDatabases();
		}
	}
}
