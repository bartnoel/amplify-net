using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Amplify.Data.Entity
{
	public class EntityList<T> : BindingList<T>, 
		INotifyPropertyChanged, 
		INotifyPropertyChanging,
		ITrackChanges
		where T: ITrackChanges, INotifyPropertyChanged, INotifyPropertyChanging 
	{

		private IPropertyInfo property;
		private List<T> deletedList;
		private bool deleteAll = false;
		private bool isNew = false;


		public EntityList()
		{

		}

		public EntityList(IPropertyInfo property)
		{
			this.property = property;
		}

		public bool IsRoot
		{
			get { return this.property == null; }
		}

		protected virtual List<T> DeletedList 
		{
			get {
				if (this.deletedList == null)
					this.deletedList = new List<T>();
				return this.deletedList;
			}
		}

		#region ITrackChanges Members

		public virtual bool IsModified
		{
			get {
				foreach (T item in this.DeletedList)
					if (!item.IsNew)
						return true;

				foreach (T item in this)
					if (item.IsModified)
						return true;

				return false;
			}
		}

		bool ITrackChanges.IsMarkedForDeletion
		{
			get {
				return false;
			}
		}

		public virtual bool IsNew
		{
			get {
				return this.isNew;
			}
		}

		public virtual bool IsValid
		{
			get {
				foreach (T item in this)
					if (!item.IsValid)
						return false;
				return true;
			}
		}

		public virtual bool IsEntityValid
		{
			get { return this.IsValid; }
		}

		public virtual bool IsEntityModified
		{
			get { return this.IsModified; }
		}

		public virtual void AddRange(IEnumerable<T> range)
		{
			foreach (T item in range)
				this.Add(item);
		}

		protected override void InsertItem(int index, T item)
		{
			base.InsertItem(index, item);
			this.OnPropertyChanged("Items[]");
			this.OnPropertyChanged("Count");
		}



		protected override void RemoveItem(int index)
		{
			T item = this[index];
			this.DeletedList.Add(item);
			base.RemoveItem(index);
			this.OnPropertyChanged("Items[]");
			this.OnPropertyChanged("Count");
		}

		protected override void ClearItems()
		{
			
			base.ClearItems();
			this.OnPropertyChanged("Items[]");
			this.OnPropertyChanged("Count");
		}

		#endregion

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eh = this.PropertyChanged;
			if (eh != null)
				eh(this, new PropertyChangedEventArgs(propertyName));
		}

		#region INotifyPropertyChanging Members

		public event PropertyChangingEventHandler PropertyChanging;

		#endregion

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangingEventHandler eh = this.PropertyChanging;
			if(eh != null)
				eh(this, new PropertyChangingEventArgs(propertyName));
		}
	}
}
