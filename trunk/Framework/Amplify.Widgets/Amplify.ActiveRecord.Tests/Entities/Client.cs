using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Entities
{

	using Amplify.Data;
	using Amplify.Models;

	[Table(Name="Clients")]
	public class Client : Base<Client>
	{

		[Column("Id", IsPrimaryKey= true)]
		public Guid Id
		{
			get { return (Guid)this["Id"]; }
		}

		[Column("Name", DefaultValue="")]
		public string Name
		{
			get { return (String)this["Name"]; }
			set { this["Name"] = value; }
		}

		[Column("CreatedOn")]
		public DateTime CreatedOn
		{
			get { return (DateTime)this["CreatedOn"]; }
			set { this["CreatedOn"] = value; }
		}


	}
}
