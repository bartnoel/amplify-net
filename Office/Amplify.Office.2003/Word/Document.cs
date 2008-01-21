namespace Amplify.Office.v2003.Word
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Reflection;

	using WI = Microsoft.Office.Interop.Word;

	using Amplify.Office.Word;

	public class Document : IDocument
	{
		private WI.Document document;
		private IList<IDocument> parent;

		public Document(WI.Document document)
		{
			this.document = document;
		}

		public string Name
		{
			get { return this.document.Name; }
		}

		public bool IsReadOnly
		{
			get { return this.document.ReadOnly; }
		}

		internal void SetParent(IList<IDocument> parent)
		{
			
			this.parent = parent;
		}
		

		public void Close()
		{
			this.Close(new CloseArgs());		
		}

		public void Close(CloseArgs args)
		{
			object	saveChanges = args.SaveChanges,
					orginalFormat = args.OriginalFormat,
					routeDocument = args.RouteDocument;

			((WI.Document)this.document).Close(
				ref saveChanges,
				ref orginalFormat,
				ref routeDocument);

			if (this.parent != null)
				this.parent.Remove(this);
		}

		public bool HasCustomProperties(IEnumerable<string> propertyNames)
		{
			return this.HasProperties(this.document.CustomDocumentProperties, propertyNames);
		}

		public Dictionary<string, object> GetCustomProperties(IEnumerable<string> properyNames)
		{
			return this.GetProperties(this.document.CustomDocumentProperties, properyNames, true);
		}


		public void MergeFields(Dictionary<string, object> data)
		{
			
			int fieldCount = 0;
			foreach (WI.Field field in this.document.Fields)
			{
				fieldCount++;
				string text = field.Code.Text;

				if (text.StartsWith(" MERGEFIELD"))
				{
					int end = text.IndexOf("\\");
					int length = text.Length - end;
					int newLength = end - 11;
					newLength = (newLength < 0) ? 0 : newLength;
					string fieldName = text.Substring(11).Replace("\"", "").Trim();


					field.Select();
					if (data.ContainsKey(fieldName))
					{
						object value = data[fieldName];
						if (value.GetType() == typeof(DateTime))
							this.document.Application.Selection.TypeText(((DateTime)value).ToShortDateString());
						else
							this.document.Application.Selection.TypeText(data[fieldName].ToString());
					}
				}
			}
		}

		public bool HasProperties(object properties, IEnumerable<string> propertyNames)
		{
			try
			{
				Type type = properties.GetType();

				foreach (string propertyName in propertyNames)
				{
					object property = type.InvokeMember("Item", BindingFlags.Default |
								  BindingFlags.GetProperty,
								  null, properties,
								  new object[] { propertyName });
					if (property == null)
						return false;
				}
			}
			catch 
			{
				return false;
			}
			return true;
		}

		public Dictionary<string, object> GetProperties(object properties, IEnumerable<string> propertyNames, bool supressException)
		{
			Type type = properties.GetType();
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			foreach (string propertyName in propertyNames)
			{
				try
				{
					object property = type.InvokeMember("Item", BindingFlags.Default |
								  BindingFlags.GetProperty,
								  null, properties,
								  new object[] { propertyName });

					object value = property.GetType().InvokeMember("Value",
								  BindingFlags.Default |
								  BindingFlags.GetProperty,
								  null, property,
								  new object[] { });
					dictionary.Add(propertyName, value);
				}
				catch
				{
					if (!supressException)
						throw;
				}
			}
			return dictionary;
		}

		public void SaveAs(SaveAsArgs args)
		{
			object filename = args.FileName,
					fileFormat = args.FileFormat,
					lockComments = args.LockComments,
					passWI = args.Password,
					addToRecentFiles = args.AddToRecentFiles,
					writePassWI = args.WritePassword,
					readOnlyRecommended = args.ReadOnlyRecommended,
					embedTrueTypeFonts = args.EmbedTrueTypeFonts,
					saveNativePictureFormat = args.SaveNativePictureFormat,
					saveFormsData = args.SaveFormsData,
					saveAsAOCELetter = args.SaveAsAOCELetter,
					encoding = args.Encoding,
					insertLineBreaks = args.InsertLineBreaks,
					allowSubstitutions = args.AllowSubstitutions,
					lineEnding = args.LineEnding,
					addBiDiMarks = args.AddBiDiMarks;

			this.document.SaveAs(
				ref filename,
				ref fileFormat,
				ref lockComments,
				ref passWI,
				ref addToRecentFiles,
				ref writePassWI,
				ref readOnlyRecommended,
				ref embedTrueTypeFonts, 
				ref saveNativePictureFormat,
				ref saveFormsData,
				ref saveAsAOCELetter,
				ref encoding,
				ref insertLineBreaks,
				ref allowSubstitutions,
				ref lineEnding, 
				ref addBiDiMarks);
		}

		public void Save()
		{
			this.document.Save();
		}

		#region IDisposable Members

		public void Dispose()
		{
			this.Close();
		}

		#endregion
	}
}
