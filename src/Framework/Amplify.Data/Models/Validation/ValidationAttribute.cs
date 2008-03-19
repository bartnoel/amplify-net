using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Models.Validation
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Constructor | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
	public class ValidationAttribute : System.Attribute 
	{
		public virtual string Message { get; set; }

		public string Name
		{
			get { return this.Rule.Name; }
			set { ((IDecoratedObject)this.Rule)["Name"] = value; }
		}

		public virtual string If { get; set; }

		public object Level { get; set; }

		public virtual ComponentModel.IValidationRule Rule { get { return null; } }

		public ValidationAttribute()
		{

		}
	}
}
