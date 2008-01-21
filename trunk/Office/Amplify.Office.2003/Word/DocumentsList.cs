//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Office.v2003.Word
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Text;

	using Office;
	using WI = Microsoft.Office.Interop.Word;

	using Amplify.Office.Word;

	public class DocumentsList : List<IDocument>, IDocumentsList
	{
		private WI.Application parent;

		public DocumentsList(WI.Application parent)
		{
			this.parent = parent;
		}


		public IDocument Open(string filename)
		{
			return this.Open(filename, true, false);
		}

		public IDocument Open(string filename, bool isReadonly, bool isVisible)
		{
			OpenArgs args = new OpenArgs();
				args.FileName = filename;
				args.ReadOnly = isReadonly;
				args.Visible = isVisible;

			return this.Open(args);
		}

		public IDocument Open(OpenArgs args)
		{
			object	filename = args.FileName,
					confirmConversions = args.ConfirmConversions,
					readOnly = args.ReadOnly,
					addToRecentFiles = args.AddToRecentFiles,
					passwordDocument = args.PasswordDocument,
					passwordTemplate = args.PasswordTemplate,
					revert = args.Revert,
					writePasswordDocument = args.WritePasswordDocument,
					writePasswordTemplate = args.WritePasswordTemplate,
					format = args.Format,
					encoding = args.Encoding,
					visible = args.Visible,
					openAndRepair = args.OpenAndRepair,
					documentDirection = args.DocumentDirection,
					noEncodingDialog = args.NoEncodingDialog,
					xmlTransformation = args.XmlTransformation;


			WI.Document document = this.parent.Documents.Open(
					ref filename,
					ref confirmConversions,
					ref readOnly,
					ref addToRecentFiles,
					ref passwordDocument,
					ref passwordTemplate,
					ref revert,
					ref writePasswordDocument,
					ref writePasswordTemplate,
					ref format,
					ref encoding,
					ref visible,
					ref openAndRepair,
					ref documentDirection,
					ref noEncodingDialog,
					ref xmlTransformation);

			Document doc = new Document(document);
			doc.SetParent(this);

			this.Add(doc);
			return doc;
		}

		public IDocument this[string name]
		{
			get {
				foreach (IDocument item in this)
					if (item.Name.ToLower().Equals(name.ToLower()))
						return item;
				return null;
			}
		}

		#region IDisposable Members

		public void Dispose()
		{
			foreach (IDocument document in this)
				document.Dispose();
		}

		#endregion
	}
}
