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
	using System.Text.RegularExpressions;
	using System.Web.UI.WebControls;

	using Amplify.ComponentModel;

	public class ValidateFormat : ValidationRule 
	{
		public string With 
		{
			get { return (this["With"] as string); }
			set { this["With"] = value; }
		}

		public RegexOptions Options 
		{
			get { return (RegexOptions)this["Options"]; }
			set { this["Options"] = value; }
		}

		public ValidateFormat()
		{
			this.Message = "{0} is invalid.";
			this.Options = RegexOptions.ECMAScript;
		}

		public override string Message
		{
			get
			{
				return
					string.Format(base.Message,
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
			return new RegularExpressionValidator()
			{
				ControlToValidate = name,
				ID = name + "-Regex",
				ForeColor = Color.Transparent,
				ValidationGroup = this.EntityType,
				ValidationExpression = this.With,
				ErrorMessage = this.Message
			};
		}

		public override bool ValidateData(object entity, object value)
		{
			object data = value;

			if (data != null && (this.If == null || this.If(data)))
				return StringUtil.IsMatch(data as string, this.With, this.Options);
			return true;
		}
	}
}
