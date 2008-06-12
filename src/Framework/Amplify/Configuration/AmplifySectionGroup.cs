//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------



namespace Amplify.Configuration 
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Configuration;
	using System.ComponentModel;
	using System.Web.Configuration;

	/// <summary>
	/// The basic application section that should be inherited by more specific applications like
	/// web, wpf, mobile, windows.. etc
	/// </summary>
	/// <remarks>
	///		<para>
	///		This class implements <see cref="System.ComponentModel.INotifyPropertyChanged" /> since
	///		there could be a valid need of watching some of the properties of this class being
	///		changed. That way the application can adjust accordingly.
	///     </para>
	/// </remarks>
	public class AmplifySection : System.Configuration.ConfigurationSection, INotifyPropertyChanged
	{
        private const string c_applicationName = "applicationName";
        private const string c_connectionStringName = "connectionStringName";
        private const string c_remoteConnectionStringName = "remoteConnectionStringName";
		private const string c_isConnectionStringEncrypted = "isConnectionStringEncrypted";
		private const string c_mode = "mode";
		private const string c_overrideMode = "overrideMode";
		private const string c_isLinqEnabled = "isLinqEnabled";

		


		/// <summary>
		/// Gets or sets if the connection strings of the applicaiton is encrypted or not.
		/// </summary>
		[ConfigurationProperty(
			c_isConnectionStringEncrypted,
			DefaultValue = false,
			IsRequired = false)]
		public bool IsConnectionStringEncrypted 
		{
			get {
				if (!this.OverrideMode)
					return (this.Mode.ToLower() == "production");
				return (bool)this[c_isConnectionStringEncrypted]; 
			}
			set {
				if (this.OverrideMode)
				{
					if (!Object.Equals(this[c_isConnectionStringEncrypted], value))
					{
						this[c_isConnectionStringEncrypted] = value;
						NotifyPropertyChange("IsConnectionStringEncrypted");
					}
				}
			}
		}

	 
		[ConfigurationProperty("plugins", IsDefaultCollection= false)]
		public PluginsCollection Plugins
		{
			get {
				object value = this["plugins"];
				if (value == null)
				{
					value = new PluginsCollection();
					this["plugins"] = value;
				}
				return (PluginsCollection)value; 
			}
			set { this["plugins"] = value; }
		}

		[ConfigurationProperty(
					c_overrideMode,
					DefaultValue = false,
					IsRequired = false)]
		public bool OverrideMode
		{
			get { return (bool)this[c_overrideMode]; }
			set { this[c_overrideMode] = value; }
		}

		[ConfigurationProperty(
			c_mode,
			DefaultValue = "development",
			IsRequired = false)]
		public virtual string Mode
		{
			get {
				return this[c_mode].ToString();
			}
			set
			{
				if (!Object.Equals(value, this[c_mode]))
				{
					this[c_mode] = value;
					this.NotifyPropertyChange("Mode");
				}
			}
		}

		[ConfigurationProperty(
			c_isLinqEnabled,
			DefaultValue= false,
			IsRequired= false)]
		public virtual bool IsLinqEnabled
		{
			get
			{
				return (bool)this[c_isLinqEnabled];
			}
			set
			{
				if(!Object.Equals(value, this[c_isLinqEnabled])) 
				{
					this[c_isLinqEnabled] = value;
					this.NotifyPropertyChange("IsLinqEnabled");
				}
			}
		}



		/// <summary>
		/// Gets or sets the name of the application.
		/// </summary>
		/// <value>The name of the application.</value>
		[ConfigurationProperty(
		   c_applicationName,
			DefaultValue = "Amplify.Net Application",
			IsRequired = true)]
		public string ApplicationName
		{
			get { return (string)this[c_applicationName]; }
			set { this[c_applicationName] = value; }
		}

		/// <summary>
		/// Gets or sets the local connection string.
		/// </summary>
		/// <value>The local connection string.</value>
        [ConfigurationProperty(
            c_connectionStringName,
            DefaultValue = "development",
            IsRequired = false)]
        public string ConnectionStringName
		{
            get {
				if (!this.OverrideMode) {
					return this.Mode.ToLower();
				}
				return (string)this[c_connectionStringName]; 
			}
			set
			{
				if (this.OverrideMode)
				{
					if (!Object.Equals(this[c_connectionStringName], value))
					{
						this[c_connectionStringName] = value;
						this.NotifyPropertyChange("ConnectionStringName");
					}
				}
			}
        }

		/// <summary>
		/// Gets or sets the remote connection string.
		/// </summary>
		/// <value>The remote connection string.</value>
        [ConfigurationProperty(
            c_remoteConnectionStringName,
            DefaultValue = "remote",
            IsRequired = false)]
        public string RemoteConnectionStringName
		{
            get { return (string)this[c_remoteConnectionStringName]; }
            set {
				if (!Object.Equals(this[c_remoteConnectionStringName], value))
				{
					this[c_remoteConnectionStringName] = value;
					this.NotifyPropertyChange("RemoveConnectionStringName");
				}
			}
        }

		/// <summary>
		/// Fires the property changed event.
		/// </summary>
		/// <param name="propertyName"> The name of the property that was changed. </param>
		protected void NotifyPropertyChange(string propertyName)
		{
			PropertyChangedEventHandler eh = this.PropertyChanged;
			if (eh != null)
				eh(this, new PropertyChangedEventArgs(propertyName));
		}

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}
