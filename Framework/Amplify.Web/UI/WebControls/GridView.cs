using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

[assembly:TagPrefix("Amplify.Web.UI.WebControls", "amp")]

namespace Amplify.Web.UI.WebControls
{
	public class GridView : System.Web.UI.WebControls.GridView
	{
		[Description("")]
		public bool ShowAddRow
		{
			get {
				object value = this.ViewState["ShowAddRow"];
				if (value == null)
					return false;
				return (bool)value;
			}
			set {
				this.ViewState["ShowAddRow"] = value;
			}
		}

	


		protected override void OnRowCreated(GridViewRowEventArgs e)
		{
			if (this.ShowAddRow)
			{
				if (e.Row.RowType == DataControlRowType.Header)
				{
					
				}
			}
			base.OnRowCreated(e);
		}

		protected override void DataBind(bool raiseOnDataBinding)
		{
			base.DataBind(raiseOnDataBinding);
		}

		protected override System.Web.UI.WebControls.GridViewRow CreateRow(int rowIndex, int dataSourceIndex, System.Web.UI.WebControls.DataControlRowType rowType, System.Web.UI.WebControls.DataControlRowState rowState)
		{
			return base.CreateRow(rowIndex, dataSourceIndex, rowType, rowState);
		}

		protected override void PrepareControlHierarchy()
		{
			Table table = this.Controls[0] as Table;
			GridViewRow row = base.CreateRow(0, -99, DataControlRowType.EmptyDataRow, DataControlRowState.Alternate | System.Web.UI.WebControls.DataControlRowState.Edit);
			row.Visible = true;
			table.Controls.AddAt(1, row);
			base.PrepareControlHierarchy();
		}

		protected override void InitializeRow(GridViewRow row, DataControlField[] fields)
		{
			if (this.ShowAddRow)
			{
				bool empty = ((row.RowType == DataControlRowType.EmptyDataRow));
				if (empty && row.DataItemIndex == -99)
				{
					foreach (DataControlField field in fields)
						if (field is CommandField)
						{
							CommandField command = (CommandField)field;
							command.ShowDeleteButton = false;
							command.ShowEditButton = false;
							command.ShowDeleteButton = false;
							command.ShowInsertButton = true;
							command.ShowCancelButton = true;
						}
				}
			}

			base.InitializeRow(row, fields);	
		}

		protected override void OnRowUpdating(GridViewUpdateEventArgs e)
		{
			if (this.ShowAddRow)
			{
				IDataSource ds = this.GetDataSource();
				if (ds.GetViewNames().Count > 0)
				{
					string viewname = "";
					foreach (string name in ds.GetViewNames())
					{
						viewname = name;
						break;
					}

					DataSourceView view = ds.GetView(viewname);
					if (view != null && view.CanInsert)
						view.Insert(e.NewValues, null);
					else
					{
						GridViewUpdateEventArgs args = new GridViewUpdateEventArgs(e.RowIndex+1);

						foreach (KeyValuePair<string, object> item in e.OldValues)
							args.OldValues.Add(item.Key, item.Value);

						foreach (KeyValuePair<string, object> item in e.NewValues)
							args.NewValues.Add(item.Key, item.Value);

						foreach (KeyValuePair<string, object> item in e.Keys)
							args.Keys.Add(item.Key, item.Value);

						base.OnRowUpdating(args);
					}
				}
				
			}
			
		}
	}
}
