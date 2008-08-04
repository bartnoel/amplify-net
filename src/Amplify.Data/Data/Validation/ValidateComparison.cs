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

	public class ValidateComparison : ValidationRule 
	{
		public ValidateComparison()
		{
			this.Operator = ValidationCompareOperator.Equal;
			this.PropertyToCompare = "";
			this.Message = "{0} must be {1} {2}.";
					
		}

		public string PropertyToCompare
		{
			get { return this["PropertyToCompare"] as string; }
			set { this["PropertyToCompare"] = value; }
		}

		public ValidationCompareOperator Operator
		{
			get { return (ValidationCompareOperator)this["Operator"]; }
			set { this["Operator"] = value; }
		}

		public IComparable ValueToCompare
		{
			get { return (IComparable)this["ValueToCompare"]; }
			set { this["ValueToCompre"] = value; }
		}

		public override string Message
		{
			get
			{
				if (this.Operator == ValidationCompareOperator.DataTypeCheck)
					return "";

				return string.Format(base.Message,
					Inflector.Titleize(this.PropertyName),
					Inflector.Titleize(this.Operator.ToString().Replace("Equal", "Equal To").Replace("Than Equal", "Than Or Equal")).ToLower(),
					Inflector.Titleize(this.PropertyToCompare));
			}
			set
			{
				base.Message = value;
			}
		}

		protected internal override System.Web.UI.IValidator GetValidator()
		{
			string name = this.GetControlPropertyName();
			CompareValidator validator = new CompareValidator()
			{
				ControlToValidate = name,
				ID = name + "-Compare",
				ForeColor = Color.Transparent,
				CssClass = "error",
				Operator = this.Operator,
				ValidationGroup = this.EntityType
			};
			if (!string.IsNullOrEmpty(this.PropertyToCompare))
				validator.ControlToCompare = this.EntityType + "." + this.PropertyToCompare;
			else
				validator.ValueToCompare = this.ValueToCompare.ToString();
			return validator;
		}

		public override bool ValidateData(object entity, object value)
		{
			if (this.Operator == ValidationCompareOperator.DataTypeCheck)
				return true;

			object data = value;
			object control = null;
			if (value is IDecoratedObject)
			{
				control = ((IDecoratedObject)value)[this.PropertyToCompare];
			}
			else
			{
				Type type = value.GetType();
				control = type.GetProperty(this.PropertyToCompare).GetValue(value, null);
			}

			int compare = ((IComparable)control).CompareTo((IComparable)data);

			switch (this.Operator)
			{
				case ValidationCompareOperator.Equal:
					return (compare == 0);
				case ValidationCompareOperator.GreaterThan:
					return (compare > 0);
				case ValidationCompareOperator.GreaterThanEqual:
					return (compare == 0 || compare > 0);
				case ValidationCompareOperator.LessThan:
					return (compare < 0);
				case ValidationCompareOperator.LessThanEqual:
					return (compare == 0 || compare < 0);
				case ValidationCompareOperator.NotEqual:
					return (compare != 0);
			}
			return false;
		}
	}
}
