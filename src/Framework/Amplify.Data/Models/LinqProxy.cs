using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Linq;

namespace Amplify.Models
{
	public class LinqProxy : ModelProxy 
	{
		private DataContext context;
		private string connectionString;

		public LinqProxy()
		{
			this.connectionString = ApplicationContext.ConnectionString;
		}

		public LinqProxy(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public DataContext Context
		{
			get
			{
				if(context == null) {
					context = (ApplicationContext.Properties[this.connectionString] as DataContext);
					if(context == null) {
						context = new DataContext(this.connectionString);
						ApplicationContext.Properties[this.connectionString] = context;
					}
				}
				return context;
			}
		}

		public void Update<T>(T obj) where T : Base<T>
		{
			
		}

		public void Insert<T>(T obj) where T : Base<T>
		{
			this.Context.GetTable<T>().InsertOnSubmit(obj);
		}

		public void Delete<T>(T obj) where T : Base<T>
		{
			this.Context.GetTable<T>().DeleteOnSubmit(obj);
		}

		public void Commit<T>(T obj) where T : Base<T>
		{
			this.Context.SubmitChanges();
		}
	}
}
