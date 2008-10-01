//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Data.SqlClient
{
	using System;
	using System.Collections.Generic;
	using System.Data.SqlClient;
	
	using System.Text;

	/// <summary>
	/// 
	/// </summary>
	public class SqlConnectionString : ConnectionStringAdapter
	{

		private int port = 1433;
		private string host = "";

		public SqlConnectionString(string connectionString): this()
		{
			this.ConnectionString = connectionString;
		}

		public SqlConnectionString()
		{
			this.Builder = new System.Data.SqlClient.SqlConnectionStringBuilder();
			this.ConnectionTimeout = 30;
		}

		protected SqlConnectionStringBuilder SqlBuilder
		{
			get { return (SqlConnectionStringBuilder)this.Builder; }
		}

		/// <summary>
		/// Gets the default port if the database supports it.
		/// </summary>
		/// <value>The default port.</value>
		public override int DefaultPort
		{
			get { return 1433; }
		}

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
		public override bool IsFileDriven
		{
			get { return false; }
		}

		/// <summary>
		/// Gets a flag that termines if the database allows user instances.
		/// </summary>
		/// <value><c>true</c> if [allows user instance]; otherwise, <c>false</c>.</value>
		public override bool AllowsUserInstance
		{
			get { return true;  }
		}


		/// <summary>
		/// Gets or sets the host url of the database server.
		/// </summary>
		/// <value>The host.</value>
		public override string Host
		{
			get
			{ 
				string value = (this.SqlBuilder.DataSource);
				if(!string.IsNullOrEmpty(value))
					return value.Replace(","+port.ToString(), "");
				return value;
			}
			set
			{
				if(!Object.Equals(this.host, value)) {
					this.host = value;
					if (this.port != this.DefaultPort)
						this.SqlBuilder.DataSource = string.Format("{0},{1}", value, this.port);
					else
						this.SqlBuilder.DataSource = value;
				}
			}
		}


		/// <summary>
		/// Gets or sets the specific name of the database.
		/// </summary>
		/// <value>The database.</value>
		public override string Database
		{
			get { return (this.SqlBuilder.InitialCatalog); }
			set { this.SqlBuilder.InitialCatalog = value; }
		}

		/// <summary>
		/// Gets or sets the port number.
		/// </summary>
		/// <value>The port.</value>
		public override int Port
		{
			get { return this.port; }
			set
			{
				if(!Object.Equals(this.port, value))
				{
					this.port = value;
					if (value != this.DefaultPort)
						this.SqlBuilder.DataSource = string.Format("{0},{1}", this.host, value);
				}
			}
		}

		public bool IsUserInstance
		{
			get { return this.SqlBuilder.UserInstance; }
			set { this.SqlBuilder.UserInstance = value; }
		}

		public string AttachFileDbFilename
		{
			get { return this.SqlBuilder.AttachDBFilename; }
			set { this.SqlBuilder.AttachDBFilename = value; }
		}

		/// <summary>
		/// Gets or sets the user name used to log into the database.
		/// </summary>
		/// <value>The username.</value>
		public override string Username
		{
			get { return this.SqlBuilder.UserID; }
			set { this.SqlBuilder.UserID = value; }
		}

		/// <summary>
		/// Gets or sets the password used to log into the database.
		/// </summary>
		/// <value>The password.</value>
		public override string Password
		{
			get { return this.SqlBuilder.Password; }
			set { this.SqlBuilder.Password = value; }
		}

		/// <summary>
		/// Gets the provider type of the connection string.
		/// </summary>
		/// <value>The provider.</value>
		public override string Provider
		{
			get { return "System.Data.SqlClient"; }
		}

		/// <summary>
		/// Gets or sets the connection timeout should the connection
		/// take to long to talk to the host.
		/// </summary>
		/// <value>The connection timeout.</value>
		public override int ConnectionTimeout
		{
			get { return this.SqlBuilder.ConnectTimeout; }
			set { this.SqlBuilder.ConnectTimeout = value; }
		}
	}
}
