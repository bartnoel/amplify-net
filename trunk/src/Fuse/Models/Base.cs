using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuse.Models
{
	using Amplify.Linq;
	using Amplify.Models;
	using System.Reflection;
	using System.Configuration;

	public class Base<T> : IUnitOfWork, IUniqueIdenitifer where  T:Base<T> 
	{

		private bool isNew = true;
		private bool isModified = false;
		private Hash properties = new Hash();
		internal static Amplify.Data.Adapter adapter;


		


		static Base()
		{
			T item = Activator.CreateInstance<T>();
			item.InitializeOnce();
		}

		public object this[string propertyName]
		{
			get {
				return properties[propertyName];
			}
			set {
				this.properties[propertyName] = value; 
			}
		}


		protected object GetColumn(string name)
		{
			return this.properties[name];
		}

		protected void SetColumn(string name, object value)
		{
			this.properties[name] = value;
		}

		protected object GetAssociation(string name)
		{
			object value = this.properties[name];
			if (value is IFill && ((IFill)value).IsDeferred)
				((IFill)value).Load(name, (T)this);
			return value;
		}

		protected void SetAssociation(string name, IFill value)
		{
			AssocationAttribute attr = this.Info.Assocations.SingleOrDefault(o => o.Property.Name == name);
			if (value != null)
			{
				value.SetParent(this.Info.Type.Name, this);
				
				value[attr.ForeignKey] = this[this.Info.PrimaryKeys[0]];
				this[name] = value;
			}
			else
			{
				IFill association = (this[name] as IFill);
				if (association != null)
				{
					association[attr.ForeignKey] = null;
					association.RemoveParent(this.Info.Type.Name);
				}
				this[name] = null;
			}
		}



		bool IFill.IsDeferred { get; set; }

		void IFill.Load(string name, IFill parent)
		{
			this.Load(name, parent);
		}

		void IFill.SetParent(string name, IFill parent)
		{
			this.SetParent(name, parent);
		}

		protected internal void Load(string name, object parent)
		{
			IFill temp = (IFill)parent;
			ModelMetaInfo parentInfo = ModelMetaInfo.Repository[parent.GetType()];
			AssocationAttribute attr = parentInfo.Assocations.SingleOrDefault(o => o.Property.Name == name);
			
			object id = temp[parentInfo.PrimaryKeys[0]];
			
			string query = "SELECT {0} FROM {1} WHERE {2} = '{3}'".Fuse(
				this.Info.PrimaryTable.Selection,
				this.Info.PrimaryTable.TableName,
				attr.ForeignKey,
				id);

			using(var dr = this.Adapter.Select(query)) 
			{
				while (dr.Read())
				{
					this.Fill(dr, null);
				}
			}
		}

		protected internal void SetParent(string name, object parent)
		{
			this[name] = parent;
		}


		internal ModelMetaInfo Info
		{
			get
			{
				Type type = typeof(T);
				if (ModelMetaInfo.Repository.ContainsKey(type))
					return ModelMetaInfo.Repository[type];
				return null;
			}
			set
			{
				ModelMetaInfo.Repository[typeof(T)] = value;
			}
		}	

		public virtual Amplify.Data.Adapter Adapter
		{
			get {
				if (adapter == null)
					adapter = Amplify.Data.Adapter.Create(ConfigurationManager.ConnectionStrings["development"].ConnectionString, "SqlClientCe");
				return adapter;
			}
		}


		protected void Initialize()
		{
			foreach (ColumnAttribute column in this.Info.Columns)
				this[column.Property.Name] = column.GetDefaultValue();

		}

		protected void InitializeOnce() 
		{
			if (Info == null)
			{
				Info = new ModelMetaInfo();
				Info.Type = this.GetType();
				object[] attrs = Info.Type.GetCustomAttributes(typeof(TableAttribute), true);
				TableAttribute primaryTable =  null;
				foreach(TableAttribute table in attrs) 
				{
					if(table.IsPrimary)
					{
						if(string.IsNullOrEmpty(table.TableName))
							table.TableName = Info.Type.Name.Pluralize();
						this.Info.PrimaryTable = table;
					}
					Info.Tables.Add(table);
				}

				PropertyInfo[] properties = Info.Type.GetProperties();
				foreach (PropertyInfo property in properties)
					this.ExamineProperty(property);

				List<string> primaryKeys = new List<string>();
				primaryTable.Columns.Where(column => column.IsPrimaryKey).Each(column => primaryKeys.Add(column.Name));
				this.Info.PrimaryKeys = primaryKeys.ToArray();
			}
		}

		protected void ExamineProperty(PropertyInfo property)
		{
			object[] attrs = property.GetCustomAttributes(typeof(ColumnAttribute), true);
			foreach (ColumnAttribute column in attrs)
			{
				column.Property = property;
				if (string.IsNullOrEmpty(this.Info.PrimaryTable.TableName))
				{
					column.TableName = this.Info.PrimaryTable.TableName;
					this.Info.PrimaryTable.Columns.Add(column);
				}
				else
					Info.Tables.Single(t => t.TableName == column.TableName).Columns.Add(column);
				
				Info.Columns.Add(column);
			}

			foreach (AssocationAttribute association in attrs)
			{
				association.Property = property;
				this.Info.Assocations.Add(association);
			}
		}

		protected void Initialize()
		{

		}

		

		#region IUnitOfWork Members

		public virtual bool IsNew
		{
			get { return this.isNew; }
			protected set { this.isNew = value; }
		}

		public virtual bool IsModified
		{
			get { return this.isModified; }
			protected set { this.isModified = value; }
		}

		public virtual bool IsValid
		{
			get { return true; }
		}

		public virtual bool IsSaveable
		{
			get { return this.IsModified && this.IsValid; }
		}

		#endregion

		#region ISaveable Members

		public virtual object Save()
		{
			if (this.IsModified)
			{
				bool commit = this.Adapter.StartTransaction();
				if (this.IsNew)
					this.Insert();
				else
					this.Update();
				this.SaveChildren();
				
				if (commit)
					this.Adapter.Commit();
			}
			return this;
		}


		protected virtual void Update()
		{
			string set = "";
			List<object> values = new List<object>();
			int index = 0;

			foreach(ColumnAttribute column in this.Info.PrimaryTable.Columns) 
			{
				if(!column.IsPrimaryKey) { 
					set += column.Name + " = {" + index.ToString() + "}, "; 
					values.Add(this[column.Property.Name]);
					index ++;
				}
			}
			
			string query = this.Adapter.GetUpdateSql(
				this.Info.PrimaryTable.TableName,
				set.TrimEnd(", "),
				this.Info.PrimaryKeys[0],
				this[this.Info.PrimaryKeys[0]]);

			this.Adapter.Update(query, values.ToArray());
		}

		protected virtual void Insert()
		{
			string columns = "";
			string valuesString = "";
			List<object> values = new List<object>();
			int index = 0;

			foreach (ColumnAttribute column in this.Info.PrimaryTable.Columns)
			{
				columns += column.Name + ", ";
				valuesString = "{" + index.ToString() + "}, ";
				values.Add(this[column.Property.Name]);
				index++;
			}

			string query = this.Adapter.GetInsertSql(
				this.Info.PrimaryTable.TableName,
				columns,
				valuesString);

			object id = this.Adapter.Insert(query, values.ToArray());

			if (this[this.Info.PrimaryKeys[0]].GetType() == typeof(Int32))
				this[this.Info.PrimaryKeys[0]] = id;
		}

		protected virtual void SaveChildren()
		{
			foreach (AssocationAttribute item in this.Info.Assocations)
			{
				IUnitOfWork work = (this[item.Property.Name] as IUnitOfWork);
				if(work != null && work is IHasAssocation)
					work.Save();
			}
		}


		#endregion

		#region IDeletable Members

		public virtual bool Delete()
		{
			
			
			string query = this.Adapter.GetDeleteSql(
							this.Info.PrimaryTable.TableName, 
							this.Info.PrimaryKeys[0], 
							this[Info.PrimaryKeys[0]]);
			bool delete = false;
			bool commit = this.Adapter.StartTransaction();

			this.DeleteChildren();

			if (this.Adapter.Delete(query) > 0)
				delete = true;

			if (commit)
				this.Adapter.Commit();

			return delete;
		}

		private virtual void DeleteChildren()
		{
			
		}

		#endregion

		#region IUniqueIdenitifer Members

		

		#endregion
	}
		

}
