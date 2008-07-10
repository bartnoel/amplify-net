using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuse.Models
{
	using Amplify.Data;

	public class ServerSchema
	{
		private List<DatabaseSchema> databases;
	

		public ServerSchema()
		{
			this.Type = "mssql";
			this.Version = "sqlexpress";
			this.NumericVersion = "2005";
			this.LoginType = "windows authentication";
			this.UserId = "";
			this.Password = "";
			this.Url = "";
			this.Timeout = 30;
		}

		public string Name { get { return this.Url; } }

		public string Version { get; set; }

		public string NumericVersion { get; set; }

		public string Type { get; set; }

		public string UserId { get; set; }

		public string Password { get; set; }

		public string Url { get; set; }

		public int Port { get; set; }

		public int Timeout { get; set; }

		public string LoginType { get; set; }


		public override string ToString()
		{
			var builder = Adapter.Add("").GetBuilder();
			if (this.Type == "mssql" && this.LoginType == "windows authentication")
			{
				builder["Server"] = this.Url;
				builder["Integrated Security"] = "SSPI";
			}
			else
			{
				builder["Server"] = this.Url;
				builder["User Id"] = this.UserId;
				builder["Password"] = this.Password;
			}
			if (this.Type != "mssql" && this.Type != "xml")
			{
				builder["Port"] = this.Port;
			}

			builder["Timeout"] = this.Timeout;


			return builder.ToString();
		}

		public List<DatabaseSchema> Databases
		{
			get {
				if (this.databases == null)
					this.databases = new List<DatabaseSchema>();
				return this.databases;
			}
		}

		public void Load()
		{
			this.Databases.AddRange(DatabaseSchema.Find(this.ToString()));
		}
		
	}
}
