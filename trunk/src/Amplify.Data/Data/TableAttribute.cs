using System;
using System.Collections.Generic;

using System.Text;

using Amplify.Linq;

namespace Amplify.Data
{
	[AttributeUsage(AttributeTargets.Class)]
	public class TableAttribute : System.Attribute, ITableDescriptor, ITableEntityDescriptor 
	{
		private List<IColumnDescriptor> columns;
		private Hash select;
		private Hash delete;


		public TableAttribute()
		{
			this.columns = new List<IColumnDescriptor>();
		}

		public string Name { get; set; }


		

		#region ITableDescriptor Members


		public IEnumerable<IColumnDescriptor> Columns
		{
			get { return this.columns; }
		}

		public IEnumerable<IColumnDescriptor> PrimaryKeys
		{
			get {
				List<IColumnDescriptor> list = new List<IColumnDescriptor>();
				foreach (IColumnDescriptor o in this.columns)
					if (o.IsPrimaryKey)
						list.Add(o);
				return list;
			}
		}

		public void AddColumn(IColumnDescriptor column)
		{
			this.columns.Add(column);
		}

		public void RemoveColumn(IColumnDescriptor column)
		{
			this.columns.Remove(column);
		}

		#endregion

		#region ITableEntityDescriptor Members

		public bool IsReadOnly { get; set; }
		public bool AllowUpdates { get; set; }
		public bool AllowInserts { get; set; }
		public bool AllowDeletes { get; set; }
		public bool IsPrimary { get; set; }

		#endregion

		#region ITableEntityDescriptor Members


		//string ITableEntityDescriptor.UpdateQuery { get; set; }

		//string ITableEntityDescriptor.InsertQuery { get; set; }

		//Amplify.Linq.Hash ITableEntityDescriptor.SelectQueries
		//{
		//    get
		//    {
		//        if (this.select == null)
		//            this.select = Hash.New();
		//        return this.select;
		//    }
		//    set { this.select = value; }
		//}

		//Amplify.Linq.Hash ITableEntityDescriptor.DeleteQuerues
		//{
		//    get
		//    {
		//        if (this.delete == null)
		//            this.delete = Hash.New();
		//        return this.delete;
		//    }
		//    set { this.delete = value; }
		//}


		#endregion

		
		
	}
}
