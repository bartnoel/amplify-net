
namespace Amplify.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Reflection;

	using Amplify.Linq;

	public class Migrator
	{
		private List<Script> scripts = new List<Script>();


		private class Script
		{
			public string Version { get; set; }
			public string Name { get; set; }
			public string FullName { get; set; }
		}

		public Adapter Adapter { get; set; } 

		public MigrationDirection Direction { get; set; }

		private void GetMigrationsFromAssembly(string filename)
		{
			Assembly asm =	Assembly.LoadFile("");

			Type[] types =	asm.GetTypes();
			foreach(Type type in types) {
				if(type.IsSubclassOf(typeof(Migration))) {
					string name = "", version = "";
					type.Name.Gsub(@"(\[A-Za-z_].*)(\d.*)", delegate(Match match) {
						 name =	match.Groups[0].Value;
						 version = match.Groups[1].Value;
						 return match.Value;
					});
					this.scripts.Add(new Script()
					{
						FullName = type.Name, 
						Name =  name,
						Version = version
					});
				}
			}
		}


		public void GetCurrentVersion()
		{

		}

		public void Migrate()
		{
			
		}
	}

	public enum MigrationDirection
	{
		Up,
		Down
	}
}
