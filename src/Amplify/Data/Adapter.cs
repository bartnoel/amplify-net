
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1020")]

namespace Amplify.Data
{
    using System;
    using System.Collections.Generic;
	using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Text;
	using Properties;

    /// <summary>
    /// Serves as an abstract class that encapsulates adapting queries
    /// to various DataStores
    /// </summary>
	public abstract partial class Adapter
    {
		private static Dictionary<string, Adapter> adapters;
        private static string defaultAdapter;

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString { get; set; }

		/// <summary>
		/// Gets the default adapter.
		/// </summary>
		/// <value>The default adapter.</value>
        protected static string DefaultAdapter
        {
            get 
            {
                if (defaultAdapter == null)
                {
                    IAmp o = Amp.Get();

                    if (o.IsInDevelopment)
                        defaultAdapter = "development";
                    if (o.IsInTest)
                        defaultAdapter = "test";
                    if (o.IsInStaging)
                        defaultAdapter = "staging";
                    else
                        defaultAdapter = "production";
                }
                return defaultAdapter;
            }
        }

		/// <summary>
		/// Gets the adapters.
		/// </summary>
		/// <value>The adapters.</value>
        protected static Dictionary<string, Adapter> Adapters
        {
            get 
            {
                if (adapters == null)
                    adapters = new Dictionary<string, Adapter>();
                return adapters;
            }
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        public static Adapter Get()
        {
            return Get(null);
        }

        /// <summary>
        /// Gets the instance specified by the key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>returns An Adapter</returns>
        public static Adapter Get(string key)
        {
            return Get(key, true);
        }

        /// <summary>
        /// Gets the instance specified by the key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="useSuffix">if set to <c>true</c> [use suffix].</param>
        /// <returns>An Adapter</returns>
		public static Adapter Get(string key, bool useSuffix)
		{
            string actualKey;

            if (string.IsNullOrEmpty(key))
                actualKey = DefaultAdapter;
            else
                actualKey = useSuffix ? string.Format(CultureInfo.InvariantCulture, "{0}_{1}", key, DefaultAdapter) : key;
			if (!Adapters.ContainsKey(actualKey))
			{
				ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[actualKey];
				if (settings == null)
					throw new ArgumentException(
                        string.Format(
                            CultureInfo.CurrentCulture, 
                            Resources.Exception_ConnectionStringKeyNotFound,
							actualKey));
				return Add(settings);
			}
				
			return adapters[actualKey];
		}

        /// <summary>
        /// Adds a new instance of an adapter by the connection string.  
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
		public static Adapter Add(System.Configuration.ConnectionStringSettings settings)
		{
			return Add(settings.Name, settings.ProviderName, settings.ConnectionString);
		}

        /// <summary>
        /// Adds a new instance of an adapter with a keyed lookup.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        [SuppressMessageAttribute("Microsoft.Globalization", "CA1308")]
		public static Adapter Add(string key, string providerName, string connectionString)
		{
			Adapter adapter = null;
			switch (providerName.ToLowerInvariant())
			{
				case "system.data.sqlclient":
				case "mssql":
				case "sqlclient":
					adapter = new Adapters.SqlAdapter();
					break;
				case "system.data.sqlserverce":
					// adapter = new SqlClientCe.SqlAdapter(connectionString);
					break;
				case "system.data.mysql":
				case "sun.data.mysql":
				case "mysqlclient":
				case "mysql":
					adapter = new Adapters.MySqlAdapter();
					break;
				default:
					throw new ArgumentException(
                        string.Format(
                            CultureInfo.CurrentCulture,
						    Resources.Exception_DataAdapterNotSupported,
						    providerName));
			}

			adapter.ConnectionString = connectionString;
			Adapters.Add(key, adapter);
			return adapter;
		}
    }
}
