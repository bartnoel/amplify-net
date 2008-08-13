using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuse.Models
{
	using Amplify.ActiveRecord;
	using Amplify.Data.Validation;

	public class Project : Base<Project>
	{ 
		[Column(IsPrimaryKey = true, Default = 0)]
		public int Id
		{
			get { return (int)this["Id"]; }
			protected set { this["Id"] = value; } 
		}

		[ValidatePresence]
		[Column(Default = "", Limit = 100)]
		public string Name
		{
			get { return (string)this["Name"]; }
			set { this["Name"] = value; }
		}

		[ValidatePresence]
		[Column(Default = "", Limit = 255)]
		public string Description
		{
			get { return (string)this["Description"]; }
			set { this["Description"] = value; }
		}

		[Column(Default = 0)]
		protected int Type
		{
			get { return (int)this["Type"]; }
			set { this["Type"] = value; }
		}
	}
}
