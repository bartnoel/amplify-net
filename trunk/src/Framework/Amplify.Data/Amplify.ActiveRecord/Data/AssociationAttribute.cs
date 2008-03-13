//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace Amplify.Data
{
	using System;
	using System.Collections.Generic;
	using System.Collections;
	using System.Linq;
	using System.Text;
	using System.Reflection;

	using Amplify.Linq;

	public enum AssociationDependency
	{
		None,
		Destroy,
		Nullify,
		Delete
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)] 
	public class AssociationAttribute : System.Attribute
	{
		public string AssociationId { get; set; }
		public Type ClassType { get; set; }
		public string Conditions { get; set; }
		public string Order { get; set; }
		public string ForeignKey { get; set; }
		public IDictionary<string, object> Includes { get; set; }


		public override object TypeId
		{
			get
			{
				return this.AssociationId;
			}
		}

		public void SetValues(PropertyInfo info)
		{
			this.AssociationId = info.Name;
			if (this.ClassType == null)
			{
				if (info.PropertyType.IsInstanceOfType(typeof(IList)))
					this.ClassType = info.PropertyType.GetProperty("Item").PropertyType;
				else
					this.ClassType = info.PropertyType;
			}
		}
	}
}
