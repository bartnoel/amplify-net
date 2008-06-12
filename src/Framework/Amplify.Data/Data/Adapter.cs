

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
		private string connectionString;

		public enum Types
		{
			Sql,
			SqlCe
		}

		public static Adapter Create(string connectionString, string type)
		{
			Adapter adapter = (Adapter)Activator.CreateInstance(Type.GetType("Amplify.Data." + type + ".SqlAdapter", true, true));
			adapter.ConnectionString = connectionString;
			return adapter;
		}

		public static Adapter Create(string connectionString)
		{
			return new SqlClient.SqlAdapter(connectionString);	
		}

		public abstract System.Data.Common.DbConnectionStringBuilder GetBuilder();
	

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

		

		public string ConnectionString
		{
			get {
				return this.connectionString;
			}
			set
			{
				this.connectionString = value;
			}
		}

		public abstract Hash NativeDatabaseTypes { get; }

		public abstract string PrimaryKeyType { get; }

		public abstract IEnumerable<Column> GetColumns(string tableName);


		protected string RenameSelection(string tableName, string selection)
		{
			string columns = "";
			selection.Split(",").Each(column =>
				columns += this.QuoteTableName(tableName) + "." +
				column + ", ");
			return columns.TrimEnd(", ".ToCharArray());
		}

		public string ConstructFinderSql(IOptions options, params AssociationAttribute[] includes)
		{
			string sql = "";
			string select = options.Select;
			bool joined = (includes != null && includes.Length > 0);

			if(joined)
				select = this.RenameSelection(options.As, options.Select);

			sql += "SELECT {0} {1} ".Fuse((options.IsDistinct ? "DISTINCT" : ""), select);
			sql += " FROM {0} ".Fuse(this.QuoteTableName(options.From) + ((joined) ? " AS " + options.As : ""));

			sql = AddJoins(sql, options);
			sql = AddConditions(sql, options);

			if (!string.IsNullOrEmpty(options.Group))
				sql += " GROUP BY {0} ".Fuse(options.Group);

			sql = AddOrder(sql, options);
			sql = AddLimit(sql, options);
			sql = AddLock(sql, options);
			return sql;
		}

		public string ConstructFinderSql(IOptions options, IRelational obj)
		{
			return ConstructFinderSql(options);

		}


		protected virtual string AddJoins(string sql, IOptions options, params AssociationAttribute[] includes)
		{
			foreach (AssociationAttribute include in includes)
			{

			}
			return sql;
		}

		protected virtual string AddHasOne(string sql, IOptions options, HasOneAttribute hasOne)
		{
			//hasOne.As 
		}

		protected virtual string AddConditions(string sql, IOptions options)
		{
			if (!string.IsNullOrEmpty(options.Where))
			{
				sql += " WHERE {0} ".Fuse(options.Where);
				object[] temp = new object[] { };
				options.Conditions.CopyTo(temp, 1);
				options.Conditions = temp;
			}
			return sql;
		}

		protected virtual string AddOrder(string sql, IOptions options)
		{
			if (!string.IsNullOrEmpty(options.Order))
				sql += " ORDER BY {0} ".Fuse(options.Order);
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
