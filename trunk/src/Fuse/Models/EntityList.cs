using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuse.Models
{
	using Amplify.Data;
	using Amplify.ObjectModel;
	using System.ComponentModel;


	public class EntityList<T> : BindingList<T>, IFill, IUnitOfWork where T: Base<T> 
	{
		private List<T> removed;
		private bool isNew = true;

		bool IFill.IsDeferred { get; set; }
		

		protected virtual List<T> Removed
		{
			get { 
				if (this.removed == null)
					this.removed = new List<T>();
				return this.removed;
			}
		}


		protected override void ClearItems()
		{
			this.Removed.Clear();
			base.ClearItems();
		}

		protected override object AddNewCore()
		{
			return Activator.CreateInstance<T>();
		}
		
		protected override void RemoveItem(int index)
		{
			T item = this[index];
			this.Removed.Add(item);
			base.RemoveItem(index);
		}

		#region IUnitOfWork Members

		public virtual bool IsNew
		{
			get {
				return this.isNew;
			}
		}

		public virtual bool IsModified
		{
			get {
				foreach (T item in this)
					if (item.IsModified)
						return true;
				return false;
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

		public bool IsSaveable
		{
			get { return this.IsModified && this.IsValid; }
		}

		#endregion

		#region ISaveable Members

		public object Save()
		{
			if (!this.IsValid)
				throw new Exception("Objects not in valid state");
			if (this.IsModified)
			{
				Adapter adapter = this[0].Adapter;
				bool commit = adapter.StartTransaction();
				foreach (T item in this.Removed)
					item.Delete();

				foreach (T item in this)
					item.Save();

				if (commit)
					adapter.Commit();
			}

			return this;
		}

		#endregion

		#region IDeletable Members

		public bool Delete()
		{
			Adapter adapter = this[0].Adapter;
			bool commit = adapter.StartTransaction();
			foreach (T item in this.Removed)
				item.Delete();

			foreach (T item in this)
				item.Delete(); 

			if (commit)
				adapter.Commit();

			return true;
		}

		#endregion

		#region IFill Members

		void IFill.Fill(System.Data.IDataReader datareader, IDictionary<string, object> includes)
		{
			
		}

		#endregion
	}
}
