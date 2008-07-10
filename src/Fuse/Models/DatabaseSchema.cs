using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuse.Models
{
	using Amplify.Data;
	using Amplify.Linq;

	public class DatabaseSchema
	{
		private List<TableSchema> tables;

		public string Name { get; set; }

		public List<TableSchema> Tables
		{
			get {
				if (this.tables == null)
					this.tables = new List<TableSchema>();
				return this.tables;
			}
		}

		public static List<DatabaseSchema> Find(string connection)
		{
			List<DatabaseSchema> list = new List<DatabaseSchema>();
			string[] databases = Adapter.Add(connection).GetDatabases();
			foreach (string database in databases)
				list.Add(new DatabaseSchema() { Name = database });
			return list;
		}
	}
}
