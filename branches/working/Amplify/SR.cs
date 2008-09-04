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
	using System.Globalization;
	using System.Resources;

	/// <summary>
	/// Class which exposes string resources
	/// </summary>
	public class SR 
	{
		private ResourceManager resources;
		private static SR loader = null;
		private static object synclock = new object();
		protected static Type type;
		protected static Type SRtype;
		

		/// <summary>
		/// Default constructor
		/// </summary>
		internal SR()
		{
			type = this.GetType();
			SRtype = type;
			this.BaseName = "Csla.Extentions";
			this.CreateResources();
		}

		/// <summary>
		/// 
		/// </summary>
		protected void CreateResources()
		{
			this.resources = new System.Resources.ResourceManager(this.BaseName, type.Assembly);
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual string BaseName { get; set; }

		/// <summary>
		/// Return the resource manager for the assembly
		/// </summary>
		public  ResourceManager Resources
		{
			get { return this.resources; }
		}

		/// <summary>
		/// 
		/// </summary>
		public static CultureInfo Culture 
		{ 
			get { return null; } 
		}


		/// <summary>
		/// Return the static loader instance
		/// </summary>
		/// <returns></returns>
		private static SR GetLoader()
		{
			if (loader == null)
			{
				lock (synclock)
				{
					if (loader == null)
						loader = (SR)SRtype.TypeInitializer.Invoke(new object[] { }); 
				}
			}

			return loader;
		}

		/// <summary>
		/// Get a string resource
		/// </summary>
		/// <param name="name">The resource name</param>
		/// <returns>The localized resource</returns>
		public static string GetString(string name)
		{
			SR sr = GetLoader();
			if (sr == null)
				return null;

			return sr.Resources.GetString(name, SR.Culture);
		}

		/// <summary>
		/// Get the localized string for a particular culture
		/// </summary>
		/// <param name="name">The resource name</param>
		/// <param name="args">The args that are passed into the formatted string. </param>
		/// <returns>The localized resource</returns>
		public static string GetString(string name, params object[] args)
		{
			SR sr = GetLoader();
			if (sr == null)
				return null;

			string format = sr.resources.GetString(name, SR.Culture);
			if (args == null || args.Length == 0)
				return format;

			return string.Format(SR.Culture, format, args);
		}
	}
}
