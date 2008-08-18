//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Data
{
	using Amplify.Linq;

	public enum Scope
	{
		All,
		One
	}

	public class Options : Amplify.Data.IOptions
	{
		private Hash options;

		public Options(params object[] conditions) :this()
		{
			this.Conditions = conditions;
		}


		public Options()
		{
			this.options = new Hash() {
			{ "SCOPE", Scope.All },
			{ "WHERE", new object[] { } },
			{ "INCLUDES", new Hash() },
			{ "SELECT", "*" },
			{ "DISTINCT", "" }
		};
		}

		internal void Map(Hash options)
		{
			foreach (string key in options.Keys)
				this.options[key] = options[key];
		}

		public object this[string key]
		{
			get { return options[key]; }
			set { options[key] = value; }
		}

		public object[] Conditions
		{
			get { return (object[])this["WHERE"]; }
			set { this["WHERE"] = value; }
		}

		public int? Limit
		{
			get { return (int?)this["LIMIT"]; }
			set { this["LIMIT"] = value; }
		}

		public int? Offset
		{
			get { return (int?)this["OFFSET"]; }
			set { this["OFFSET"] = value; }
		}

		public string Order
		{
			get { return (this["ORDER BY"] as string); }
			set { this["ORDER BY"] = value; }
		}

		public string Group
		{
			get { return (this["GROUP BY"] as string); }
			set { this["GROUP BY"] = value; }
		}

		public bool IsDistinct
		{
			get { return (this["DISTINCT"] != ""); }
			set
			{
				if (value)
					this["DISTINCT"] = "DISTINCT";
				else
					this["DISTINCT"] = "";
			}
		}

		public object Id
		{
			get { return (this["Id"] as object); }
			set
			{
				this["Id"] = value;
				this.Scope = Scope.One;
			}
		}

		public Scope Scope
		{
			get { return (Scope)this["SCOPE"]; }
			set { this["SCOPE"] = value; }
		}

		public Hash Include
		{
			get { return (this["INCLUDES"] as Hash); }
			set { this["INCLUDES"] = value; }
		}

		#region IOptions Members



		#endregion

		#region IOptions Members


		string Amplify.Data.IOptions.Select
		{
			get { return this["SELECT"] as string; }
			set { this["SELECT"] = value; }
		}

		bool Amplify.Data.IOptions.ReadOnly
		{
			get { return (bool)this["ReadOnly"]; }
			set { this["ReadOnly"] = value; }
		}

		public string Join
		{
			get { return this["Join"] as string; }
			set { this["Join"] = value; }
		}



		string Amplify.Data.IOptions.As
		{
			get
			{
				string value = (this["AS"] as string);
				if (!string.IsNullOrEmpty(value))
					return value;
				return (this["FROM"] as string);
			}
			set { this["AS"] = value; }
		}

		string Amplify.Data.IOptions.From
		{
			get { return (this["FROM"] as string); }
			set { this["FROM"] = value; }
		}

		string Amplify.Data.IOptions.Where
		{
			get { return null; }
			set { this["Temp"] = null; }
		}

		#endregion


	}
}