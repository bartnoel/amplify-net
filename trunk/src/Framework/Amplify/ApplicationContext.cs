//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace Amplify
{
	using System;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Text;
	using System.Web;

	using Amplify.ComponentModel;
	using Amplify.Configuration;
	using Amplify.Data;

	public class ApplicationContext
	{
		private static NameValueContext s_properties;

		static ApplicationContext()
		{
		}

		public static bool ContainsKey(string propertyName)
		{
			if (HttpContext.Current != null)
			{
				foreach (string key in HttpContext.Current.Application.AllKeys)
					if (key == propertyName)
						return true;
			}
			else
			{
				foreach (string key in Context.Keys)
					if (key == propertyName)
						return true;
			}
			return false;
		}

		public static object GetProperty(string propertyName)
		{
			if (HttpContext.Current != null)
				return HttpContext.Current.Application[propertyName];
			else
				return Context[propertyName];
		}

		public static void SetProperty(string propertyName, object value) 
		{
			if (HttpContext.Current != null)
				HttpContext.Current.Application[propertyName] = value;
			else
				Context[propertyName] = value;
		}

		private static NameValueContext Context
		{
			get
			{
				if (s_properties == null)
					s_properties = new NameValueContext();
				return s_properties;
			}
		}


		/*
		internal static string DefaultKey
		{
			get
			{
				if (!ContainsKey("DefaultKey"))
					SetProperty("DefaultKey", ApplicationName);
				return GetProperty("DefaultKey").ToString();
			}
		}

		internal static byte[] DefaultIV
		{
			get
			{
				if (!ContainsKey("DefaultIV"))
					SetProperty("DefaultIV", new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF });
				return (byte[])GetProperty("DefaultIV");
			}
		}
		 */
		
		public  static AmplifySection AmplifyConfiguration
		{
			get { 
				if(!ContainsKey("ApplicationSection"))
				{
					SetProperty("ApplicationSection", System.Configuration.ConfigurationManager.GetSection("amplify"));
					if (GetProperty("ApplicationSection") == null)
						SetProperty("ApplicationSection", new AmplifySection());
				}
				return (AmplifySection)GetProperty("ApplicationSection");
			}
		}

		public static string ApplicationName
		{
			get
			{
				return AmplifyConfiguration.ApplicationName;
			}
			set
			{
				AmplifyConfiguration.ApplicationName = value;
			}
		}

		public static string ConnectionStringName
		{
			get
			{
				return AmplifyConfiguration.ConnectionStringName;
			}
			set
			{
				AmplifyConfiguration.ConnectionStringName = value;
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
			set
			{
				ConnectionStrings[ConnectionStringName] = value;
			}
		}

		public static System.Security.Principal.IPrincipal User
		{
			get {
				if (HttpContext.Current != null)
					return HttpContext.Current.User;
				else
					return System.Threading.Thread.CurrentPrincipal;
			}
			set {
				if (HttpContext.Current != null)
					HttpContext.Current.User = value;
				else
					System.Threading.Thread.CurrentPrincipal = value;
			}
		}

		public static ComponentModel.ServiceRegistry Services
		{
			get
			{
				if(!ContainsKey("!Services"))
					SetProperty("!Services", new ServiceRegistry());
				return (ServiceRegistry)GetProperty("!Services");
			}
		}

		internal static ConnectionStrings ConnectionStrings
		{
			get
			{
				if (!ContainsKey("!ConnectionStrings"))
				{	
					SetProperty("!ConnectionStrings", new ConnectionStrings());
				}
				return (ConnectionStrings)GetProperty("!ConnectionStrings");
			}
		}

	}
}
