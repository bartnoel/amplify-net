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

	public class OpenArgs
	{
		private object filename;
		private object confirmConversions = Type.Missing;
		private object readOnly = Type.Missing;
		private object addToRecentFiles = Type.Missing;
		private object passwordDocument = Type.Missing;
		private object passwordTemplate = Type.Missing;
		private object revert = Type.Missing;
		private object writePasswordDocument = Type.Missing;
		private object writePasswordTemplate = Type.Missing;
		private object format = Type.Missing;
		private object encoding = Type.Missing;
		private object visible = Type.Missing;
		private object openAndRepair = Type.Missing;
		private object documentDirection = Type.Missing;
		private object noEncodingDialog = Type.Missing;
		private object xmlTransformation = Type.Missing;

		public object FileName 
		{
			get { return this.filename; }
			set { this.filename = value;} 
		}

		public object ConfirmConversions 
		{
			get { return this.confirmConversions; }
			set { this.confirmConversions = value; }
		}

		public object ReadOnly 
		{
			get { return this.readOnly; }
			set { this.readOnly = value; }
		}

		public object AddToRecentFiles 
		{
			get { return this.readOnly; }
			set { this.readOnly = value; }
		}

		public object PasswordDocument 
		{
			get { return this.passwordDocument; }
			set { this.passwordDocument = value; }
		}

		public object PasswordTemplate 
		{
			get { return this.passwordTemplate; }
			set { this.passwordTemplate = value; }
		}


		public object Revert 
		{
			get { return this.revert; }
			set { this.revert = value; }
		}

		public object WritePasswordDocument 
		{
			get { return this.writePasswordDocument; }
			set { this.writePasswordDocument = value; }
		}

		public object WritePasswordTemplate 
		{
			get { return this.writePasswordTemplate; }
			set { this.writePasswordTemplate = value; }
		}

		public object Format 
		{
			get { return this.format; }
			set { this.format = value; }
		}


		public object Encoding {
			get { return this.encoding; }
			set { this.encoding = value; }
		}

		public object Visible 
		{
			get { return this.visible; }
			set { this.visible = value; }
		}

		public object OpenAndRepair
		{
			get { return this.openAndRepair; }
			set { this.openAndRepair = value; }
		}
		
		public object DocumentDirection 
		{
			get { return this.documentDirection; }
			set { this.documentDirection = value; }
		}
		
		public object NoEncodingDialog 
		{
			get { return this.noEncodingDialog; }
			set { this.noEncodingDialog = value; }
		}
		
		public object XmlTransformation 
		{
			get { return this.xmlTransformation; }
			set { this.xmlTransformation = value; }
		}
	}
}
