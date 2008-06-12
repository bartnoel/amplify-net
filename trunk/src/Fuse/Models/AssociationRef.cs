using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuse.Models
{
	public class AssocationRef<T> where T :Base<T> 
	{
		private T entity = default(T);
		private bool set = false;
		private AssocationAttribute assocation;


		public bool IsAssigned
		{
			get { return this.set;  }
		}

		public bool IsDeferred { get; internal set; }


		public T Entity
		{
			get { return this.entity; }
			set {
				set = true;
				this.entity = value;
			}
		}

	}
}
