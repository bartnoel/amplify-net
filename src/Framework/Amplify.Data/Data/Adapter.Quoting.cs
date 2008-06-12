using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data
{
	using Amplify.Linq;
	using Amplify.Diagnostics;


	public abstract partial class Adapter
	{

		public string Quote(object value)
		{
			return this.Quote(value, null);
		}

		public string Quote(object value, ColumnDefinition column)
		{
			if (value == null)
				return "NULL";

			Log.Debug(value.GetType().ToString());
			switch (value.GetType().ToString())
			{
				case "System.String":
					if(column != null && new[] {integer, @float}.Contains(column.Type)) 
					{
						object temp = (column.Type == integer) ? Convert.ToInt32(value) : Convert.ToSingle(value);
						return temp.ToString();
					}
					return QuoteString(value.ToString());
				case "System.Guid":
					return QuoteString(value.ToString().ToLower());
				case "System.Byte[]":
					throw new ArgumentException("All binary values must be passed into an sql parameter");
				case "System.Boolean":
					if (column != null && column.Type == integer)
						return (Convert.ToBoolean(value)) ? "1" : "0";
					else
						return (Convert.ToBoolean(value)) ? this.QuotedTrue : this.QuotedFalse;
				case "System.Decimal":
					return value.ToString();
				case "System.DateTime":
					return QuotedDate((DateTime)value);
				default:
					return QuoteString(value.ToString());
			}
		}

		public virtual string QuoteString(string value)
		{
			return string.Format("'{0}'", value.Replace("'", "''"));
		}

		public virtual string QuoteColumnName(string name)
		{
			return string.Format("[{0}]", name);
		}

		public virtual string QuoteTableName(string name)
		{
			return string.Format("[{0}]", name);
		}

		public virtual string QuotedTrue
		{
			get { return "1"; }
		}

		public virtual string QuotedFalse
		{
			get { return "0"; }
		}

		public virtual string QuotedDate(DateTime value)
		{
			return this.QuoteString(value.ToString("MM-dd-yyyy HH:mm:ss"));
		}

	}
}
