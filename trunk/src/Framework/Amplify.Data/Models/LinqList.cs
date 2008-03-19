using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace Amplify.Models
{
	//public class LinqList<L, T> : System.ComponentModel.BindingList<T> 
	//    where L: LinqList<L, T>
	//    where T: LinqBase<T>
	//{
	//    private DataContext context;
	//    private List<T> insertList;
	//    private List<T> removeList;

	//    protected List<T> InsertList
	//    {
	//        get {
	//            if (this.insertList == null)
	//                this.insertList = new List<T>();
	//            return this.insertList; 
	//        }
	//        set { this.insertList = value; }
	//    }

	//    protected List<T> RemoveList
	//    {
	//        get {
	//            if (this.removeList == null)
	//                this.removeList = new List<T>();
	//            return this.removeList;
	//        }
	//    }

	//    public static L New()
	//    {
	//        return Activator.CreateInstance<L>();
	//    }

	//    public static L Create(IEnumerable<T> items)
	//    {
	//        L list = New();
	//        list.AddRange(items);
	//        list.Save();
	//        return list;
	//    }

		


	//    protected override void RemoveItem(int index)
	//    {
	//        base.RemoveItem(index);
	//    }

	//    protected override void InsertItem(int index, T item)
	//    {
	//        base.InsertItem(index, item);
	//    }

	//}
}
