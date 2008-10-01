

namespace Amplify.Data.Linq
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Data.Linq;
	using System.Linq;
	using System.Text;

	public class Adapter
	{
		private static IDictionary<string, DataContext> s_adapters = new Dictionary<string, DataContext>();
		private static string s_defaultAdapter = "production";

		static Adapter()
		{
			if (ApplicationContext.IsDevelopment)
				s_defaultAdapter = "development";
			if (ApplicationContext.IsTesting)
				s_defaultAdapter = "test";
		}

		public static DataContext Get()
		{
			return Get(null);
		}

		public static DataContext Get(string key)
		{
			return Get(key, true);
		}

		public static string GetString(string key, bool useSuffix)
		{
			if (string.IsNullOrEmpty(key))
				key = s_defaultAdapter;
			else
				key = useSuffix ? string.Format("{0}_{1}", key, s_defaultAdapter) : key;
			if (!s_adapters.ContainsKey(key))
				return ConfigurationManager.ConnectionStrings[key].ConnectionString;
			return null;
		}

		public static DataContext Get(string key, bool useSuffix)
		{
			if (string.IsNullOrEmpty(key))
				key = s_defaultAdapter;
			else
				key = useSuffix ? string.Format("{0}_{1}", key, s_defaultAdapter) : key;
			if (!s_adapters.ContainsKey(key))
				return Add(ConfigurationManager.ConnectionStrings[key]);
			return s_adapters[key];
		}

		public static DataContext Add(System.Configuration.ConnectionStringSettings settings)
		{
			return Add(settings.Name, settings.ProviderName, settings.ConnectionString);
		}

		public static DataContext Add(string name, string providerName, string connectionString)
		{
			DataContext adapter = null;
			switch (providerName.ToLower())
			{
				case "system.data.sqlclient":
					adapter = new DataContext(connectionString);
					break;
				
				default:
					throw new Exception(string.Format(
						"Sql provider '{0}' is not currently supported",
						providerName));
			}
			s_adapters.Add(name, adapter);
			return adapter;
		}
	}
}
