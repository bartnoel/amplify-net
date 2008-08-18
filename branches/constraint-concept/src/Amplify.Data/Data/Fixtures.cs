using System;
using System.Collections.Generic;
using System.IO;

using System.Text;
using System.Xml;

using Amplify.Diagnostics;
using Amplify.Linq;

namespace Amplify.Data
{
	public class Fixtures
	{
		protected string FixturesDirectory { get; set; }
		protected Dictionary<string, Fixture> Items { get; set; }

		protected Fixtures()
		{
			this.Items = new Dictionary<string, Fixture>();
		}

		public int Count
		{
			get { return this.Items.Count; }
		}

		public Fixture this[string fixtureName]
		{
			get {
				foreach(KeyValuePair<string, Fixture> item in this.Items)
					if(item.Key.ToLower() == fixtureName.ToLower())
						return item.Value;
				return null;
			}
		}

		public static Fixtures New(string fixturesDirectory, Adapter adapter)
		{
			Fixtures fixtures = new Fixtures();

			string[] extensions = new string[] { ".xml",  ".fixture", ".csv", ".yml" };
			List<string> files = new List<string>();

			foreach (string file in Directory.GetFiles(fixturesDirectory))
				foreach (string extension in extensions)
					if (Path.GetExtension(file).ToLower() == extension)
						files.Add(file);

			foreach (string file in files)
			{
				Log.Debug("Fixture Added: " + file);
				foreach (Fixture fixture in Fixture.Read(file, adapter))
					fixtures.Items.Add(fixture.Name, fixture);
			}

			return fixtures;
		}
	}

	public class Fixture 
	{

		public string Name { get; set; }
		public string TableName { get; set; }
		protected string FileName { get; set; }
		protected Adapter Adapter { get; set; }
		public List<Hash> Rows { get; set; }

		public virtual void RenewFixtures()
		{
			this.DeleteFixtures();
			this.InsertFixtures();
		}

		protected virtual void DeleteFixtures()
		{
			Log.Debug(string.Format("Deleting Fixtures For {0}",this.TableName));
			this.Adapter.ExecuteNonQuery(string.Format("DELETE FROM {0}",
				this.Adapter.QuoteTableName(this.TableName)));
		}

		protected virtual void InsertFixtures()
		{
			Log.Debug(string.Format("Inserting Fixtures For {0}", this.TableName));
			foreach (Hash o in this.Rows)
			{
				this.Adapter.ExecuteNonQuery(
					string.Format("INSERT INTO {0} ({1}) VALUES ({2})",
						EnumerableUtil.Join<string>(o.Keys, ","),
						EnumerableUtil.Join<object>(o.Values, ",")));
			}
		}

		protected static IEnumerable<Fixture> LoadFromXml(string fileName, Adapter adapter)
		{
			List<Fixture> fixtures = new List<Fixture>();
			XmlDocument document = new XmlDocument();
			document.Load(fileName);
			XmlNodeList tables = document.SelectNodes("tables/table");
			foreach (XmlNode table in tables)
			{
				Log.Debug(Path.GetFileNameWithoutExtension(fileName));
				Fixture fixture = new Fixture()
				{
					Name = Path.GetFileNameWithoutExtension(fileName),
					FileName = fileName,
					TableName = table.Attributes["name"].Value.ToString(),
					Adapter = adapter
				};
				fixtures.Add(fixture);
				fixture.Rows = new List<Hash>();
				for(int i = 0; i < table.ChildNodes.Count; i++)
				{
					Hash row = Hash.New();
					fixture.Rows.Add(row);
					foreach (XmlNode field in table.ChildNodes[i].ChildNodes)
						row.Add(field.Name,adapter.Quote(field.InnerText));
					
					
				}
			}
			return fixtures;
		}


		protected static IEnumerable<Fixture> LoadFromCvs(string fileName, Adapter adapter)
		{
			throw new NotImplementedException();
		}

		protected static IEnumerable<Fixture> LoadFromYaml(string fileName, Adapter adapter)
		{
			throw new NotImplementedException();
		}


		public static IEnumerable<Fixture> Read(string fileName, Adapter adapter)
		{
			switch (Path.GetExtension(fileName).ToLower())
			{
				case ".xml":
				case ".fixture":
					return Fixture.LoadFromXml(fileName, adapter);
				case ".cvs":
					return Fixture.LoadFromCvs(fileName, adapter);
				case ".yml":
					return Fixture.LoadFromYaml(fileName, adapter);
				default:
					return Fixture.LoadFromXml(fileName, adapter);
			}
		}
	}
}
