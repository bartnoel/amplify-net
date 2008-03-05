using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data
{
	public enum AssocationType
	{
		BelongsTo,
		HasOne,
		HasMany,
		BelongsToAndHasOne,
		BelongsToAndHasMany
	}
}
