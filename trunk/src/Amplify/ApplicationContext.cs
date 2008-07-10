//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace Amplify
{
	using System;
	using System.Configuration;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Text;
	using System.Web;

	using Amplify.ComponentModel;
	using Amplify.Configuration;

	public class ApplicationContext
	{
		private static NameValueContext s_properties;

		static ApplicationContext()
		{
			IsWebsite = (System.Web.HttpContext.Current != null);
			if (IsWebsite)
			{
				System.Web.Configuration.CompilationSection section
					= (System.Web.Configuration.CompilationSection)
						ConfigurationManager.GetSection("system.web/compliation");

				IsDevelopment = (section != null && section.Debug);
			}
			else
			{
				IsDevelopment = System.Diagnostics.Debugger.IsAttached;
			}
		}

		public static bool IsWebsite { get; internal set; }

		public static bool IsDevelopment { get; internal set; }

		public static bool IsTesting { get; internal set; }

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


	}
}
