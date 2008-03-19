using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Model
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple= true)]
	public class TableAttribute : System.Attribute
	{

		public string TableName { get; set; }


		public override object TypeId
		{
			get
			{
				return this.TableName;
			}
		}

		public TableAttribute(string tableName)
		{
			this.TableName = tableName;
		}
	}
}
