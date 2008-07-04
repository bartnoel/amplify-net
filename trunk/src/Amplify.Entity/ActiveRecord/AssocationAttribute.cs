using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.ActiveRecord
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

		public virtual string ClassName 
		{
			get
			{
				if (string.IsNullOrEmpty(className))
					return this.Property.Name.Singularize();
				return this.className;
			}
			set { this.className = value; }
		}

		public virtual string ForeignKey { get; set; }

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
