using System;
using System.Collections.Generic;
using System.Configuration;
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

		public void Test(string path, int? version)
		{
			Assembly lib = Assembly.LoadFile(path);
			List<Migration> migrations = new List<Migration>();
			foreach (Type type in lib.GetTypes())
			{
				if(type.IsSubclassOf(typeof(Migration)))
					migrations.Add((Migration)Activator.CreateInstance(type));
			}

			migrations.Sort(delegate(Migration a, Migration b)
			{
				return a.CreatedOn.CompareTo(b.CreatedOn);
			});




		}
		
	}
}
