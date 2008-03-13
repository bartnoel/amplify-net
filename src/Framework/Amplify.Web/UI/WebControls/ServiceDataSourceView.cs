

namespace Amplify.Web.UI.WebControls
{
	using System.Web.UI;

	public class ServiceDataSourceView: DataSourceView 
	{

		

		public ServiceDataSourceView(ServiceDataSourceControl control)
		{
			this.Control = control;
		}


		protected ServiceDataSourceControl Control { get; set; }

		protected override System.Collections.IEnumerable ExecuteSelect(DataSourceSelectArguments arguments)
		{
			throw new System.NotImplementedException();
		}


		public override void Insert(System.Collections.IDictionary values, DataSourceViewOperationCallback callback)
		{
			base.Insert(values, callback);
		}

		public override void Update(System.Collections.IDictionary keys, System.Collections.IDictionary values, System.Collections.IDictionary oldValues, DataSourceViewOperationCallback callback)
		{
			base.Update(keys, values, oldValues, callback);
		}

		public override void Delete(System.Collections.IDictionary keys, System.Collections.IDictionary oldValues, DataSourceViewOperationCallback callback)
		{
			base.Delete(keys, oldValues, callback);
		}
	}
}
