using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuse.Models
{
	using Amplify.Linq;
	using System.Reflection;

	public class AssocationAttribute : System.Attribute 
	{
		private string className = "";
		private string conditions = "";
		private string foreignKey = "";

		public AssocationAttribute()
		{
			this.Order = "";

			this.Conditions = "";
		}

		public string ClassName 
		{
			get
			{
				if (string.IsNullOrEmpty(className))
					return this.Property.Name.Singularize();
				return this.className;
			}
			set { this.className = value; }
		}

		public string ForeignKey
		{
			get
			{
				if (string.IsNullOrEmpty(this.foreignKey))
					return this.ClassName + "Id";
				return this.foreignKey;
			}
			set
			{
				this.foreignKey = value;
			}
		}


		public string Conditions { get; set; }

		public string Order { get; set; }

		public PropertyInfo Property { get; set; }

		public override object TypeId
		{
			get
			{
				return this.Property.Name;
			}
		}
	}
}
