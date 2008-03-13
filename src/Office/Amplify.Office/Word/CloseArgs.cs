//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Office.Word
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	public class CloseArgs
	{
		private object saveChanges = Type.Missing;
		private object originalFormat = Type.Missing;
		private object routeDocument = Type.Missing;

		public object SaveChanges
		{
			get { return this.saveChanges; }
			set { this.saveChanges = value; }
		}

		public object OriginalFormat 
		{
			get { return this.originalFormat; }
			set { this.originalFormat = value; }
		}

		public object RouteDocument 
		{
			get { return this.routeDocument; }
			set { this.routeDocument = value; }
		}

		public CloseArgs()
		{
			this.SaveChanges = Type.Missing;
			this.OriginalFormat = Type.Missing;
			this.RouteDocument = Type.Missing;
		}
	}
}
