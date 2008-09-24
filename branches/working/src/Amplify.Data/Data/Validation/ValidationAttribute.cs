using System;
using System.Collections.Generic;

using System.Text;

namespace Amplify.Data.Validation
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Constructor | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
	public class ValidationAttribute : System.Attribute 
	{
		public virtual string Message { get; set; }

		public string PropertyName
		{
			get { return this.Rule.PropertyName; }
			set { ((IDecoratedObject)this.Rule)["PropertyName"] = value; }
		}

		public virtual Predicate<object> If { get; set; }

		public object Level { get; set; }

		public virtual IValidationRule Rule { get { return null; } }

		public ValidationAttribute()
		{

		}
	}
}
