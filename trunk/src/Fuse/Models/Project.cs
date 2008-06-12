using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuse.Models
{
	using Amplify.Data;


	public partial class ProjectSetting : Base<ProjectSetting>
	{

	}


	public partial class Project : Base<Project>
	{

		[HasOne] 
		public ProjectSetting LocalSettings
		{
			get;
			set; 
		}

	}
}
