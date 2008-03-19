using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;

using Amplify.Data;
using Amplify.Linq;

namespace Amplify.Fixtures
{
	public class FixtureParser
	{


		public static void Parse(string filename, Adapter adapter) 
		{
			XmlDocument document = new XmlDocument();
			document.Load(filename);
			XmlNodeList list = document.SelectNodes("//");
			Dictionary<string, List<XmlNode>> tables = new Dictionary<string,List<XmlNode>>();

			foreach (XmlNode node in list)
			{
				if (tables.ContainsKey(node.Name))
					tables[node.Name].Add(node);
				else
					tables[node.Name] = new List<XmlNode>() { node };
			}

			tables.Each(delegate(KeyValuePair<string, List<XmlNode>> table)
			{
				InsertTable(table, adapter);
			});
		}


		private static void InsertTable(KeyValuePair<string, List<XmlNode>> table, Adapter adapter)
		{
			table.Value.Each(delegate(XmlNode row) {
				string query = "INSERT INTO {0} SET (".Inject(adapter.QuoteTableName(table.Key));
				string holder = " VALUES ( ";
				List<object> values = new List<object>();
				
				int count = 0;
				foreach(XmlNode field in row.ChildNodes) 
				{
					query += "\t{0},\n".Inject(field.Name);
					holder += "\t{0},\n".Inject(count);
					values.Add(field.Value);
					count++;
				}

				query = query.TrimEnd(",\n".ToCharArray()) + ") ";
				query += holder.TrimEnd(",\n".ToCharArray()) + ") ";
				adapter.ExecuteNonQuery(query, values.ToArray());
			});
		}
	}
}
