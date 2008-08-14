//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

/// <summary>
/// 
///</summary>
namespace Amplify.Data
{

	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.Common;
	
	using System.Text;

	/// <summary>
	/// A base class for connection string adaption between the different providers.
	/// </summary>
	public abstract class ConnectionStringAdapter
	{

		/// <summary>
		/// Gets or sets the database connection string builder.
		/// </summary>
		protected DbConnectionStringBuilder Builder { get; set; }

		/// <summary>
		/// Gets or sets the connection string.
		/// </summary>
		public string ConnectionString
		{
			get { return this.Builder.ConnectionString; }
			set { this.Builder.ConnectionString = value; }
		}

		/// <summary>
		/// Gets the default port if the database supports it.
		/// </summary>
		/// <value>The default port.</value>
		public abstract int DefaultPort { get; }

		/// <summary>
		/// Gets a flag that determines if this database is file driven.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is file driven; otherwise, <c>false</c>.
		/// </value>
		/// <remarks>
		/// A file driven database is something like sqlite or sqlce
		/// where the connection depends on the file uri/path being
		/// specified in order to connect.
		/// </remarks>
		public abstract bool IsFileDriven { get; }

		/// <summary>
		/// Gets a flag that termines if the database allows user instances.
		/// </summary>
		/// <value><c>true</c> if [allows user instance]; otherwise, <c>false</c>.</value>
		public abstract bool AllowsUserInstance { get; }

		/// <summary>
		/// Gets or sets the host url of the database server.
		/// </summary>
		/// <value>The host.</value>
		public abstract string Host { get; set; }

		/// <summary>
		/// Gets or sets the specific name of the database.
		/// </summary>
		/// <value>The database.</value>
		public abstract string Database { get; set; }

		/// <summary>
		/// Gets or sets the port number.
		/// </summary>
		/// <value>The port.</value>
		public abstract int Port { get; set; }

		/// <summary>
		/// Gets or sets the user name used to log into the database.
		/// </summary>
		/// <value>The username.</value>
		public abstract string Username { get; set; }

		/// <summary>
		/// Gets or sets the password used to log into the database.
		/// </summary>
		/// <value>The password.</value>
		public abstract string Password { get; set; }

		/// <summary>
		/// Gets or sets the provider type of the connection string.
		/// </summary>
		/// <value>The provider.</value>
		public abstract string Provider { get;  }

		/// <summary>
		/// Gets or sets the connection timeout should the connection
		/// take to long to talk to the host.
		/// </summary>
		/// <value>The connection timeout.</value>
		public abstract int ConnectionTimeout { get; set; }


		/// <summary>
		/// Gets or sets the specific properties of the connection string that 
		/// is unique to the specific type of database.
		/// </summary>
		/// <param name="propertyName">The name of the property in the connection string.</param>
		/// <value> The indexed property. </value>
		/// <returns>returns a <see cref="System.Object"/>.</returns>
		public object this[string propertyName]
		{
			get { return this.Builder[propertyName]; }
			set { this.Builder[propertyName] = value; }
		}
	}
}
