using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.ActiveRecord
{
	public enum AssociationType
	{
		BelongsTo,
		HasOne,
		HasMany,
		BelongsToAndHasOne,
		BelongsToAndHasMany
	}

	public class AssociationAttribute : System.Attribute 
	{
		public string Type { get; set; }

		public AssociationAttribute(string type)
		{
			this.Type = type;
		}

		public AssociationAttribute(AssociationType type)
		{
			this.Type = type.ToString();
		}

	}
}
