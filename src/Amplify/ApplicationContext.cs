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
			
			System.Web.Configuration.CompilationSection section
				= (System.Web.Configuration.CompilationSection)
					ConfigurationManager.GetSection("system.web/compilation");

			IsDevelopment = (section != null && section.Debug);
			

			if(!IsDevelopment) 
				IsDevelopment = System.Diagnostics.Debugger.IsAttached;
			
		}

		public static bool IsWebsite { get; internal set; }

		public static bool IsDevelopment { get; set; }

		public static bool IsTesting { get; set; }

		public static bool ContainsProperty(string propertyName)
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
			if (IsWebsite)
				return HttpContext.Current.Application[propertyName];
			else
				return Context[propertyName];
		}

		public static void SetProperty(string propertyName, object value) 
		{
			if (IsWebsite)
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
				if(!ContainsProperty("!Services"))
					SetProperty("!Services", new ServiceRegistry());
				return (ServiceRegistry)GetProperty("!Services");
			}
		}


	}
}
