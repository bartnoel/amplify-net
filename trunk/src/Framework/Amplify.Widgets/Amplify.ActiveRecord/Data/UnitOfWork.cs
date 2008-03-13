//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.ActiveRecord
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Text;

	public delegate void ChangeValueHandler(object value);

	public class UnitOfWork : DecoratedObject, IUnitOfWork, INotifyPropertyChanged, INotifyPropertyChanging 
	{
		private bool isNew = true;
		private bool isModified = false;
		private bool isDeletable = false;
		
		public virtual bool IsNew
		{
			get { return this.isNew; }
		}

		public virtual bool IsModified
		{
			get { return this.isModified; }
		}

		public virtual bool IsValid
		{
			get { return true; }
		}

		public virtual bool IsDeletable
		{
			get { return this.isDeletable; }
		}

		public virtual bool IsSaveable
		{
			get { return (this.IsValid && this.IsModified); }
		}

		protected virtual void SetProperty(string propertyName, object value, ChangeValueHandler hander)
		{
			if (!Object.Equals(base.GetProperty(propertyName), value))
			{
				if (hander != null)
					hander(value);
				this.NotifyPropertyChanging(propertyName);
				base.SetProperty(propertyName, value);
				this.Validate(propertyName, value);
				this.NotifyPropertyChanged(propertyName);
			}
		}

		protected override void SetProperty(string propertyName, object value)
		{
			this.SetProperty(propertyName, value, null);
		}



		protected virtual void NotifyPropertyChanging(string propertyName)
		{
			PropertyChangingEventHandler eh = this.PropertyChanging;
			if (eh != null)
				eh(this, new PropertyChangingEventArgs(propertyName));
		}

		protected virtual void NotifyPropertyChanged(string propertyName) 
		{
			PropertyChangedEventHandler eh = this.PropertyChanged;
			if (eh != null)
				eh(this, new PropertyChangedEventArgs(propertyName));

			this.MarkModified();
		}

		protected virtual void Validate(string propertyName, object value)
		{

		}

		protected virtual void MarkModified() { this.isModified = true; }

		protected virtual void MarkNew() { 
			this.isNew = true;
			this.isModified = false;
		}

		protected virtual void MarkForDeletion() { this.isDeletable = true; }

		protected virtual void MarkNotDeleted() { this.isDeletable = false; }

		protected virtual void MarkOld() 
		{ 
			this.isModified = false;
			this.isNew = false; 
		}

		public virtual object Save()
		{
			if (this.IsDeletable)
				this.DeleteSelf();
			else {
				if (!this.IsSaveable)
					throw new InvalidOperationException("The save operation can not be performed");
		
				if (this.IsNew)
					this.Insert();
				else
					this.Update();

				this.MarkOld();
			}
			return this;
		}

		protected virtual void Update() 
		{
			throw new NotImplementedException();
		}

		protected virtual void Insert() 
		{
			throw new NotImplementedException();
		}

		protected virtual void DeleteSelf() 
		{
			throw new NotImplementedException();
		} 

		public virtual bool Delete() 
		{
			this.MarkForDeletion();
			this.Save();
			return true;
		}


		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region INotifyPropertyChanging Members

		public event PropertyChangingEventHandler PropertyChanging;

		#endregion
	}
}
