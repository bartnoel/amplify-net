//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.ActiveRecord
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using Amplify.Linq;

	public abstract partial class Base<T>
	{


		#region New

		public static T New()
		{
			T item = Activator.CreateInstance<T>();
			item.IsNew = true;
			return item;
		}

		public static T New(IDictionary<string, object> values)
		{
			T item = New();
			item.Send(values);
			return item;
		}

#if LINQ
		
		public static T New(params Func<object, object>[] values)
		{
			return New(Hash.New(values));
		}
#endif
		#endregion



	}
}
