

namespace Amplify.Data
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Data;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;

	using Amplify.Configuration;
	using Amplify.Linq;

	public abstract partial class Adapter : SchemaBase 
	{

		private ConnectionStringSettings connectionStringSettings;

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

		public AmplifySection Configuration
		{
			get {
				return ApplicationContext.AmplifyConfiguration;
			}
		}

		public ConnectionStringSettings ConnectionStringSettings
		{
			get
			{
				//allow global changes of the connection string if the local is not set.
				if(this.connectionStringSettings == null)
					return ConfigurationManager.ConnectionStrings[this.Configuration.ConnectionStringName];
				return this.connectionStringSettings;
			}
			
			set {
				this.connectionStringSettings = value;
			}
		}

		public string ConnectionString
		{
			get {
				return this.ConnectionStringSettings.ConnectionString;
			}
		}

		public abstract Hash NativeDatabaseTypes { get; }

		public abstract string PrimaryKeyType { get; }

		public abstract IEnumerable<Column> GetColumns(string tableName);


		public string ConstructFinderSql(IOptions options)
		{
			string sql = "";
			sql += "SELECT {0} {1} ".Inject((options.Distinct ? "DISTINCT" : ""), options.Select);
			sql += " FROM {0} ".Inject(this.QuoteTableName(options.From));

			sql = AddJoins(sql, options);
			sql = AddConditions(sql, options);

			if (!string.IsNullOrEmpty(options.Group))
				sql += " GROUP BY {0} ".Inject(options.Group);

			sql = AddOrder(sql, options);
			sql = AddLimit(sql, options);
			sql = AddLock(sql, options);
			return sql;
		}

		public string ConstructFinderSql(IOptions options, IRelational obj)
		{
			return ConstructFinderSql(options);

		}


		protected virtual string AddJoins(string sql, IOptions options)
		{
			if (!string.IsNullOrEmpty(options.Join))
				sql += options.Join;
			return sql;
		}

		protected virtual string AddConditions(string sql, IOptions options)
		{
			if (!string.IsNullOrEmpty(options.Where))
			{
				sql += " WHERE {0} ".Inject(options.Where);
				options.Conditions.RemoveAt(0);
			}
			return sql;
		}

		protected virtual string AddOrder(string sql, IOptions options)
		{
			if (!string.IsNullOrEmpty(options.Order))
				sql += " ORDER BY {0} ".Inject(options.Order);
			return sql;
		}

		protected virtual string AddLimit(string sql, IOptions options)
		{
			return sql;
		}

		protected virtual string AddLock(string sql, IOptions options)
		{
			return sql;
		}

		internal IEnumerable<string> MergeIncludes(IEnumerable<string> first, IEnumerable<string> second)
		{
			List<string> list = first.ToList();
			foreach(string s in second)
				if(!list.Contains(s))
					list.Add(s);
			return (IEnumerable<string>)list;
		}


		public string AddColumnOptions(Hash options)
		{
			string sql = "";
			if (this.OptionsIncludeDefault(options))
				sql += string.Format(" DEFAULT {0}", Quote(options["Default"], (options["Column"] as ColumnDefinition)));
			if (options["Null"].Default(true) == false)
				sql += " NOT NULL";
			return sql;
		}

		protected bool OptionsIncludeDefault(Hash options)
		{
			return (options["Default"] != null);
		}
	}
}
