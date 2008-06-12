using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Amplify
{
	public class NameValueContext : NameObjectCollectionBase 
	{
		public void Add(string name, object value)
		{
			base.BaseAdd(name, value);
		}

		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		public object this[string name]
		{
			get { return this.BaseGet(name); }
			set { this.BaseSet(name, value); }
		}

		public object this[int index]
		{
			get { return this.BaseGet(index); }
			set { this.BaseSet(index, value); }
		}

		public void Clear()
		{
			base.BaseClear();
		}
	}
}
