

namespace Amplify.Data
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Data;
	
	using System.Text;
	using System.Text.RegularExpressions;

	using Amplify.Linq;

	public abstract partial class Adapter : SchemaBase 
	{

		private ConnectionStringSettings connectionStringSettings;
		private string connectionString;
		private Security.Cryptography.IEncryptable encryptor;

		private static IDictionary<string, Adapter> adapters = new Dictionary<string, Adapter>();
		private static string defaultAdapter = "production";


		static Adapter()
		{
			if (ApplicationContext.IsDevelopment)
				defaultAdapter = "development";
			else if (ApplicationContext.IsTesting)
				defaultAdapter = "test";
		}

		public static Adapter Get()
		{
			return Get(defaultAdapter);
		}

		public static Adapter Get(string name)
		{
			if (!adapters.ContainsKey(name))
				return Add(ConfigurationManager.ConnectionStrings[name]);
			return adapters[name];
		}

		public enum Types
		{
			Sql,
			SqlCe
		}

		public static Adapter Add(System.Configuration.ConnectionStringSettings settings)
		{
			return Add(settings.Name, settings.ProviderName, settings.ConnectionString);
		}

		public static Adapter Add(string name, string providerName, string connectionString)
		{
			Adapter adapter = null; 
			switch (providerName.ToLower())
			{
				case "system.data.sqlclient":
					adapter = new SqlClient.SqlAdapter(connectionString);
					break;
				case "system.data.sqlserverce":
					adapter = new SqlClientCe.SqlAdapter(connectionString);
					break;
				default:
					throw new Exception(string.Format(
						"Sql provider '{0}' is not currently supported",
						providerName));
			}
			adapters.Add(name, adapter);
			return adapter;
		}

		public abstract System.Data.Common.DbConnectionStringBuilder GetBuilder();

		public Security.Cryptography.IEncryptable ConnectionStringEncryptor
		{
			get { return this.encryptor; }
			set { 
				if(!Object.Equals(this.encryptor, value))
				{
					this.encryptor = value;
					if (!string.IsNullOrEmpty(this.connectionString)) 
						 this.connectionString = this.encryptor.Decrypt(this.ConnectionString);
				}
			}
		}

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

		

		

		public string ConnectionString
		{
			get {
				return this.connectionString;
			}
			set
			{
				if (!Object.Equals(this.connectionString, value))
				{
					if (this.encryptor != null)
						this.connectionString = this.encryptor.Decrypt(value);
					else
						this.connectionString = value;
				}
				
			}
		}

		public abstract Hash NativeDatabaseTypes { get; }

		public abstract string PrimaryKeyType { get; }

		public abstract IEnumerable<Column> GetColumns(string tableName);


		protected string RenameSelection(string tableName, string selection)
		{
			string columns = "";

			foreach (string column in StringUtil.Split(selection, ","))
				columns += this.QuoteTableName(tableName) + "." + columns + ", ";
		
			return StringUtil.TrimEnd(columns, ", ");
		}

		public string ConstructFinderSql(IOptions options)
		{
			string sql = "";
			string select = options.Select;
			bool joined = options.Include.Count > 0; 

			sql += string.Format("SELECT {0} {1} ", (options.IsDistinct ? "DISTINCT" : ""), select);
			sql += string.Format(" FROM {0} ", this.QuoteTableName(options.From) + ((joined) ? " AS " + options.As : ""));

			sql = AddJoins(sql, options);
			sql = AddConditions(sql, options);

			if (!string.IsNullOrEmpty(options.Group))
				sql += string.Format(" GROUP BY {0} ", options.Group);

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

		

		protected virtual string AddConditions(string sql, IOptions options)
		{
			if (!string.IsNullOrEmpty(options.Where))
			{
				sql += string.Format(" WHERE {0} ", options.Where);
				object[] temp = new object[] { };
				options.Conditions.CopyTo(temp, 1);
				options.Conditions = temp;
			}
			return sql;
		}

		protected virtual string AddOrder(string sql, IOptions options)
		{
			if (!string.IsNullOrEmpty(options.Order))
				sql += string.Format(" ORDER BY {0} ", options.Order);
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
			List<string> list = new List<string>(first);

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
			
			if ((options["Null"] == null) ? false : (bool)options["Null"]) 
				sql += " NOT NULL";
			
			return sql;
		}

		protected bool OptionsIncludeDefault(Hash options)
		{
			return (options["Default"] != null);
		}
	}
}
