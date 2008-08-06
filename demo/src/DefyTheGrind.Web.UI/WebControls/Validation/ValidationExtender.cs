using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace DefyTheGrind.Web.UI.WebControls.Validation
{
	public class ValidationExtender : System.ComponentModel.Component, IStateManager 
	{
		private StateBag viewState;
		private bool isTrackingViewState = false;
		private bool hasInternalViewState = false;

		public ValidationExtender() : this(null) { }

		public ValidationExtender(StateBag state)
		{
			this.viewState = state;
		}

		protected StateBag ViewState
		{
			get
			{
				if (this.viewState == null)
				{
					this.viewState = new StateBag(false);
					if (this.IsTrackingViewState)
						(this.viewState as IStateManager).TrackViewState();
				}
				return this.viewState;
			}
		}

		public string ControlToValidate
		{
			get {
				object value = this.ViewState["ControlToValidate"];
				if (value == null)
					return "";
				return value.ToString();
			}
			set { this.ViewState["ControlToValidate"] = value; }
		}

		public string ErrorMessage
		{
			get
			{
				object value = this.ViewState["ErrorMessage"];
				if (value == null)
					return "";
				return value.ToString();
			}
			set { this.ViewState["ErrorMessage"] = value; }
		}



		#region IStateManager Members

		public bool IsTrackingViewState
		{
			get { return this.isTrackingViewState; }
		}

		void IStateManager.LoadViewState(object state)
		{
			if (this.viewState != null)
				((IStateManager)this.viewState).LoadViewState(state);
		}

		object IStateManager.SaveViewState()
		{
			if (this.viewState != null)
			{
				if (this.viewState.Count > 0)
					return ((IStateManager)this.viewState).SaveViewState();
			}
			return null;
		}

		void IStateManager.TrackViewState()
		{
			if (this.hasInternalViewState)
				(this.viewState as IStateManager).TrackViewState();
			this.isTrackingViewState = true;
		}

		#endregion
	}
}
