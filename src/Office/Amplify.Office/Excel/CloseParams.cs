//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace BlueRidgeEsop.Office2003.Excel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class CloseParams
	{
		private object saveChanges = Type.Missing;
		private object fileName = Type.Missing;
		private object routeWorkbook = Type.Missing;

		public object SaveChanges 
		{
			get { return this.saveChanges; }
			set { this.saveChanges = value; }
		}

		public object FileName 
		{
			get { return this.fileName; }
			set { this.fileName = value; }
		}


		public object RouteWorkbook
		{
			get { return this.routeWorkbook; }
			set { this.routeWorkbook = value; }
		}

	}
}
