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


		internal static ApplicationSection AppSection
		{
			get { 
				if(!Properties.ContainsKey("ApplicationSection"))
				{
					Properties ["ApplicationSection"] = System.Configuration.ConfigurationManager.GetSection(ApplicationSectionPath);
					if (Properties["ApplicationSection"] == null)
						Properties["ApplicationSection"] = new ApplicationSection();
				}
				return (ApplicationSection)Properties["ApplicationSection"];
			}
		}

		public static string ApplicationName
		{
			get
			{
				return AppSection.ApplicationName;
			}
		}

		public static string ConnectionStringName
		{
			get
			{
				return AppSection.ConnectionStringName;
			}
		}

		public static bool IsConnectionStringEncrypted
		{
			get
			{
				return AppSection.IsConnectionStringEncrypted;
			}
		}

		public static bool IsLinqEnabled
		{
			get { return AppSection.IsLinqEnabled; }
		}

		public static System.Configuration.ConnectionStringSettings ConnectionStringSettings
		{

			get
			{
				return System.Configuration.ConfigurationManager.ConnectionStrings[ConnectionStringName];
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
