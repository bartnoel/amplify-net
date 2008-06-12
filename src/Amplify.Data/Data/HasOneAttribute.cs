using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data
{

	public enum DependentAction
	{
		Destroy,
		Nullify,
		Delete,
	}

	public class HasOneAttribute : AssociationAttribute 
	{
		public HasOneAttribute()
			: base()
		{
			this.Dependent = DependentAction.Nullify;
			this.Include = new string[] { };
		}

		public DependentAction Dependent { get; set; }

		public string[] Include { get; set; }

		public string As
		{
			get
			{
				return this.PropertyName;
			}
		}
	}
}
