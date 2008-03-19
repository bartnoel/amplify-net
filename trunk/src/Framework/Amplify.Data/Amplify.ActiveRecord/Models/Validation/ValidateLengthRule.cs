using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Amplify.ComponentModel;
using Amplify.Linq;

namespace Amplify.Models.Validation
{

	public class ValidateLengthRule : ValidationRule,IClone
	{
		private Broken broken;

		private enum Broken
		{
			None,
			Max,
			Min,
			Is
		}

		public int Maximum 
		{
			get { return this.In.Maximum; }
			set { this.In.Maximum = value; }
		}

		public int Minimum 
		{
			get { return this.In.Minimum; }
			set { this.In.Minimum = value; }
		}

		public Range In 
		{
			get { 
				object value = (this["Range"] as Range);
				if(value == null)
					this["Range"] = new Range();
				return (Range)this["Range"];
			}
			set { this["Range"] = value; }
		}

	
		public int Is 
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
			get {
				switch (this.broken)
				{
					case Broken.Min:
						return this.TooShort.Inject(this.Minimum);
					case Broken.Max:
						return this.TooLong.Inject(this.Maximum);
					case Broken.Is:
						return this.WrongLength.Inject(this.Is);
					case Broken.None:
					default:
						return "";
				}
			}
			set {
				this.WrongLength = value;
			}
		}

		public ValidateLengthRule()
		{
			this.Is = int.MinValue;
			this.AllowNull = false;
			this.TooShort = "is too short (min is {0} characters)";
			this.TooLong = "is too long (max is {0} characters)";
			this.WrongLength = "is the wrong length (should be {0} characters)";
		}

		public override bool Validate(object value)
		{
			if (this.AllowNull && value == null)
				return true;
			else if (value == null)
			{
				this.broken = Broken.Min;
				return false;
			}

			if (this.Is != int.MinValue)
			{
				this.broken = Broken.Is;
				return (value.ToString().Length == this.Is);
			}
			if (this.In.Minimum != int.MinValue)
			{
				if (value.ToString().Length <= this.In.Minimum)
				{
					this.broken = Broken.Min;
					return false;
				}
			}
			if (this.In.Maximum != int.MaxValue)
			{
				if (value.ToString().Length <= this.In.Maximum)
				{
					this.broken = Broken.Max;
					return false;
				}
			}
			this.broken = Broken.None;
			return true;
		}

		#region ICloneable Members

		public IValidationRule Clone()
		{
			ValidateLengthRule rule = new ValidateLengthRule();
			this.EachKey(key => rule[key] = this[key]);
			return rule;
		}

		#endregion
	}
}
