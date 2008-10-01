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

	[Table(Name = "projects")]
	public class ProjectDatabase : Base<ProjectDatabase>
	{

		public ProjectDatabase()
		{
			this.ProjectId = 0;
			this.DatabaseId = 0;
		}

		public override object UniqueId
		{
			get { return this.ProjectId; }
		}



		[Column(Name = "project_id", IsPrimaryKey=true)]
		public int ProjectId
		{
			get { return (int)this.Get("project_id"); }
			set { this.Set("project_id", value); }
		}

		[Column(Name = "database_id", IsPrimaryKey=true)]
		public int DatabaseId
		{
			get { return (int)this.Get("database_id"); }
			set { this.Set("database_id", value); }
		}


	}
}
