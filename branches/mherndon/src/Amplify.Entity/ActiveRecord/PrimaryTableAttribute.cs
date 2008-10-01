//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.ActiveRecord
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	using Amplify.Data;

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class PrimaryTableAttribute : TableAttribute
	{

		public PrimaryTableAttribute()
		{
			this.Selection = "*";
		}

		public override bool IsPrimary
		{
			get
			{
				return true;
			}

		}


		public string GetSelectSqlFragment()
		{
			return this.GetSelectSqlFragment(false);
		}

		public string GetSelectSqlFragment(Adapter adapter)
		{
			return this.GetSelectSqlFragment(false, null, adapter);
		}

		public string GetSelectSqlFragment(bool distinct)
		{
			return this.GetSelectSqlFragment(distinct, null);
		}

		public string GetSelectSqlFragment(bool distinct, TableAttribute[] tables)
		{
			return this.GetSelectSqlFragment(distinct, tables, null);
		}

		public string GetSelectSqlFragment(bool distinct, TableAttribute[] tables, Adapter adapter)
		{
			string select = string.Format("SELECT {0} {1} ", distinct ? "DISTINCT" : "", this.GetSelectionFragment());

			if (tables != null)
				foreach (TableAttribute table in tables)
					select += "," + table.GetSelectionFragment() + " ";

			return string.Format("{0} FROM {1} ", select, this.GetTableFragment(adapter));
		}

		public string GetUpdateSqlFragment()
		{
			return this.GetUpdateSqlFragment(null);
		}

		public string GetUpdateSqlFragment(Adapter adapter)
		{
			return string.Format("UPDATE {0} ", this.GetTableName(adapter));
		}

		private string GetTableName(Adapter adapter)
		{
			string tableName = this.TableName;
			if (adapter != null)
				tableName = adapter.QuoteTableName(this.TableName);
			return tableName;
		}

		public string GetInsertSqlFragment()
		{
			return this.GetInsertSqlFragment(null);
		}

		public string GetInsertSqlFragment(Adapter adapter)
		{
			return string.Format("INSERT INTO {0} ", this.GetTableName(adapter));
		}

		public string GetDeleteSqlFragment()
		{
			return this.GetDeleteSqlFragment(null);
		}

		public string GetDeleteSqlFragment(Adapter adapter)
		{
			return string.Format("DELETE FROM {0} ", this.GetTableName(adapter));
		}
	}
}
