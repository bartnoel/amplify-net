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

	/// <summary>
	/// Essentially a static class that allows inheritence in order to grab/store values
	/// in an application's context. It makes use of the HttpApplicationContext or a static
	/// Amplify.NameValueContext object depending if the website is available or not. 
	/// </summary>
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

		/// <summary>
		/// Gets a value whether the application is a website (true) or not (false). 
		/// </summary>
		public static bool IsWebsite { get; internal set; }

		/// <summary>
		/// Gets or sets whether this application is currently in a development state or not.
		/// <remarks>
		///	<para>
		///		This property will automatically be set to true if the debugger is running or
		///		if the debug property of the system.web\compilation section of the app/web.config
		///		is set to true. 
		/// </para>
		/// <example>
		///		<pre class="code xml">
		///			&gt;system.web&lt; &gt;compilation debug=&quot;true&quot; &lt; &gt;/compilation&lt; &gt;/system.web&lt;
		///		</pre>
		/// </example>
		/// </remarks>
		/// </summary>
		public static bool IsDevelopment { get; set; }

		/// <summary>
		/// Gets or sets wheither this application is currently in a testing state or not.
		/// </summary>
		/// <remarks>
		/// <para>
		///		This property should be set to true inside an AssemblyFixture for a testing project. 
		/// </para>
		/// <example>
		///		<pre class="code csharp">
		///			[FixtureSetUp]
		///			public void InvokeOnStartUp()
		///			{
		///				ApplicationContext.IsTesting = true;
		///			}
		///		</pre>
		/// </example>
		/// </remarks>
		public static bool IsTesting { get; set; }

		/// <summary>
		/// Determines if the given property is stored inside the application context or not.
		/// </summary>
		/// <param name="propertyName">The name of property that might be stored.</param>
		/// <returns>return true if the property is stored, even if i its null.</returns>
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

		/// <summary>
		/// Gets the object value of the specified property. 
		/// </summary>
		/// <param name="propertyName">The name of the property that is stored.</param>
		/// <returns>returns the object value of the property.</returns>
		public static object GetProperty(string propertyName)
		{
			if (IsWebsite)
				return HttpContext.Current.Application[propertyName];
			else
				return Context[propertyName];
		}

		/// <summary>
		/// Sets the value of the specified property.
		/// </summary>
		/// <param name="propertyName">The name of the property that is to be stored.</param>
		/// <param name="value">The actual value that is to be stored.</param>
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

		/// <summary>
		/// The <see cref="System.Security.Principal.IPrincipal"/> User
		/// </summary>
		/// <remarks>
		/// <para>
		/// If the application is a website, the user is retrieved and stored with
		/// on the"User" Property of <see cref="System.Web.HttpContext.Current" /> otherwise it uses
		/// the "CurrentPrincipal" property of <see cref="System.Threading.Thread/>
		/// </para>
		/// </remarks>
		public static System.Security.Principal.IPrincipal User
		{
			get {
				if (IsWebsite)
					return HttpContext.Current.User;
				else
					return System.Threading.Thread.CurrentPrincipal;
			}
			set {
				if (IsWebsite)
					HttpContext.Current.User = value;
				else
					System.Threading.Thread.CurrentPrincipal = value;
			}
		}

		/// <summary>
		/// Gets a service registery object used for application level services.
		/// </summary>
		/// <remarks>
		/// All services must impliment <see cref="Amplify.ComponentMode.IService"/> in order
		/// to be stored in the service registry. 
		/// </remarks>
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
