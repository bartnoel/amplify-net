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
	using Amplify.Linq;

	[Serializable]
	public class ValidateRange : ValidationRule
	{
		private Broken broken;

		

		public ValidateRange()
		{
			this.Is = int.MinValue;
			this.AllowNull = false;
			this.TooShort = "{0} is too minimal (min is {1}). ";
			this.TooLong = "{0} is too great (max is {1}). ";
			this.WrongLength = "{0} is the wrong lenght (should be {1} characters). ";
		}

		private enum Broken
		{
			None,
			Max,
			Min,
			Is
		}

		public IComparable Maximum
		{
			get { return this.In.Maximum; }
			set { this.In.Maximum = value; }
		}

		public IComparable Minimum
		{
			get { return this.In.Minimum; }
			set { this.In.Minimum = value; }
		}

		public Range In
		{
			get
			{
				object value = (this["Range"] as Range);
				if (value == null)
					this["Range"] = new Range();
				return (Range)this["Range"];
			}
			set { this["Range"] = value; }
		}


		public IComparable Is
		{
			get { return (int)this["Is"]; }
			set { this["Is"] = value; }
		}

		public bool AllowNull
		{
			get { return (bool)this["AllowNull"]; }
			set { this["AllowNull"] = value; }
		}

		public string TooLong
		{
			get { return this["TooLong"].ToString(); }
			set { this["TooLong"] = value; }
		}

		public string TooShort
		{
			get { return this["TooShort"].ToString(); }
			set { this["TooShort"] = value; }
		}

		public string WrongLength
		{
			get { return this["WrongLength"].ToString(); }
			set { this["WrongLength"] = value; }
		}

		public override string Message
		{
			get
			{
				string message = "";
				if (this.broken == Broken.Min)
					message += string.Format(this.TooShort, Inflector.Titleize(this.PropertyName), this.Minimum);
				if (this.broken == Broken.Max)
					message += string.Format(this.TooLong, Inflector.Titleize(this.PropertyName), this.Maximum);
				if (this.broken == Broken.Is)
					message = string.Format(this.WrongLength, Inflector.Titleize(this.PropertyName), this.Is);
				if (this.broken == Broken.None)
					return "";
				return message;
			}
			set
			{
				this.WrongLength = value;
			}
		}

		protected internal override System.Web.UI.IValidator GetValidator()
		{
			if (this.Is is int && (int)this.Is == int.MinValue)
			{
				string name = this.GetControlPropertyName();
				string title = Inflector.Titleize(this.PropertyName);
				return new RangeValidator()
				{
					Display = ValidatorDisplay.Dynamic,
					ControlToValidate = name,
					CssClass = "error",
					ForeColor = Color.Empty,
					ID = name + "_Range",
					MinimumValue = this.Minimum.ToString(),
					MaximumValue = this.Maximum.ToString(),
					ValidationGroup = this.EntityType,
					ErrorMessage = string.Format(this.TooShort, title, this.Minimum) +
						string.Format(this.TooLong, title, this.Maximum)

				};
			}
			return null;
		}

		public override bool ValidateData(object entity, object value)
		{
			object data = value;
			this.broken = Broken.None;

			if (data is string)
				data = data.ToString().Length;

			if (this.AllowNull && data == null)
				return true;
			
			else if (data == null)
			{
				this.broken = Broken.Min;
				return false;
			}

			if (!(this.Is is int) || (int)this.Is != int.MinValue)
			{
				int compare = ((IComparable)data).CompareTo(this.Is);

				if (compare != 0)
				{
					this.broken = Broken.Is;
					return false;
				}
			}
			bool returnValue = true;

			if (!(this.In.Minimum is int) || (int)this.In.Minimum != int.MinValue)
			{
				int compare = ((IComparable)data).CompareTo(this.In.Minimum);

				if (compare < 0)
				{
					this.broken = Broken.Min;
					returnValue = false;
				}
			}
			if (!(this.In.Maximum is int) || (int)this.In.Maximum != int.MaxValue)
			{
				int compare = ((IComparable)data).CompareTo(this.In.Maximum);

				if (compare > 0)
				{
					if(this.broken == Broken.Min)
						this.broken = (this.broken | Broken.Max);
					this.broken = Broken.Max;
					returnValue = false;
				}
			}

			return returnValue;
		}
	}
}
