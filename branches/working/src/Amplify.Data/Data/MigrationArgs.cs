﻿

namespace Amplify.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Reflection;

	/// <summary>
	/// Provides the possible arguements to the <see cref="Amplify.Data.MigrationCommand"/>
	/// that runs the database migrations.
	/// </summary>
	public class MigrationArgs
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="MigrationArgs"/> class.
		/// </summary>
		public MigrationArgs()
		{
			this.PathOrAssemblyName = "";
			this.Database = "";
		}

		/// <summary>
		/// Gets or sets the name of the path or assembly.
		/// </summary>
		/// <value>The name of the path or assembly.</value>
		public string PathOrAssemblyName { get; set; }

		public Assembly Assembly { get; set; }

		public bool LowerNaming { get; set; }

		public Adapter Adapter { get; set; }

		/// <summary>
		/// Gets or sets the version.
		/// </summary>
		/// <value>The version.</value>
		public int? Version { get; set; }

		/// <summary>
		/// Gets or sets the database.
		/// </summary>
		/// <value>The database.</value>
		public string Database { get; set; }
	}
}