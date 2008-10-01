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
			this.CreateTable("projects", table => {
				table
					.AddColumn("name",					DbTypes.String,		Limit => 100)
					.AddColumn("description",			DbTypes.String);
			});

			this.CreateTable("databases", table => {
				table
					.AddColumn("name",					DbTypes.String,		Limit => 100)
					.AddColumn("server",				DbTypes.String,		Limit => 100,	Unique => true)
					.AddColumn("database",				DbTypes.String)
					.AddColumn("connection_string",		DbTypes.Text);
			});

			this.CreateTable("projects_databases", table => {
				table.SetId(false)
					.AddColumn("project_id",			DbTypes.PrimaryKey)
					.AddColumn("database_id",			DbTypes.PrimaryKey);
			});
		}

		public override void Down()
		{
			this.DropDatabase();
		}
	}
}
