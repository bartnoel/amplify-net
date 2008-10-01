using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuse.Models
{
	public class ColumnView
	{
		public string Name { get; set; }

		public string Type { get; set; }

		public int? Limit { get; set; }

		public int? Precision { get; set; }

		public int? Scale { get; set; }

	}
}
