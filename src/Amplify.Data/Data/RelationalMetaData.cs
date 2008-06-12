using System;
using System.Collections.Generic;
  using System.Linq;
using System.Text;

namespace Amplify.Data
{
	public abstract class RelationalMetaData : IRelationalMetaData 
	{
		protected List<ITableEntityDescriptor> TableList { get; set; }
		private Validation.ValidationRules rules;


		public RelationalMetaData(Type type)
		{
			this.TableList= new List<ITableEntityDescriptor>();
			this.Extract(type);
		}

		protected abstract void Extract(Type type);


		#region IRelationalMetaData Members
		public Validation.ValidationRules ValidationRules
		{
			get {
				if (this.rules == null)
					this.rules = new Validation.ValidationRules();
				return this.rules;
			}
		}

		public IEnumerable<ITableEntityDescriptor> Tables
		{
			get
			{
				return this.TableList;
			}
		}

		#endregion

		
	}
}
