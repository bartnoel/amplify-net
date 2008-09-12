namespace Fuse.Migrations
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using Amplify.Data;

	public class DatabaseSchema : Migration 
	{
		public override DateTime CreatedOn
		{
			get { return DateTime.Parse("9/11/2008 6:10:00 PM"); }
		}


		public override void Up()
		{
			EnvDTE90.Solution3 sol;
			
			this.CreateDatabase();
		}

		public override void Down()
		{
			
		}
	}
}
