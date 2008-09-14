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

	[Table(Name="projects")]
	public class Project : Base<Project>
	{
		private EntitySet<Database> databases;

		public Project()
		{
			this.Id = 0;
			this.Name = "";
			this.Description = "";
			this.databases = new EntitySet<Database>(AddDatabase, RemoveDatabase); 
		}

		public override object UniqueId
		{
			get { return this.Id; }
		}

		protected void AddDatabase(Database item)
		{
			ProjectDatabase pd= new ProjectDatabase();
			pd.DatabaseId = item.Id;
			pd.ProjectId = this.Id;
			Db.SubmitChanges();
		}

		protected void RemoveDatabase(Database item)
		{
			var pd = (from o in Db.GetTable<ProjectDatabase>() 
					  where o.ProjectId == this.Id && 
					  o.DatabaseId == item.Id select o)
					  .SingleOrDefault();

			if (pd != null)
				Db.GetTable<ProjectDatabase>().DeleteOnSubmit(pd);
		}

		public List<Database> Databases
		{
			get { 
				if(!this.databases.HasLoadedOrAssignedValues)
					this.databases.Assign((from d in Db.GetTable<Database>() 
									  join pd in Db.GetTable<ProjectDatabase>() on d.Id equals pd.DatabaseId 
									  where pd.ProjectId == this.Id select d));
				return this.databases.ToList();
			}
		}

		[Column(Name="id", IsPrimaryKey = true)]
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

		[Column(Name = "description")]
		public string Description
		{
			get { return this.Get("description") as string; }
			set { this.Set("name", value); }
		}

		
	}
}
