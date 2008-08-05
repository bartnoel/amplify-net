//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace DefyTheGrind.Web.UI.WebControls
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Web.UI;

	public interface IDataBoundControl : IDataBind
	{
		string DataMember { get; set; }
		string DataSourceID { get; set; }
		IDataSource DataSourceObject { get; }
		object DataSource { get; set; }
		event EventHandler DataBound;
	}
}
