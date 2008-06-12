using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data
{
	using Amplify.Linq;
	using System.Reflection;

	public class AssociationAttribute : System.Attribute 
	{
		private string className = "";
		private string conditions = "";
		private string foreignKey = "";

		public AssociationAttribute()
		{
			this.Order = "";
			this.PropertyName = "";
			this.Conditions = "";
		}

		public string PropertyName { get; set; }

		public string ClassName 
		{
			get
			{
				if (string.IsNullOrEmpty(className))
					return this.PropertyName.Singularize();
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

		public PropertyInfo PropertyInfo { get; set; }

		public override object TypeId
		{
			get
			{
				return this.PropertyName;
			}
		}
	}
}
