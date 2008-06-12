using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuse.Models
{
	using System.Data.Common;

	using Amplify.Data;

	public class TableSchema
	{
		private List<ColumnSchema> columns;

		public string Name { get; set; }

		public List<ColumnSchema> Columns
		{
			get {
				if (columns == null)
					this.columns = new List<ColumnSchema>();
				return this.columns;
			}
		}

		public static List<TableSchema> Find(string database, string connectionString)
		{
			List<TableSchema> list = new List<TableSchema>();
			Adapter adapter =	Adapter.Create(connectionString);
			var builder = adapter.GetBuilder();
			builder["InitialCatalog"] = database;
			adapter = Adapter.Create(builder.ToString());
			foreach (string table in adapter.GetTableNames())
				list.Add(new TableSchema() { Name = table });
		
			return list;
		}
	}
}
