//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Data.Validation
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.Text;
	using System.Web.UI.WebControls;

	using Amplify.ComponentModel;

	public class ValidatePresence : ValidationRule
	{

		public ValidatePresence()
		{
			this.Message = "{0} is required.";
			this.DefaultValue = "";
		}

		public object DefaultValue
		{
			get { return this["DefaultValue"]; }
			set { this["DefaultValue"] = value; }
		}

		public override string Message
		{
			get
			{
				return string.Format(base.Message,
					Inflector.Titleize(this.PropertyName));
			}
			set
			{
				base.Message = value;
			}
		}

		protected internal override System.Web.UI.IValidator GetValidator()
		{
			string name = this.GetControlPropertyName();
			return new RequiredFieldValidator()
			{
				ControlToValidate = this.GetControlPropertyName(),
				CssClass = "error",
				ForeColor = Color.Transparent,
				ID = name + "-Required",
				ErrorMessage = this.Message,
				InitialValue = this.DefaultValue.ToString(),
				ValidationGroup = EntityType
			};
		}

		public override bool ValidateData(object entity, object value)
		{
			object data = value;

			if (this.If == null || this.If(data))
			{
				if (data != null)
				{
					if (data is DateTime)
						return !value.Equals(DateTime.MinValue);
					if (data is int)
						return !value.Equals(-1);
				}
				if (data is string || this.DefaultValue is string)
					return !string.IsNullOrEmpty(data as string);
				return (!Object.Equals(data, this.DefaultValue));
			}
			return true;
		}

		
	}
}
