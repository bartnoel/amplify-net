//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace Amplify
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	using Amplify.Configuration;
	using Amplify.Data;

	public class ApplicationContext
	{
		private static Dictionary<string, object> s_properties;

		static ApplicationContext()
		{
			s_properties = new Dictionary<string, object>();
		}

		public static Dictionary<string, object> Properties
		{
			get
			{
				return s_properties;
			}
		}


		internal static string DefaultKey
		{
			get
			{
				if (!Properties.ContainsKey("DefaultKey"))
				{
					Properties["DefaultKey"] = ApplicationName;
				}
				return Properties["DefaultKey"].ToString();
			}
		}

		internal static byte[] DefaultIV
		{
			get
			{	
				if(!Properties.ContainsKey("DefaultIV"))
				{
					Properties["DefaultIV"] =  new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
				}
				return (byte[])Properties["DefaultIV"];
			}
		}

		
		public  static AmplifySection AmplifyConfiguration
		{
			get { 
				if(!Properties.ContainsKey("ApplicationSection"))
				{
					Properties ["ApplicationSection"] = System.Configuration.ConfigurationManager.GetSection("amplify");
					if (Properties["ApplicationSection"] == null)
						Properties["ApplicationSection"] = new AmplifySection();
				}
				return (AmplifySection)Properties["ApplicationSection"];
			}
		}

		public static string ApplicationName
		{
			get
			{
				return AmplifyConfiguration.ApplicationName;
			}
		}

		public static string ConnectionStringName
		{
			get
			{
				return AmplifyConfiguration.ConnectionStringName;
			}
		}

		public static bool IsConnectionStringEncrypted
		{
			get
			{
				return AmplifyConfiguration.IsConnectionStringEncrypted;
			}
		}

		public static bool IsLinqEnabled
		{
			get { return AmplifyConfiguration.IsLinqEnabled; }
		}

		public static System.Configuration.ConnectionStringSettings ConnectionStringSettings
		{

			get
			{
				return System.Configuration.ConfigurationManager.ConnectionStrings[ConnectionStringName];
			}
		}

		public static string Mode
		{
			get {
				return AmplifyConfiguration.Mode;
			}
			set
			{
				AmplifyConfiguration.Mode = value;
			}
		}

		public static string ConnectionString
		{
			get
			{
				return ConnectionStrings[ConnectionStringName];
			}
		}

		public static ServiceRegistry Services
		{
			get
			{
				if(!Properties.ContainsKey("!Services"))
					Properties.Add("!Services", new ServiceRegistry());
				return (ServiceRegistry)Properties["!Services"];
			}
		}

		internal static ConnectionStrings ConnectionStrings
		{
			get
			{
				if (!Properties.ContainsKey("!ConnectionStrings"))
				{
					byte[] iv = DefaultIV;
					string key = DefaultKey;
					
					Properties.Add("!ConnectionStrings", new ConnectionStrings(key, iv));
				}
				return (ConnectionStrings)Properties["!ConnectionStrings"];
			}
		}

	}
}
