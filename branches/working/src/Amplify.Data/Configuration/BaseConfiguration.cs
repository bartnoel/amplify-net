//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace Amplify.ActiveRecord.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Linq;
	using System.Text;

	public class BaseConfiguration : ConfigurationSection  
	{
		private const string c_schemaTableName = "schemaTableName";
		protected const string c_connectionStringName = "connectionStringName"; 

		protected override void Init()
		{
			base.Init();
			
		}

		[ConfigurationProperty(c_schemaTableName, DefaultValue="amplify_SchemaVersion")]
		public string SchemaTableName
		{
			get { return this[c_schemaTableName].ToString(); }
			set { this[c_schemaTableName] = value; }
		}

		[ConfigurationPermission(c_connectionStringName, DefaultValue="development")]
		public virtual string ConnectionStringName
		{
			get { return this[c_connectionStringName].ToString(); }
			set { this[c_connectionStringName] = value; }
		}
	}
}
