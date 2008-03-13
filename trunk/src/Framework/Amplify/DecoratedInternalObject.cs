//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify
{
	using System;
	using System.Collections.Generic;
	using System.Collections;
	using System.Text;

	public class DecoratedInternalObject : IDisposable 
	{
		private readonly Hashtable attributes = new Hashtable();

		internal IDictionary Values
		{
			get { return (IDictionary)this.attributes; } 
		}

		internal object Get(string name)
		{
			return this.attributes[name];
		}

		internal void Set(string name, object value)
		{
			this.attributes[name] = value;
		}

		#region IDisposable Members

		void IDisposable.Dispose()
		{
			if (this.attributes != null)
			{
				foreach (object obj in this.attributes)
				{
					if (obj is IDisposable)
						((IDisposable)obj).Dispose();
				}
				this.attributes.Clear();
			}
		}

		#endregion
	}
}
