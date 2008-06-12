using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuse.Models
{

	public enum DependentAction
	{
		Destroy,
		Nullify,
		Delete,
	}

	public class HasOneAttribute : AssocationAttribute, IHasAssocation
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
