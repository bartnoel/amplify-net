//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace DefyTheGrind.Web.UI.WebControls
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Web.UI;
	using System.Web.UI.Design.WebControls;
	using System.Web.UI.WebControls;
	

	using Amplify;
	using Amplify.Linq;
	using Amplify.Data.Validation;

	public class FormViewItem : WebControl, ITextControl 
	{
		private TextBox textbox;
		private string datafield = "";
		private bool isDataBound = false;
		private Control container;

		
		public Control FormControl
		{
			get { return this.textbox; }
		}





		public virtual object Value
		{
			get { return (object)this.textbox.Text; }
		}

		public string TypeName
		{
			get
			{
				object value = base.ViewState["TypeName"];
				if (value == null)
					return "";
				return value.ToString();
			}

			set
			{
				base.ViewState["TypeName"] = value;
			}
		}

		public string DataField
		{
			get
			{
				if (this.datafield == null)
				{
					object value = base.ViewState["DataField"];
					if (value == null)
						this.datafield = string.Empty;
					else
						this.datafield = value.ToString();
				}
				return this.datafield;
			}
			set
			{
				if(!Object.Equals(value, base.ViewState["DataField"])) {
					base.ViewState["DataField"] = value;
					this.datafield = value;
					this.OnFieldChanged();
				}
			}
		}


		protected virtual void OnFieldChanged() { }

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.Page.RegisterRequiresControlState(this);
		}

		protected override void LoadControlState(object savedState)
		{
			if (savedState != null)
			{
				Pair p = savedState as Pair;
				if (p != null)
				{
					base.LoadControlState(p.First);
					this.isDataBound = (bool)p.Second;
				}
				else
				{
					if (savedState is bool)
						this.isDataBound = (bool)savedState;
					else
						base.LoadControlState(savedState);
				}
			}
			base.LoadControlState(savedState);
		}

		protected override object SaveControlState()
		{
			object obj1 = base.SaveControlState();
			if (obj1 != null)
				return new Pair(obj1, this.isDataBound);
			else
				return this.isDataBound;
		}

		protected internal virtual void Bind(Control container)
		{
			this.container = container;
		
			this.CreateControls();
		}

		protected virtual void CreateControls()
		{
			this.textbox = new TextBox() {
				ID = this.TypeName + "_" + this.DataField,
				CssClass = "text",
			};
			
			this.CreateLabel(this.textbox.ClientID);
			this.Controls.Add(this.textbox);
		}

		protected virtual void CreateLabel(string clientId)
		{
			this.Controls.Add(
				new LiteralControl(
					string.Format("<label for=\"{0}\">{1}:</label>", clientId, Inflector.Titleize(this.DataField))));
		}

		protected override void DataBind(bool raiseOnDataBinding)
		{
			if (raiseOnDataBinding)
				OnDataBinding(EventArgs.Empty);

			this.isDataBound = true;

			Object obj1 = DataBinder.GetDataItem(this.container);
			this.SetValue(obj1);
		}

		protected virtual void SetValue(object value)
		{
			if (value != null)
			{
				if (value is IDecoratedObject)
					this.textbox.Text = (((IDecoratedObject)value)[this.DataField] as string);
				else
					this.textbox.Text = (value.GetType().GetProperty(this.DataField).GetValue(value, null) as string);
			}
		}


		protected override void OnPreRender(EventArgs e)
		{

			List<IValidator> list = null;
			object value = null;
			if (this.isDataBound)
			{
				Object obj1 = DataBinder.GetDataItem(this.container);

				if(obj1 is IWebFormValidation)
				{
					base.ViewState["Object"] = obj1;
					value = obj1;
				}
			}
			else
			{
				value = base.ViewState["Object"];
			}


			if (value != null)
			{
				list = ((IWebFormValidation)value).GetValidators(this.DataField);
			}

			if(list != null)
				list.Each(item => this.Controls.Add(((Control)item)));

			base.OnPreRender(e);
		}

		protected override void Render(HtmlTextWriter writer)
		{
			this.RenderChildren(writer);
		}


		#region ITextControl Members

		public string Text
		{
			get
			{
				return this.textbox.Text;
			}
			set
			{
				this.textbox.Text = value;
			}
		}

		#endregion
	}
}
