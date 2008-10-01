
namespace DefyTheGrind.Web.UI.Presenters
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Text;
	using System.Web;

	using Amplify.ActiveRecord;
	using Amplify.Linq;

	public delegate object SelectionNeeded();

	public class DataBoundPresenter<V, T> : UserControlPresenter<V> 
		where V : Views.IDataBoundView
		where T : Base<T> 
	{

		private Type dataType = null;
		private Hash viewChanges = null;


		public Hash ViewChanges
		{
			get {
				if (this.viewChanges == null)
				{
					object value = HttpContext.Current.Session[this.View.ClientID + "-Changes"];
					if (value == null)
					{
						value = new Hash();
						HttpContext.Current.Session[this.View.ClientID + "-Changes"] = value;
					}
					this.viewChanges = (Hash)value;
				}
				return this.viewChanges;
			}
		}
	

		public object Selection 
		{ 
			get {
				return HttpContext.Current.Session[this.View.ClientID];
			}
			set
			{
				HttpContext.Current.Session[this.View.ClientID] = value;
			}
		}

		protected bool DataBindingRequired  { get; private set; }

		public event SelectionNeeded SelectionNeeded;

		public DataBoundPresenter(V view): base(view)
		{
			this.dataType = typeof(T);

			if (!this.View.IsPostback)
			{
				this.RequiresDataBinding();
				this.ViewChanges.Clear();
			}

			this.View.DataSource.SelectObject += new EventHandler<Csla.Web.SelectObjectArgs>(Select);
			this.View.DataSource.InsertObject += new EventHandler<Csla.Web.InsertObjectArgs>(Insert);
			this.View.DataSource.UpdateObject += new EventHandler<Csla.Web.UpdateObjectArgs>(Update);
			this.View.DataSource.DeleteObject += new EventHandler<Csla.Web.DeleteObjectArgs>(Delete);
			this.View.DataSource.TypeName = this.dataType.Name;
			this.View.DataSource.TypeSupportsPaging = true;
			this.View.DataSource.TypeSupportsSorting = true;
			this.View.DataSource.TypeAssemblyName = this.dataType.AssemblyQualifiedName;
			this.View.DataSource.DataBinding += new EventHandler(DataBinding);
			this.View.UpdateView += new EventHandler(UpdateView);
			this.View.PreRender += new EventHandler(PreRender);
		}

		protected virtual void PreRender(object sender, EventArgs e)
		{
			if (this.DataBindingRequired)
				this.UpdateView(sender, e);
		}

		protected virtual void UpdateView(object sender, EventArgs e)
		{
			
		}

		public virtual void AddSort(string propertyName, ListSortDirection direction)
		{
			if (this.Selection != null && this.Selection is IEnumerable<T>)
			{
				switch (direction)
				{
					case ListSortDirection.Ascending:
						this.Selection = ((IEnumerable<T>)this.Selection).OrderBy(o => o[propertyName]);
						break;

					case ListSortDirection.Descending:
						this.Selection = ((IEnumerable<T>)this.Selection).OrderByDescending(o => o[propertyName]);
						break;
				}
				this.ViewChanges["Sort"] = string.Format("{0}:{1}", propertyName, direction);
			}
		}

		public virtual void AddFilter(string propertyName, object value)
		{
			if (this.Selection != null && this.Selection is IEnumerable<T>)
			{
				this.Selection = (from o in ((IEnumerable<T>)this.Selection) where o[propertyName].ToString().StartsWith(value.ToString(), StringComparison.CurrentCultureIgnoreCase) select o);
				this.ViewChanges["Filter"] = string.Format("{0}:{1}", propertyName, value);
			}
		}

		protected virtual void RequiresDataBinding()
		{
			this.DataBindingRequired = true;
		}

		protected virtual void DataBinding(object sender, EventArgs e)
		{
		
		}

		protected virtual void Delete(object sender, Csla.Web.DeleteObjectArgs e)
		{
			this.View.Validate();
			if (this.View.IsValid)
			{
				T item = null;
				ModelMetaInfo info = ModelMetaInfo.Get(this.dataType);
				List<T> list = null;

				if (this.Selection is IEnumerable<T>)
				{
					list = ((IEnumerable<T>)this.Selection).ToList();
					item = (from o in list where o[info.PrimaryKeys[0]] == e.Keys[info.PrimaryKeys[0]] select o).SingleOrDefault();
					item.Delete();
					list.Remove(item);
					this.Selection = list;
				}
				else
				{
					item = Activator.CreateInstance<T>();
					item = (T)((IFetchService)item).Fetch(new[] { string.Format(" {0} = '{1}' ", info.PrimaryKeys[0], e.Keys[info.PrimaryKeys[0]]) });
					item.Delete();
				}
			}
		}

		protected virtual void Update(object sender, Csla.Web.UpdateObjectArgs e)
		{
			this.View.Validate();
			if (this.View.IsValid)
			{
				T item = null;
				ModelMetaInfo info = ModelMetaInfo.Get(this.dataType);
				List<T> list = null;
				if (this.Selection is IEnumerable<T>)
				{
					list = ((IEnumerable<T>)this.Selection).ToList();
					item = (from o in list where o[info.PrimaryKeys[0]] == e.Keys[info.PrimaryKeys[0]] select o).SingleOrDefault();
				}
				else if (this.Selection != null)
				{
					item = this.Selection as T;

				}
				else
				{

					item = Activator.CreateInstance<T>();
					item = (T)((IFetchService)item).Fetch(new[] { string.Format(" {0} = '{1}' ", info.PrimaryKeys[0], e.Keys[info.PrimaryKeys[0]]) });
				}

				foreach (string key in e.Values.Keys)
					item[key] = e.Values[key];

				item.Save();

				if (list != null)
					this.Selection = list;
				else
					this.Selection = item;
			}
		}

		protected virtual void Insert(object sender, Csla.Web.InsertObjectArgs e)
		{
				this.View.Validate();
				if (this.View.IsValid)
				{
					ModelMetaInfo info = ModelMetaInfo.Get(typeof(T));
					T item = Activator.CreateInstance<T>();
					foreach (string key in e.Values.Keys)
						item[key] = e.Values[key];
					item.Save();

					if (this.Selection is IEnumerable<T>)
					{
						List<T> list = ((IEnumerable<T>)this.Selection).ToList();
						list.Add(item);
						this.Selection = list;
					}
					else
					{
						this.Selection = item;
					}
				}
		}



		protected virtual void Select(object sender, Csla.Web.SelectObjectArgs e) 
		{
			if (this.Selection == null)
			{
				SelectionNeeded eh = this.SelectionNeeded;
				if (eh != null)
					this.Selection = eh();
				else
					this.Selection = ((IFetchService)Activator.CreateInstance<T>()).Fetch();
			}

			this.ApplyViewChanges();
			e.BusinessObject = this.Selection;
		}

		protected virtual void ApplyViewChanges()
		{
			if(this.Selection is IEnumerable<T>) {
				Hash changes = this.ViewChanges;
				if (changes.Count > 0)
				{
					if(changes.ContainsKey("Sort")) {
						object[] split = changes["Sort"].ToString().Split(":");
						this.AddSort(split[0].ToString(), (ListSortDirection)Enum.Parse(typeof(ListSortDirection), split[1].ToString())); 
					}
					if(changes.ContainsKey("Filter")) {
						object[] split = changes["Filter"].ToString().Split(":");
						this.AddFilter(split[0].ToString(), split[1]);
					}
				}
			}
		}

	}
}
