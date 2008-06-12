using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuse.Models
{
	public class BelongsToAttribute : AssocationAttribute 
	{

		public BelongsToAttribute()
			: base()
		{
			this.Include = new string[] {};
			this.Polymorphic = false;
		}

		public string[] Include { get; set; }

		public bool Polymorphic { get; set; }
	}
}
