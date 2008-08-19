using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;

using Amplify.Data;
using Amplify.Linq;

namespace Amplify.Fixtures
{
	public class MigrationRunner
	{
		public void Run(string path)
		{
			this.Run(path, null);
		}

		public void Run(string path, int? version)
		{
			this.Run(path, version, null);
		}

		public void Run(string path, int? version, string database)
		{
			Assembly lib = Assembly.LoadFile(path);
			List<Migration> migrations = new List<Migration>();
			Adapter adapter = Adapter.Get(database);

			foreach (Type type in lib.GetTypes())
			{
				if(type.IsSubclassOf(typeof(Migration))) 
				{
					Migration migration =	(Migration)Activator.CreateInstance(type);
					migration.Adapter = adapter;
					migrations.Add(migration);
					migration.Version = migrations.Count; 
				}
					
			}
			
			migrations.Sort(delegate(Migration a, Migration b)
			{
				return a.CreatedOn.CompareTo(b.CreatedOn);
			});

			this.CreateTable(adapter);

			int migrateToVersion = (version.HasValue) ? version.Value : migrations.Count;
			int currentVersion = 0;

			string query = string.Format("SELECT CompatibleSchemaVersion FROM aspnet_SchemaVersions WHERE Feature = '{0}'",
				"ApplicationSchema");

			using(IDataReader dr = adapter.Select(query))  
			{
				while (dr.Read())
				{
					currentVersion = dr.GetInt32(0);
				}
			}

			if (migrateToVersion > currentVersion)
			{
				for (int i = currentVersion; i < migrateToVersion; i++)
					migrations[i].Up();
			}
			else
			{
				for (int i = currentVersion; i > migrateToVersion; i--)
					migrations[i].Down();
			}

		}


		public void CreateTable(Adapter adapter)
		{
			string tableName = "aspnet_SchemaVersions";

			if (!adapter.GetTableNames().Contains(tableName))
			{
				adapter.CreateTable(tableName,
				null, delegate(TableDefinition table)
				{
					table.Column("Feature", Adapter.@string, (o) => { o.Limit = 128; });
					table.Column("CompatibleSchemaVersion", Adapter.@string, (o) => { o.Limit = 128; });
					table.Column("IsCurrentVersion", Adapter.boolean);
				});

				adapter.Insert("aspnet_SchemaVersions", new string[] {"Feature", 
				"CompatibleSchemaVersion", "IsCurrentVersion"}, "ApplicationSchema", 0, 1);
			}
		}
	}
}
