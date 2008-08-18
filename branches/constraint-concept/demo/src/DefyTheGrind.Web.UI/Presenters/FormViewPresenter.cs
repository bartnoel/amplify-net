

namespace DefyTheGrind.Web.UI.Presenters
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;


	using Amplify.ActiveRecord;

	public class FormViewPresenter<V, T> : DataBoundPresenter<V, T>
		where V : Views.IFormViewOnUserControlView
		where T : Base<T>
	{



		public FormViewPresenter(V view) : base(view) 
		{
			
		}

		protected override void UpdateView(object sender, EventArgs e)
		{
			base.UpdateView(sender, e);
			if (this.DataBindingRequired)
			{
				if (this.Selection == null)
					this.Selection = Activator.CreateInstance<T>();
				this.View.FormView.DataBind();

			}
		}
	}
}
