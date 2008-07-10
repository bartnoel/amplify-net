

namespace Amplify.ActiveRecord
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Text;

	using Amplify.Linq;
	using Amplify.ObjectModel;

	public abstract class Base: IUnitOfWork, INotifyPropertyChanging, INotifyPropertyChanged 
	{
		private Hash properties = new Hash();
		private bool isValid = true;
		private bool isNew = false;
		private bool isModified = false;

		internal protected virtual bool IsValid { get { return this.isValid; } }

		internal protected virtual bool IsNew 
		{
			get { return this.isNew; }
			set { this.isNew = value; }
		}

		internal protected virtual bool IsModified 
		{
			get { return this.isModified; }
			set { this.isModified = value; }
		}

		public object this[string propertyName]
		{
			get { return this.Get(propertyName); }
			set { this.Set(propertyName, value); }
		}

		protected virtual DateTime GetDate(string propertyName)
		{
			object value = this.Get(propertyName);
				if(value == null)
					return DateTime.MinValue;
			return (DateTime)value;
		}

		protected virtual string GetString(string propertyName)
		{
			object value = this.Get(propertyName);
			if (value == null)
				return "";
			return (string)value;
		}

		protected virtual int GetInt32(string propertyName)
		{
			object value = this.Get(propertyName);
			if (value == null)
				return 0;
			return (int)value;
		}

		protected virtual object Get(string propertyName)
		{
			return this.properties[propertyName];
		}

		protected virtual void Set(string propertyName, object value) 
		{
			this.properties[propertyName] = value;
		}

		public void Send(string propertyName, object value)
		{
			this.Set(propertyName, value);
		}

		public void Send(IDictionary<string, object> values)
		{
			foreach (KeyValuePair<string, object> item in values)
				this.Set(item.Key, item.Value);
		}

#if LINQ
		public void Send(params Func<object, object>[] values)
		{
			this.Send(Hash.New(values));
		}
#endif 

		protected void NotifyPropertyChanged(string propertyName, object value)
		{
			PropertyChangedEventHandler  eh = this.PropertyChanged;
			if (eh != null)
				eh(this, new PropertyChangedEventArgs(propertyName));
		}

		protected void NotifyPropertyChanging(string propertyName, object value)
		{
			PropertyChangingEventHandler eh = this.PropertyChanging;
			if (eh != null)
				eh(this, new PropertyChangingEventArgs(propertyName));
		}

		public virtual object Save() 
		{
			return null;
		}

		public virtual bool Delete()
		{
			return false;
		}


		

		#region IUnitOfWork Members

		bool IUnitOfWork.IsNew
		{
			get { return this.IsNew; }
		}

		bool IUnitOfWork.IsModified
		{
			get { return this.IsModified; }
		}

		bool IUnitOfWork.IsValid
		{
			get { return this.IsValid; }
		}

		bool IUnitOfWork.IsSaveable
		{
			get { return this.IsModified && this.IsValid; }
		}

		#endregion

		#region ISaveable Members

		object ISaveable.Save()
		{
			return this.Save();
		}

		#endregion

		#region IDeletable Members

		bool IDeletable.Delete()
		{
			return this.Delete();
		}

		#endregion

		#region INotifyPropertyChanging Members

		public event PropertyChangingEventHandler PropertyChanging;

		#endregion

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}
