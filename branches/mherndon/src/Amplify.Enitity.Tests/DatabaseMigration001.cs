

namespace Amplify
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using Amplify.Data;
	using Amplify.Linq;


	public class DatabaseMigration : Migration 
	{
		public override DateTime CreatedOn
		{
			get { return DateTime.Parse("09/10/2009 05:00:00 PM"); }
		}

		public override void Up()
		{
			this.CreateDatabase();

			this.CreateTable("countries", table => {
				table
					.AddColumn("name",				DbTypes.String,		Limit => 100)
					.AddColumn("code",				DbTypes.String,		Limit => 4);
			});

			this.CreateTable("regions", table => {
				table
					.AddColumn("name",				DbTypes.String,		Limit => 100)
					.AddColumn("country_id",		DbTypes.Integer);
			});

			this.CreateTable("address_types", table => {
				table
					.AddColumn("name",				DbTypes.String,		Limit => 30)
					.AddColumn("description",		DbTypes.String,		Limit => 255);
			});

			this.CreateTable("address_address_types", table => {
				table.SetId(false)
					.AddColumn("address_id",		DbTypes.PrimaryKey)
					.AddColumn("address_type_id",	DbTypes.PrimaryKey);
			});


			this.CreateTable("addresses", table => {
				table
					.AddColumn("street_address",	DbTypes.String,		Limit => 150)
					.AddColumn("extended_address",	DbTypes.String,		Limit => 150)
					.AddColumn("post_office_box",	DbTypes.String,		Limit => 20)
					.AddColumn("locality",			DbTypes.String,		Limit => 100)
					.AddColumn("region_id",			DbTypes.Integer)
					.AddColumn("postal_code",		DbTypes.String,		Limit => 10)
					.AddColumn("country_id",		DbTypes.Integer);
			});

			
		}

		public override void Down()
		{
			this.DropDatabase();
		}
	}
}
