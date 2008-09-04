//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Models
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data;
	using System.Data.Linq;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Text;
	using System.Reflection;

	using Amplify.Data;
	using Amplify.Linq;
	using Amplify.Data.Validation;

	public partial class Base<T> : DecoratedObject,  IService , INotifyPropertyChanged, INotifyPropertyChanging 
		where T: Base<T>
	{
	
		private static bool initializeOnce = false;

		public event PropertyChangedEventHandler PropertyChanged;
		public event PropertyChangingEventHandler PropertyChanging;

		public Base()
		{
			this.StaticInitialize();
			this.Initialize();
		}


		protected void Merge(IDictionary<string, object> values)
		{
			foreach (string key in values.Keys)
				this[key] = values[key];
		}

		protected void Merge(IDecoratedObject values)
		{
			this.EachKey(key => this[key] = values[key]);
		}



		protected void SetProperty(string propertyName, object value, bool notify)
		{
			if(!Object.Equals(this[propertyName], value)) { 
				if (notify)
					this.OnPropertyChanging(propertyName);
				base.SetProperty(propertyName, value);
				if (notify)
					this.OnPropertyChanged(propertyName);
				this.Validate(propertyName, value);
			}
		}


		protected virtual void OnPropertyChanging(string propertyName)
		{
			PropertyChangingEventHandler eh = this.PropertyChanging;
			if (eh != null)
				eh(this, new PropertyChangingEventArgs(propertyName));
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eh = this.PropertyChanged;
			if (eh != null)
				eh(this, new PropertyChangedEventArgs(propertyName));
		}


		protected virtual void GetAttributesOfType()
		{
			Type type = this.GetType();
			PropertyInfo[] properties = type.GetProperties();

			this.GetClassAttributes(type);

			foreach (PropertyInfo property in properties)
				this.GetPropertyAttributes(property);
		}


		protected internal virtual void GetClassAttributes(Type type) 
		{

		}

		protected internal virtual void GetPropertyAttributes(PropertyInfo property)
		{
			property.GetCustomAttributes(typeof(ValidationAttribute), true)
				.Cast<ValidationAttribute>().Each(
					attr => this.AddValidationRule((ValidationRule)attr.Rule));

		}
	}
}
