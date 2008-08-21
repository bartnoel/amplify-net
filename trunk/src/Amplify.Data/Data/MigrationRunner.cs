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

			try
			{
				adapter.CreateDatabase();
			}
			catch (Exception ex)
			{
				// eat it
			}
			this.CreateTable(adapter);

			int migrateToVersion = (version.HasValue) ? version.Value : migrations.Count;
			int currentVersion = 0;

			if (!ApplicationContext.IsTesting)
			{
				string query = string.Format("SELECT CompatibleSchemaVersion FROM aspnet_SchemaVersions WHERE Feature = '{0}'",
					"ApplicationSchema");

				using (IDataReader dr = adapter.Select(query))
				{
					while (dr.Read())
					{
						currentVersion = int.Parse(dr.GetString(0));
					}
				}
			}

			if (migrateToVersion > currentVersion)
			{
				for (int i = currentVersion; i < migrateToVersion; i++)
					migrations[i].Up();

				adapter.Update("aspnet_SchemaVersions",
					new string[] { "CompatibleSchemaVersion" },
					new object[] { migrations.Count },
					" WHERE Feature = 'ApplicationSchema' ");
			}
			else
			{
				int i = 0;
				for (i = currentVersion; i > migrateToVersion; i--)
					migrations[i].Down();

				adapter.Update("aspnet_SchemaVersions",
					new string[] { "CompatibleSchemaVersion" },
					new object[] { i },
					" WHERE Feature = 'ApplicationSchema' ");
			}

		}


		public void CreateTable(Adapter adapter)
		{
			string tableName = "aspnet_SchemaVersions";

			if (!adapter.GetTableNames().Contains(tableName))
			{
				adapter.CreateTable(tableName, delegate(TableDefinition table)
				{
					table.Name = tableName;
					table.Id = false;
					table.Column(delegate(ColumnDefinition o) { 
						o.Name = "Feature";
						o.Type = SchemaBase.@string; 
						o.Limit = 128; 
					});
					table.Column(delegate(ColumnDefinition o) { 
						o.Name = "CompatibleSchemaVersion";
						o.Type = SchemaBase.@string;
						o.Limit = 128; 
					});
					table.Column("IsCurrentVersion", Adapter.boolean);
					table.Options += "\n\t, CONSTRAINT PK_aspnet_SchemaVersions PRIMARY KEY(Feature,CompatibleSchemaVersion) ";
				});

				adapter.Insert("aspnet_SchemaVersions", new string[] {"Feature", 
					"CompatibleSchemaVersion", "IsCurrentVersion"}, "ApplicationSchema", 0, 1);
			}
		}
	}
}
