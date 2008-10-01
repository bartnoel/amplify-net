

namespace Fuse.Models
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using Amplify.Data;
	using Amplify.Linq;

	public class DataStore
	{
		private List<TableView> tables;

		public string Name { get; set; }

		public string Description { get; set; }

		internal protected Adapter Adapter { get; set; }

		public List<TableView> Tables
		{
			get {
				if (this.tables == null)
				{
					this.tables = new List<TableView>();
					this.Adapter.GetTableNames().Each(table => {
						this.tables
							.Add(new TableView() { Name = table });
					});
				}
				return this.tables;
			}
		}

	}
}
