//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Data 
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Configuration;

	using Amplify.Security.Cryptography;

	/// <summary>
	/// Connection string wrapper that helps with managing strings that could be encypted. 
	/// </summary>
	public class ConnectionStrings 
	{
		private byte[] iv;
		private string key;
		private bool isEncrypted = AmplifyApplicationContext.IsConnectionStringEncrypted;
		private Dictionary<string, string> definedStrings;

		/// <summary>
		/// Constructor.
		/// </summary>
		public ConnectionStrings()
		{
			this.definedStrings = new Dictionary<string, string>();
			this.isEncrypted = false;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="key"></param>
		/// <param name="iv"></param>
		public ConnectionStrings(string key, byte[] iv)
		{
			this.key = key;
			this.iv = iv;
			this.definedStrings = new Dictionary<string, string>();
		}

		/// <summary>
		/// Gets or sets if the connection strings are encrypted or not.
		/// </summary>
		public bool IsEncrypted 
		{
			get { return this.isEncrypted; }
			set { 
				if(value == true)
				{
					if (this.Key == null)
						throw new NullReferenceException("The ConnectionStrings 'Key' property must be set before encryption of strings are allowed.");
					if (this.IV == null)
						throw new NullReferenceException("The ConnectionSTrings 'IV' property must be set before encryption of strings are allowed.");
				}
				this.isEncrypted = value; 
			}
		}

		/// <summary>
		/// Gets or sets the IV value for encryption.
		/// </summary>
		public byte[] IV 
		{
			internal protected get { return this.iv; }
			set 
			{
				if (!Object.Equals(this.iv, value)) 
					this.iv = value;
			}
		}

		
		/// <summary>
		/// Gets or sets the Key value for encryption.
		/// </summary>
		public string Key 
		{
			internal protected get { return this.key; }
			set
			{
				if (!Object.Equals(this.key, value))
					this.key = value;
			}
		}

		/// <summary>
		/// Gets or sets the connection string.
		/// </summary>
		/// <param name="name"> The name key of the connection string. </param>
		/// <returns> The connection value </returns>
		public string this[string name] 
		{
			get { return this.GetString(name); }
			set 
			{
				this.SetString(name, value);
			}
		}

		/// <summary>
		/// Gets the encryptor for the connection strings.
		/// </summary>
		/// <returns> Returns an object that implements <see cref="Amplify.Security.Cryptography.IEncryptable"/> </returns>
		protected virtual IEncryptable GetEncryptor()
		{
			return new Encryption64(this.Key, this.IV);
		}

		/// <summary>
		/// Gets the connection string by the name.
		/// </summary>
		/// <param name="name"> The name of the connection string. </param>
		/// <returns> The connection string, unencrypted if encrypted. </returns>
		protected string GetString(string name) 
		{
			if (definedStrings.ContainsKey(name))
				return definedStrings[name];

			string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[name].ConnectionString;
			
			if (this.isEncrypted)
			{
				if (connectionString.Length > -1)
					return this.GetEncryptor().Decrypt(connectionString);
				return "";
			}

			definedStrings.Add(name, connectionString);
			return connectionString;
		}

		/// <summary>
		/// Sets the connection string by name.
		/// </summary>
		/// <param name="name"> The name of the connection string. </param>
		/// <param name="value"> The connection string. </param>
		protected void SetString(string name, string value) 
		{
			if (definedStrings.ContainsKey(name))
				definedStrings[name] = value;
			else
				definedStrings.Add(name, value);

			Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			ConnectionStringsSection section = config.ConnectionStrings;
			
			if (isEncrypted)
				value = this.GetEncryptor().Encrypt(value);
			
			if (section.ConnectionStrings[name] == null)
				section.ConnectionStrings.Add(new ConnectionStringSettings(name, value));
			else
				section.ConnectionStrings[name].ConnectionString = value;
			
			config.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection("connectionStrings");
		}
	}
}
