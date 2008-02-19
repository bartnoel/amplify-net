

namespace Amplify.Data
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;

	using Amplify.Linq;

	public abstract partial class Adapter : SchemaBase 
	{
		public virtual string Name
		{
			get { return "Abstract"; }
		}

		public virtual bool IsMigrationsSupported
		{
			get { return true; }
		}

		public virtual bool IsCountDistinctSupported
		{
			get { return true; }
		}

		public abstract Hash NativeDatabaseTypes { get; }

		public abstract string PrimaryKeyType { get; }

		public abstract IEnumerable<Column> GetColumns(string tableName);



		public string AddColumnOptions(Hash options)
		{
			string sql = "";
			if (this.OptionsIncludeDefault(options))
				sql += string.Format(" DEFAULT {0}", Quote(options["Default"], (options["column"] as ColumnDefinition)));
			if (options["Null"].Default(true) == false)
				sql += " NOT NULL";
			return sql;
		}

		protected bool OptionsIncludeDefault(Hash options)
		{
			return (options["Default"] != null && (options["Null"].Default(true) == false && options["Default"] != null));
		}
		
		
	}
}
