

namespace Amplify.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using Amplify.Linq;

	public class Options
	{
		public List<object> Conditions { get; set; }
		public string Select { get; set; }
		public bool Distinct { get; set; }
		public string Group { get; set; }
		public string Order { get; set; }
		public int? Limit { get; set; }
		public int? Offset { get; set; }
		public bool ReadOnly { get; set; }
		public string Join { get; set; }
		public string Include { get; set; }
		public string From { get; set; }
		internal string Where { get; set; }

		

		public Options(params Func<object, object>[] options)
		{
			Hash hash = Hash.New(options);
			if (hash.ContainsKey("Conditions"))
			{
				if (hash["Conditions"] is IEnumerable<object>)
				{
					List<object> list = ((IEnumerable<object>)hash["Conditions"]).ToList();
					this.Where = list.First().ToString();
					list.RemoveAt(0);
					this.Conditions = list;
				}
				else if (hash["Conditions"] is string)
				{
					this.Where = hash["Conditions"].ToString();
				}
			}
			this.Group = hash["Group"].ToString();
			this.Order = hash["Order"].ToString();
			this.Limit = (int)hash["Limit"];
			this.Offset = (int)hash["Offset"];
			this.ReadOnly = (hash["ReadOnly"] == null) ? false : (bool)hash["ReadOnly"];
			this.Select = (hash["Select"] == null) ? "*" : hash["Select"].ToString();
			this.Distinct = (hash["Distinct"] == null) ? false : (bool)hash["Distinct"]; 
		}

		
	}
}
