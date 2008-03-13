//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Text;


	public class DecoratedObject : DecoratedInternalObject 
	{
		public object this[string propertyName]
		{
			get { return this.GetProperty(propertyName); }
			set { this.SetProperty(propertyName, value); }
		}

		protected virtual object GetProperty(string propertyName)
		{
			return this.Get(propertyName);
		}

		protected virtual void SetProperty(string propertyName, object value)
		{
			this.Set(propertyName, value);
		}

		public void EachKey(Action<string> action)
		{
			foreach (string key in this.Values.Keys)
				action(key);
		}

		public void EachValue(Action<object> action)
		{
			foreach (string key in this.Values.Keys)
				action(this.Values[key]);
		}

		public void Each(Action<KeyValuePair<string, object>> action)
		{
			foreach (string key in this.Values.Keys)
				action(new KeyValuePair<string, object>(key, this[key]));
		}

		protected void MergeInternal(DecoratedObject obj)
		{
			obj.Each(delegate(KeyValuePair<string, object> item) {
				this[item.Key] = item.Value;
			});
		}
	}
}
