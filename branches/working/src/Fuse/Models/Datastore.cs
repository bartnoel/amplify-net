﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuse.Models
{
	public class DataStore
	{

		public string Name { get; set; }

		public string Description { get; set; }


		public List<TableView> Tables
		{
			get { }
		}

	}
}
