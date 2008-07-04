

namespace Amplify.ActiveRecord
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using Amplify.Linq;

	public abstract class Base
	{
		private Hash properties = Hash.New();

		public object this[string propertyName]
		{
			get { return this.Get(propertyName); }
			set { this.Set(propertyName, value); }
		}

		protected virtual object Get(string propertyName)
		{
			return this[propertyName];
		}

		protected virtual void Set(string propertyName, object value) 
		{
			this.properties[propertyName] = value;
		}
	}
}
