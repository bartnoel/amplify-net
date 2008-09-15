using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuse.Models
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using Amplify.Data.Linq;
	using Amplify.Linq;

	public class Database : Base<Database>
	{

		public Database()
		{

		}

		public override object UniqueId
		{
			get { return this.Id; }
		}



		[Column(Name = "id", IsPrimaryKey = true)]
		public int Id
		{
			get { return (int)this.Get("id"); }
			set { this.Set("id", value); }
		}

		[Column(Name = "name")]
		public string Name
		{
			get { return this.Get("name") as string; }
			set { this.Set("name", value); }
		}

		[Column(Name = "server")]
		public string Server
		{
			get { return this.Get("server") as string; }
			set { this.Set("server", value); }
		}


		[Column(Name = "connection_string")]
		public string ConnectionString
		{
			get { return this.Get("connection_string") as string; }
			set { this.Set("connection_string", value); }
		}

	}
}
