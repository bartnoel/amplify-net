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

	public class SaveAsArgs
	{
		private object fileName;
		private object fileFormat = 0;
		private object lockComments = Type.Missing;
		private object password = Type.Missing;
		private object addToRecentFiles = Type.Missing;
		private object writePassword = Type.Missing;
		private object readOnlyRecommended = Type.Missing;
		private object embedTrueTypeFonts = Type.Missing;
		private object saveNativePictureFormat = Type.Missing;
		private object saveFormsData = Type.Missing;
		private object saveAsAOCELetter = Type.Missing;
		private object encoding = Type.Missing;
		private object insertLineBreaks = Type.Missing;
		private object allowSubstitutions = Type.Missing;
		private object lineEnding = Type.Missing;
		private object addBiDiMarks = Type.Missing;

		public object FileName 
		{
			get { return this.fileName; }
			set { this.fileName = value; }
		}

		public object FileFormat 
		{
			get { return this.fileFormat; }
			set { this.fileFormat = value; }
		}

		public object LockComments
		{
			get { return this.lockComments; }
			set { this.lockComments = value; }
		}

		public object Password
		{
			get { return this.password; }
			set { this.password = value; }
		}

		public object AddToRecentFiles
		{
			get { return this.addToRecentFiles; }
			set { this.addToRecentFiles = value; }
		}


		public object WritePassword
		{
			get { return this.writePassword; }
			set { this.writePassword = value; }
		}


		public object ReadOnlyRecommended
		{
			get { return this.readOnlyRecommended; }
			set { this.readOnlyRecommended = value; }
		}

		public object EmbedTrueTypeFonts
		{
			get { return this.embedTrueTypeFonts; }
			set { this.embedTrueTypeFonts = value; }
		}

		public object SaveNativePictureFormat
		{
			get { return this.saveNativePictureFormat; }
			set { this.saveNativePictureFormat = value; }
		}

		public object SaveFormsData
		{
			get { return this.saveFormsData; }
			set { this.saveFormsData = value; }
		}


		public object SaveAsAOCELetter
		{
			get { return this.saveAsAOCELetter; }
			set { this.saveAsAOCELetter = value; }
		}

		public object Encoding
		{
			get { return this.encoding; }
			set { this.encoding = value; }
		}

		public object InsertLineBreaks
		{
			get { return this.insertLineBreaks;  }
			set { this.insertLineBreaks = value; }
		}

		public object AllowSubstitutions
		{
			get { return this.allowSubstitutions; }
			set { this.allowSubstitutions = value; }
		}

		public object LineEnding  
		{
			get { return this.lineEnding; }
			set { this.lineEnding = value; }
		}

		public object AddBiDiMarks
		{
			get { return this.addBiDiMarks; }
			set { this.addBiDiMarks = value; }
		}

		
	}
}
