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
	using System.Reflection;

	public class DecoratedObject : IDecoratedObject, IDisposable 
	{
		private Hashtable properties;

		protected Hashtable Properties
		{
			get {
				if (this.properties == null)
					this.properties = new Hashtable();
				return this.properties; 
			}
		}

		public object this[string propertyName]
		{
			get { return this.GetProperty(propertyName); }
			set { this.SetProperty(propertyName, value); }
		}

		protected virtual object GetProperty(string propertyName)
		{
			if (this.Properties.ContainsKey(propertyName))
				return this.Properties[propertyName];
			return null;
		}

		protected virtual void SetProperty(string propertyName, object value)
		{
			this.Properties[propertyName] = value;
		}

		public void EachProperty(Action<KeyValuePair<string, object>> action)
		{
			foreach (KeyValuePair<string, object> item in this.Properties)
				action(item);
		}

		protected void Map(IDecoratedObject properties)
		{
			foreach (string propertyName in this.Properties.Keys)
			{
				object value = properties[propertyName];
				if (value != null)
					this.Properties[propertyName] = value;
			}
		}

		protected void Map(IDictionary<string, object> properties)
		{
			foreach (string propertyName in properties.Keys)
				if (this.Properties.Contains(propertyName))
					this.Properties[propertyName] = properties[propertyName];
		}

		protected void Map(object properties)
		{
			Type type = properties.GetType();
			foreach (PropertyInfo property in type.GetProperties())
			{
				if (this.Properties.Contains(property.Name))
					this.Properties[property.Name] = property.GetValue(properties, null);
			}
		}

		#region IDisposable Members

		public virtual void Dispose()
		{
			if (this.properties != null)
			{
				foreach (object value in this.Properties.Values)
					if (value is IDisposable)
						((IDisposable)value).Dispose();

				this.properties.Clear();
				this.properties = null;
			}
		}

		#endregion
	}
}
