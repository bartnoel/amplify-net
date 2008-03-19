using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Amplify.Data;
using Amplify.Linq;

namespace Amplify.Models
{
	public class ModelProxy
	{

		protected virtual object CreateTransaction()
		{
			return null;
		}

		public virtual IEnumerable<T> Select<T>(IOptions options) where T : Base<T>
		{
			return null;
		}

		public virtual void Delete<T>(T obj) where T : Base<T>
		{

		}

		public virtual void Insert<T>(T obj) where T : Base<T>
		{

		}

		public virtual void Update<T>(T obj) where T : Base<T>
		{

		}



		public virtual void Commit<T>(T obj) where T : Base<T>
		{

		}
	}
}
