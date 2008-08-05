//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace DefyTheGrind.Web.UI.WebControls
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Web.UI;
	using System.Web.UI.WebControls;

	using Amplify.Linq;

	public class FormView : System.Web.UI.WebControls.FormView, IFormView 
	{
		private object temp;

		public FormView():base()
		{
			
		}

		protected override System.Web.UI.Adapters.ControlAdapter ResolveAdapter()
		{
			if (!this.DesignMode)
				this.ResolveCssAdapter(typeof(Css.FormViewAdapter));

			return base.ResolveAdapter();
		}

		private List<FormViewItem> items = new List<FormViewItem>();

		protected override int CreateChildControls(IEnumerable datasource, bool databinding)
		{
			int value = base.CreateChildControls(datasource, databinding);
			this.temp = datasource;
			return value;
		}

		protected override void InitializeRow(FormViewRow row)
		{
			base.InitializeRow(row);

			if (row != null)
			{
				

				foreach (TableCell cell in row.Cells)
				{
					foreach (Control control in cell.Controls)
					{
						if (control is FormViewItem)
						{
							FormViewItem item = (FormViewItem)control;
							this.items.Add(item);
							item.Bind(this);
						}
					}
				}
			}
		}

		protected override void ExtractRowValues(System.Collections.Specialized.IOrderedDictionary fieldValues, bool includeKeys)
		{
			base.ExtractRowValues(fieldValues, includeKeys);
			items.Each(item => fieldValues[item.DataField] = item.Value);
		}
	}
}
