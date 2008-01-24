

namespace Amplify.ActiveRecord
{
	using System;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Linq;
	using System.Data.Linq;
	using System.Text;
	
	using Amplify.ActiveRecord.Data;

	public class ActsAsList<T, L> : BindingList<T>, IUnitOfWork, 
		IParent, INotifyPropertyChanged
		where T: Base<T>
		where L: ActsAsList<T, L>, new()
	{
		private List<T> deleteList;
		private bool raiseChangeEvents = false;
		internal protected static Data.AdapterBase<T> Adapter { get; set; }


		public ActsAsList()
		{
			
		}

		public ActsAsList(IList<T> list):base(list)
		{

		}

		protected List<T> DeleteList 
		{
			get { 
				if(this.deleteList == null)
					this.deleteList = new List<T>();
				return this.deleteList;
			}
		}


		public static L New()
		{
			return Activator.CreateInstance<L>();
		}

		public static L New(IEnumerable<T> items)
		{
			L list = New();
			list.AddRange(items);
			return list;
		}

		public static L Find(IOptions options)
		{
			return New(Adapter.Select(options));
		}

		public static L Find(string where, params object[] values)
		{
			return New(Adapter.SelectAll(new Options().Where(where, values)));
		}

		public static L FindBySql(string sql, params object[] values)
		{
			return New(Adapter.ExecuteQuery(sql, values));
		}

		public static L FindAll()
		{
			return New(Adapter.SelectAll(new Options()));
		}

		protected override void ClearItems()
		{
			base.ClearItems();
		}

		public void Assign(IEnumerable<T> items)
		{
			base.ClearItems();
			this.AddRange(items);
		}
		
		public void AddRange(IEnumerable<T> items)
		{
			foreach (T item in items)
				this.Add(item);
		}

		void IParent.RemoveChild(object item) 
		{
			this.Remove((T)item);
		}


		protected override void  InsertItem(int index, T item)
		{
			
			 ((IChild)item).SetParent(this);
 			 base.InsertItem(index, item);
		}

		protected void PauseEvents() 
		{
			this.raiseChangeEvents = this.RaiseListChangedEvents;
			this.RaiseListChangedEvents = false;
		}

		protected void UnPauseEvents() 
		{
			this.RaiseListChangedEvents = raiseChangeEvents;
		}

		protected override void RemoveItem(int index)
		{
			this.PauseEvents();
			T item = this[index];
			this.deleteList.Add(this[index]);
			base.RemoveItem(index);
			this.UnPauseEvents();
			if(this.RaiseListChangedEvents)
				this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
		}

		protected override void  SetItem(int index, T item)
		{
			
 			T child = default(T);
			this.PauseEvents();
			child = this[index];
			((IChild)item).SetParent(this);
			base.SetItem(index, item);
			this.UnPauseEvents();
			if(child != null)
				this.DeleteList.Add(child);
			if(this.RaiseListChangedEvents)
				this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index)); 
		}

		public EntitySet<T> ToEntitySet()
		{
			EntitySet<T> set = new EntitySet<T>();
			set.AddRange(this);
			return set;
		}


		#region IUnitOfWork Members

		public virtual bool IsNew
		{
			get {
				foreach(T item in this)
					if(!item.IsNew)
						return false;
				return true;
			}
		}

		public virtual bool IsModified
		{
			get { 
				foreach(T item in this)
					if(item.IsModified)
						return true;
				return false;
			}
		}

		public virtual bool IsValid
		{
			get { 
				foreach(T item in this)
					if(!item.IsValid)
						return false;
				return true;
			}
		}

		#endregion

		#region ISaveable Members

		public bool IsSaveable
		{
			get {  return this.IsModified && this.IsValid; }
		}

		object ISaveable.Save()
		{
			Adapter.SaveList(this, this.DeleteList);
			return this;
		}

		#endregion

		#region IDeletable Members

		public virtual  bool IsDeletable
		{
			get { return false; }
		}

		public bool Delete()
		{
			if (this.IsDeletable)
			{
				IEnumerable<T> items = this.ToArray();
				foreach (T item in items)
					this.Remove(item);

				Adapter.SaveList(this, this.DeleteList);
			}
			return this.IsDeletable && this.DeleteList.Count > 0;
		}

		#endregion

		#region INotifyCollectionChanged Members

		//public event NotifyCollectionChangedEventHandler CollectionChanged;

		


		//protected void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
		//{
		//    this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
		//}

		//protected void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index, int oldIndex)
		//{
		//    this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index, oldIndex));
		//}


		//protected void OnCollectionChanged(NotifyCollectionChangedAction action, object oldItem, object newItem, int index)
		//{
		//    this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
		//}

		//protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		//{
		//    NotifyCollectionChangedEventHandler eh = this.CollectionChanged;
		//    if (eh != null)
		//        eh(this, e);
		//}
 



		#endregion

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChange(string propertyName)
		{
			this.OnPropertyChange(new PropertyChangedEventArgs(propertyName));
		}

		protected void OnPropertyChange(PropertyChangedEventArgs e)
		{
			PropertyChangedEventHandler eh = this.PropertyChanged;
			if (eh != null)
				eh(this, e);
		}

		#endregion
	}
}
