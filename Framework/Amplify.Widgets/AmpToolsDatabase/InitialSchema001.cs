namespace AmpTools
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using Amplify.Data;
	using Amplify.Linq;

	public class InitialSchema001 : Migration 
	{
		public override void Up()
		{
			CreateTable("amptools_Clients", Hash.New(Force => true), delegate(TableDefinition t)
			{
				t.Column("Name",			@string,	Lenth => 120);
			});

			CreateTable("amptools_Projects", Hash.New(Force => true), delegate(TableDefinition t)
			{
				t.Column("Name",			@string,	Length => 100);
				t.Column("Description",		@string);
				t.Column("IsActive",		boolean,	Default => false);
			});

			AddIndex("amptools_Projects", new[] { "Name" });

			CreateTable("amptools_ClientsProjects", Hash.New(Force => true, PrimaryKey => false), delegate(TableDefinition t)
			{
				t.Column("ClientId",			guid);
				t.Column("ProjectId",			guid);
				t.PrimaryKey("ClientId");
				t.PrimaryKey("ProjectId");
			});

			CreateTable("amptools_ProjectDetails", Hash.New(Force => true), delegate(TableDefinition t)
			{
				t.Column("ProjectId",		guid);
				t.Column("StartDate",		datetime);
				t.Column("EndDate",			datetime);
				t.Column("Description",		text);
				t.Column("StackTypeId",		guid);		
			});
		}

		public override void Down()
		{
			DropTable("amptools_ProjectDetails");
			DropTable("amptools_ClientsProjects");
			DropTable("amptools_Projects");
			DropTable("amptools_Clients");
		}
	}
}
