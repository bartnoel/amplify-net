using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Entities
{
	using Amplify.Models;
	using Amplify.Data;


	[Table("Projects")]
	public class Project : Base<Project> 
	{
		[Column("Id", IsPrimaryKey=true)]
		public Guid Id
		{
			get { return (Guid)this["Id"]; }
			set { this["Id"] = value; }
		}

		[Column("Name", DefaultValue = "")]
		public string Name
		{
			get { return (string)this["Name"]; }
			set { this["Name"] = value; }
		}

		[Column("Description", DefaultValue = "")]
		public string Description
		{
			get { return (string)this["Description"]; }
			set { this["Description"] = value; }
		}

		[Column("CreatedOn")]
		public DateTime CreatedOn
		{
			get { return (DateTime)this["CreatedOn"]; }
			set { this["CreatedOn"] = value; }
		}
	
	}
}
