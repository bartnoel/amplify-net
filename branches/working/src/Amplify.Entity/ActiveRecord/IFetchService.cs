using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify.ActiveRecord
{
	public interface IFetchService
	{
		object Fetch(params object[] conditions);
	}
}
