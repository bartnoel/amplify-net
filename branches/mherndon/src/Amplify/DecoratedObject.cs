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

	[Serializable]
	public class DecoratedObject : IDecoratedObject, IDisposable 
	{
		private Hashtable properties;

		/// <summary>
		/// The stored properties. 
		/// </summary>
		protected Hashtable Properties
		{
			get {
				if (this.properties == null)
					this.properties = new Hashtable();
				return this.properties; 
			}
		}

		/// <summary>
		/// Gets or sets the specified property.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <returns>The value of the property.</returns>
		public object this[string propertyName]
		{
			get { return this.GetProperty(propertyName); }
			set { this.SetProperty(propertyName, value); }
		}

		/// <summary>
		/// Gets the value of the property.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <returns>The value of the property, if it doesn't exit, it returns null.</returns>
		protected virtual object GetProperty(string propertyName)
		{
			if (this.Properties.ContainsKey(propertyName))
				return this.Properties[propertyName];
			return null;
		}

		/// <summary>
		/// Sets the property value.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="value">The value of the property.</param>
		protected virtual void SetProperty(string propertyName, object value)
		{
			this.Properties[propertyName] = value;
		}

		/// <summary>
		/// Runs the action on each KeyPair value in the Decorated object;
		/// </summary>
		/// <param name="action"></param>
		public void EachProperty(Action<KeyValuePair<string, object>> action)
		{
			foreach (KeyValuePair<string, object> item in this.Properties)
				action(item);
		}

		/// <summary>
		/// Maps the object that impliments IDecorated to the current Decorated object.
		/// </summary>
		/// <param name="source"></param>
		protected void Map(IDecoratedObject source)
		{
			foreach (string propertyName in this.Properties.Keys)
			{
				object value = source[propertyName];
				if (value != null)
					this.Properties[propertyName] = value;
			}
		}

		/// <summary>
		/// Maps the IDictionary&lt;string, object&gt; values to the Decorated object.
		/// </summary>
		/// <param name="source">The source IDictionary object.</param>
		protected void Map(IDictionary<string, object> source)
		{
			foreach (string propertyName in source.Keys)
				if (this.Properties.Contains(propertyName))
					this.Properties[propertyName] = source[propertyName];
		}

		/// <summary>
		/// Maps the properties of the object to the Decorcated object.
		/// </summary>
		/// <param name="source">The source object.</param>
		protected void Map(object source)
		{
			Type type = source.GetType();
			foreach (PropertyInfo property in type.GetProperties())
			{
				if (this.Properties.Contains(property.Name))
					this.Properties[property.Name] = property.GetValue(source, null);
			}
		}

		#region IDisposable Members

		/// <summary>
		/// Disposes the resources of the object.
		/// </summary>
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
