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
			{ "Scope", Scope.All },
			{ "Where", new object[] { } },
			{ "Includes", new Hash() },
			{ "Select", "*" },
			{ "Distinct", "" }
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
			get { return (object[])this["Where"]; }
			set { this["Where"] = value; }
		}

		public int? Limit
		{
			get { return (int?)this["Limit"]; }
			set { this["Limit"] = value; }
		}

		public int? Offset
		{
			get { return (int?)this["Offset"]; }
			set { this["Offset"] = value; }
		}

		public string Order
		{
			get { return (this["Order By"] as string); }
			set { this["Order By"] = value; }
		}

		public string Group
		{
			get { return (this["Group By"] as string); }
			set { this["Group By"] = value; }
		}

		public bool IsDistinct
		{
			get { return (this["Distinct"] != ""); }
			set
			{
				if (value)
					this["Distinct"] = "DISTINCT";
				else
					this["Distinct"] = "";
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
			get { return (Scope)this["Scope"]; }
			set { this["Scope"] = value; }
		}

		public Hash Include
		{
			get { return (this["Includes"] as Hash); }
			set { this["Includes"] = value; }
		}

		#region IOptions Members



		#endregion

		#region IOptions Members


		string Amplify.Data.IOptions.Select
		{
			get { return this["Selection"] as string; }
			set { this["Selection"] = value; }
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
				string value = (this["As"] as string);
				if (!string.IsNullOrEmpty(value))
					return value;
				return (this["FROM"] as string);
			}
			set { this["As"] = value; }
		}

		string Amplify.Data.IOptions.From
		{
			get { return (this["From"] as string); }
			set { this["From"] = value; }
		}

		string Amplify.Data.IOptions.Where
		{
			get { return null; }
			set { this["Temp"] = null; }
		}

		#endregion


	}
}