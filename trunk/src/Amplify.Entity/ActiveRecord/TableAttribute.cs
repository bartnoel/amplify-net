using System;
using System.Collections.Generic;

using System.Text;

namespace Amplify.ActiveRecord
{

	
	using Amplify.Data;
	

	public abstract class TableAttribute : System.Attribute 
	{
		private string alias = "";

		private List<ColumnAttribute> columns;

		public string TableName { get; set; }

		public string Selection { get; set; }

		public string ConnectionStringPrefix { get; set; }

		public List<ColumnAttribute> Columns
		{
			get {
				if (this.columns == null)
					this.columns = new List<ColumnAttribute>();
				return this.columns;
			}
		}

		public virtual bool IsPrimary { get; set; }

		public string As
		{
			get
			{
				if (string.IsNullOrEmpty(this.alias))
					return Inflector.Uncapitalize(this.TableName);
				return this.alias;
			}
			set { this.alias = value; }
		}

		public string GetSelectionFragment()
		{
			string[] sections = this.Selection.Split(",".ToCharArray());
			string selection = "";
			foreach(string section in sections)
				selection = string.Format("{0}.{1}, ", this.As, section);
			return selection.TrimEnd(", ".ToCharArray()) + " ";
		}

		public string GetTableFragment()
		{
			return this.GetTableFragment(null);
		}

		public string GetTableFragment(Adapter adapter)
		{
			string tableName = this.TableName;
			if(adapter != null) 
				tableName = adapter.QuoteTableName(this.TableName);
			return string.Format("{0} as {1}", tableName, this.As);
		}
	}
}
