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

	using Excel = Microsoft.Office.Interop.Excel;

	public class SaveAsParams
	{

		private object fileName = "";
		private object fileFormat = Type.Missing;
		private object password = Type.Missing;
		private object writeResPassword = Type.Missing;
		private object readOnlyRecommended = Type.Missing;
		private object createBackup = Type.Missing;
		private object accessMode = Type.Missing;
		private object conflictResolution = Type.Missing;
		private object addToMru = Type.Missing;
		private object textCodepage = Type.Missing;
		private object textVisualLayout = Type.Missing;
		private object local = Type.Missing;

		public object FileName { get { return this.fileName; } set { this.fileName = value; } }
		public object FileFormat { get { return this.fileFormat; } set { this.fileFormat = value; } }
		public object Password { get { return this.password; } set { this.password = value; } }
		public object WriteResPassword { get { return this.writeResPassword; } set { this.writeResPassword = value; } }
		public object ReadOnlyRecommended { get { return this.readOnlyRecommended; } set { this.readOnlyRecommended = value; } }
		public object CreateBackup { get { return this.createBackup; } set { this.createBackup = value; } }
		public object AccessMode { get { return this.accessMode; } set { this.accessMode = value; } }
		public object ConflictResolution { get { return this.conflictResolution; } set { this.conflictResolution = value; } }
		public object AddToMru { get { return this.addToMru; } set { this.addToMru = value; } }
		public object TextCodepage { get { return this.textCodepage; } set { this.textCodepage = value; } }
		public object TextVisualLayout { get { return this.textVisualLayout; } set { this.textVisualLayout = value; } }
		public object Local { get { return this.local; } set { this.local = value; } }

		
	}
}
