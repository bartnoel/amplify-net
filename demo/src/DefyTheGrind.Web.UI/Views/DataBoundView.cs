//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace DefyTheGrind.Web.UI.Views
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Web.UI;


	public class DataBoundView : UserControlView, IDataBoundView 
	{
		#region IDataBoundView Members

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			Control control =	this.FindControl("DataSource");
			if (control == null)
			{
				this.DataSource = new Csla.Web.CslaDataSource()
				{
					ID = "DataSource"
				};
				this.Controls.Add(this.DataSource);
			}
			else
			{
				this.DataSource = control as Csla.Web.CslaDataSource;
			}
		}

		public virtual Csla.Web.CslaDataSource DataSource
		{
			get;
			set;
		}

		#endregion

		#region IDataBoundView Members


		public event EventHandler UpdateView;

		#endregion
	}
}
