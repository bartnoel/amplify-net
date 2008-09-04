//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.ActiveRecord
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Text;

	using Amplify.Linq;

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
					return  Inflector.Singularize(this.Property.Name);
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
