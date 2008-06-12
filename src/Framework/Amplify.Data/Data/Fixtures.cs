using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
				var x = (from o in this.Items
						 where o.Value.Name.ToLower() == fixtureName.ToLower()
						 select o).FirstOrDefault();
				if (!x.Equals(null))
					return x.Value;
				return null;
			}
		}

		public static Fixtures New(string fixturesDirectory, Adapter adapter)
		{
			Fixtures fixtures = new Fixtures();
			List<string> files = (from o in Directory.GetFiles(fixturesDirectory) 
								  where Path.GetExtension(o) == ".xml" ||
								  Path.GetExtension(o) == ".fixture" || 
								  Path.GetExtension(o) == ".csv" ||
								  Path.GetExtension(o) == ".yml" select o).ToList();

			files.Each(o => Log.Debug(o));

			files.Each(file =>  
				Fixture.Read(file, adapter).Each(
					fixture =>  fixtures.Items.Add(fixture.Name, fixture)));

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
			Log.Debug("Deleting Fixtures For {0}".Fuse(this.TableName));
			this.Adapter.ExecuteNonQuery("DELETE FROM {0}".Fuse(
				this.Adapter.QuoteTableName(this.TableName)));
		}

		protected virtual void InsertFixtures()
		{
			Log.Debug("Inserting Fixtures For {0}".Fuse(this.TableName));
			Rows.Each(o =>
				this.Adapter.ExecuteNonQuery("INSERT INTO {0} ({1}) VALUES ({2})".Fuse(
					this.Adapter.QuoteTableName(this.TableName),
					o.Keys.Join(","),
					o.Values.Join(",")
				)));
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
