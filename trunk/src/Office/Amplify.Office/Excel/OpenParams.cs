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

	public class OpenParams
	{
		private string fileName = "";
		private object updateLinks = Type.Missing;
		private object readOnly = true;
		private object format = Type.Missing;
		private object password = Type.Missing;
		private object writeResPassword = Type.Missing;
		private object ignoreReadOnlyRecommended = Type.Missing;
		private object origin = Type.Missing;
		private object delimiter = Type.Missing;
		private object editable = Type.Missing;
		private object notify = Type.Missing;
		private object converter = Type.Missing;
		private object addToMru = Type.Missing;
		private object local = Type.Missing;
		private object corruptLoad { get; set; }

		public string FileName {
			get { return this.fileName;  }
			set { this.fileName = value; }
		}

		public object UpdateLinks { get { return this.updateLinks; } set { this.updateLinks = value; } }
		public object ReadOnly { get { return this.readOnly; } set { this.readOnly = value; } }
		public object Format { get { return this.format; } set { this.format = value; } }
		public object Password { get { return this.password; } set { this.password = value; } }
		public object WriteResPassword { get { return this.writeResPassword; } set { this.writeResPassword = value; } }
		public object IgnoreReadOnlyRecommended { get { return this.ignoreReadOnlyRecommended; } set { this.ignoreReadOnlyRecommended; } }
		public object Origin { get { return this.origin; } set { this.origin = value; } }
		public object Delimiter { get { return this.delimiter; } set { this.delimiter = value; } }
		public object Editable { get { return this.editable; } set { this.editable = value; } }
		public object Notify { get { return this.notify; } set { this.notify = value; } }
		public object Converter { get { return this.converter; } set { this.converter = value; } }
		public object AddToMru { get { return this.addToMru; } set { this.addToMru = value; } }
		public object Local { get { return this.local; } set { this.local = value; } }
		public object CorruptLoad { get { return this.corruptLoad; } set { this.corruptLoad = value; } }


	}
}
