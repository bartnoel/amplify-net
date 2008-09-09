

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
			if (ApplicationContext.IsTesting)
				defaultAdapter = "test";
		}

		public static Adapter Get()
		{
			return Get(null);
		}

		public static Adapter Get(string key)
		{
			return Get(key, true);
		}

		public static Adapter Get(string key, bool useSuffix)
		{
			if (string.IsNullOrEmpty(key))
				key = defaultAdapter;
			else 
				key = useSuffix ? string.Format("{0}_{1}", key, defaultAdapter) : key;
			if (!adapters.ContainsKey(key))
				return Add(ConfigurationManager.ConnectionStrings[key]);
			return adapters[key];
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

		public SelectQuery Select()
		{
			return new SelectQuery(this);
		}

		public SelectQuery Select(string selection)
		{
			return new SelectQuery(this).Select(selection);
		}

		public SelectQuery CreateSelect()
		{
			return new SelectQuery(this);
		}

		public abstract string PrimaryKeyType { get; }

		public abstract IEnumerable<ColumnDefinition> GetColumns(string tableName);


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

		public abstract IEnumerable<string> GetColumnNames(string tableName);

		public string AddColumnOptions(Hash options)
		{
			string sql = "";
			if (this.OptionsIncludeDefault(options))
				sql += string.Format(" DEFAULT {0}", Quote(options["default"], null));
			
			if ((options["null"] == null) ? false : (bool)options["null"]) 
				sql += " NOT NULL";
			
			return sql;
		}

		public virtual string SimplifiedType(string fieldType)
		{
			switch (fieldType.ToLower())
			{
				case "int":
					return integer;
				case "float":
				case "double":
					return @float;
				case "decimal":
				case "numeric":
				case "number":
					return @decimal;
				case "datetime":
				case "time":
				case "date":
					return datetime;
				case "clob":
				case "text":
				case "ntext":
					return text;
				case "blob":
				case "binary":
					return binary;
				case "char":
				case "string":
				case "nvarchar":
				case "varchar":
					return @string;
				case "boolean":
				case "bit":
					return boolean;
				case "uniqueidentifier":
					return guid;
				default:
					return fieldType;
			}
		}

		protected bool OptionsIncludeDefault(Hash options)
		{
			return (options["default"] != null);
		}
	}
}
