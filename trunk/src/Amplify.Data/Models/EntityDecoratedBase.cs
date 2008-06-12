using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Amplify.Linq;

namespace Amplify.Models
{
	public class EntityDecoratedBase<T> : System.Data.Objects.DataClasses.EntityObject, IDecoratedObject 
	{
		private Hash hash = new Hash();


		protected virtual object GetValue(string propertyName)
		{
			if (hash.ContainsKey(propertyName))
				return hash[propertyName];
			return null;
		}

		protected virtual O GetValue<O>(string propertyName)
		{
			if (hash.ContainsKey(propertyName))
				return (O)hash[propertyName];
			return default(O);	
		}



		protected virtual void SetValue(string propertyName, object value)
		{
			this.hash[propertyName] = value;
		}


		protected virtual void OnPropertyChanged(string propertyName, object value)
		{
			this.OnPropertyChanged(propertyName);
		}

		protected override void OnPropertyChanged(string property)
		{
			base.OnPropertyChanged(property);
		}

		#region IDecoratedObject Members

		public object this[string propertyName]
		{
			get { return this.GetValue(propertyName); }
			set { this.SetValue(propertyName, value); }
		}

		#endregion
	}
}
