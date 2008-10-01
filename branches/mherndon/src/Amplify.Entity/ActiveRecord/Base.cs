//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.ActiveRecord
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data;
	using System.Text;

	using Amplify.Linq;
	using Amplify.ObjectModel;

	[Serializable]
	public abstract class Base: IUnitOfWork, INotifyPropertyChanging, INotifyPropertyChanged, IDecoratedObject
	{
		private Hash properties = new Hash();
		private bool isValid = true;
		private bool isNew = true;
		private bool isModified = false;
		private bool isDeleted = false;

		public static readonly object Unset = "{!unset!}";

		internal protected virtual bool IsValid { get { return this.isValid; } }

		internal protected virtual bool IsNew 
		{
			get { return this.isNew; }
			set { this.isNew = value; }
		}

		internal protected virtual bool IsDeleted { get { return this.isDeleted; } }

		internal protected virtual bool IsModified 
		{
			get { return this.isModified; }
			set { this.isModified = value; }
		}

		protected object this[string propertyName]
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

		protected abstract void Set(string propertyName, object value, bool markChanged, bool checkType);

		protected virtual void Set(string propertyName, object value, bool markChanged)
		{
			bool changed = !Object.Equals(Get(propertyName), value);
			if (changed)
			{
				if (markChanged)
					this.NotifyPropertyChanging(propertyName, value);

				this.Set(propertyName, value);

				if (markChanged)
				{
					this.IsModified = true;
					this.NotifyPropertyChanged(propertyName, value);
				}
			}
		}

		protected virtual void Set(string propertyName, object value) 
		{
			this.properties[propertyName] = value;
		}

		public void Send(string propertyName, object value)
		{
			this.Set(propertyName, value, true, true);
		}

		public void Send(IDictionary<string, object> values)
		{
			foreach (KeyValuePair<string, object> item in values)
				this.Set(item.Key, item.Value, true, true);
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

		protected virtual void Fill(object state) 
		{
			this.IsNew = false;
		}

		public virtual object Save() 
		{
			if (this.IsDeleted)
				this.Delete();

			if (this.IsModified && this.IsValid)
			{
				if (this.IsNew)
					this.Insert();
				else
					this.Update();
				this.SaveChildren();
				this.IsNew = false;
				this.IsModified = false;
			}

			return this;
		}

		protected virtual void Insert() { }

		protected virtual void Update() { }

		protected virtual void SaveChildren() { }

		protected virtual void DeleteChildren() { }

		public virtual bool Delete()
		{
			this.DeleteChildren();
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

		#region IDecoratedObject Members

		object IDecoratedObject.this[string propertyName]
		{
			get { return this.Get(propertyName); }
			set { this.Set(propertyName, value, true, true); }
		}

		#endregion
	}
}
