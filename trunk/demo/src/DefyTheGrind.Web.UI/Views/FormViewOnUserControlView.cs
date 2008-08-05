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

	using WebControls;
	using Amplify.ActiveRecord;

	public class FormViewOnUserControlView<T> : DataBoundView, IFormViewOnUserControlView 
		where T : Base<T> 
	{
		#region IFormViewOnUserControlView Members

		protected virtual Presenters.FormViewPresenter<FormViewOnUserControlView<T>, T> Presenter { get; set; }

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			Control control = this.FindControl("FormView");
			this.FormView = control as FormView;
			this.Presenter = new Presenters.FormViewPresenter<FormViewOnUserControlView<T>, T>(this);
		}

		public virtual DefyTheGrind.Web.UI.WebControls.IFormView FormView { get; set; }

	

		#endregion
	}
}
