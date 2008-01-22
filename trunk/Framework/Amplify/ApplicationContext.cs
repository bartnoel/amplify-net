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

		internal static string ApplicationSectionPath
		{
			get
			{
				return "amplify/application";
			}
		}

		internal static string DefaultKey
		{
			get
			{
				if (!Properties.ContainsKey("DefaultKey"))
				{
					Properties["DefaultKey"] = ApplciationName;
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

		internal static ConfigBase Config
		{
			get
			{
				if (!Properties.ContainsKey("Config") || !(Properties["Config"] is ConfigBase))
					Properties["Config"] = new ConfigBase(ApplicationSectionPath);
				return (ConfigBase)Properties["Config"];
			}
		}

		public static string ApplciationName
		{
			get
			{
				return Config.App.ApplicationName;
			}
		}

		public static string ConnectionStringName
		{
			get
			{
				return Config.App.ConnectionStringName;
			}
		}

		public static bool IsConnectionStringEncrypted
		{
			get
			{
				return Config.App.IsConnectionStringEncrypted;
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
