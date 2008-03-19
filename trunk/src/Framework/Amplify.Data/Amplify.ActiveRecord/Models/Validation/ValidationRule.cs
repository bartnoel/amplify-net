using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Models.Validation
{
	public class ValidationRule : DecoratedObject, ComponentModel.IValidationRule 
	{
		public string Name 
		{
			get { return (this["Name"] as string); }
			internal set { this["Name"] = value; }
		}

		public virtual string Message 
		{
			get { return (this["Message"] as string); }
			set { this["Message"] = value; }
		}

		public object Level 
		{
			get { return this["Level"]; }
			set { this["Level"] = value; }
		}

		public string RuleName
		{
			get { return this.GetType().Name; }
		}

		public string If 
		{
			get { return (this["If"] as string); }
			set { this["If"] = value; }
		}

		public ValidationRule()
		{
			this.Level = 0;
		}

		public virtual bool Validate(object value)
		{
			return true;
		}

	}
}
