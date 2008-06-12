using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuse.Models
{
	public class ModelMetaInfo
	{
		private static Dictionary<Type, ModelMetaInfo> repository;

		private List<AssocationAttribute> associations;
		private List<TableAttribute> tables;
		private List<ColumnAttribute> columns;
		private string[] primaryKeys;


		public TableAttribute PrimaryTable { get; set; }

		internal static Dictionary<Type, ModelMetaInfo> Repository
		{
			get {
				if (repository == null)
					repository = new Dictionary<Type, ModelMetaInfo>();
				return repository;
			}
		}

		public List<AssocationAttribute> Assocations
		{
			get {
				if (this.associations == null)
					this.associations = new List<AssocationAttribute>();
				return this.associations;
			}	
		}

		public List<TableAttribute> Tables
		{
			get {
				if (this.tables == null)
					this.tables = new List<TableAttribute>();
				return this.tables;
			}
		}

		public List<ColumnAttribute> Columns
		{
			get
			{
				if (this.columns == null)
					this.columns = new List<ColumnAttribute>();
				return this.columns;
			}
		}

		public string[] PrimaryKeys
		{
			get { return this.primaryKeys; }
			set { this.primaryKeys = value; }
		}

		public Type Type { get; set; }
	}
}
