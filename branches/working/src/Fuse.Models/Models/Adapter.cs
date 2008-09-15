 

namespace Fuse.Models
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;

	using Amplify;
	using Amplify.Data;

	public abstract class Adapter
	{
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

		public bool LowerNaming { get; set; }

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
					//adapter = new SqlClientCe.SqlAdapter(connectionString);
					break;
				default:
					throw new Exception(string.Format(
						"Sql provider '{0}' is not currently supported",
						providerName));
			}
			adapters.Add(name, adapter);
			return adapter;
		}

		public abstract List<String> GetTableNames();

		public abstract List<ColumnDefinition> GetColumns(string tableName);

		public abstract List<KeyConstraint> GetKeys(string tableName);

		public abstract List<ConstraintDefinition> GetConstraints(string tableName);

		public abstract List<IndexDefinition> GetIndexes(string tableName);

		public abstract List<TriggerDefinition> GetTriggers(string tableName);

		
	}
}
