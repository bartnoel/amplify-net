//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Data
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Globalization;
	using System.Text;

	public abstract partial class Adapter
	{
		/// <summary>
		/// Quotes the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public string Quote(object value)
		{
			return this.Quote(value, DbType.None);
		}

		/// <summary>
		/// Quotes the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="dbType">Type of the db.</param>
		/// <returns></returns>
		[SuppressMessageAttribute("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "pointless")]
		public string Quote(object value, DbType dbType)
		{
			if (value == null || (value is string && value.ToString().Trim().ToLowerInvariant() == "null"))
				return "NULL";

			switch (value.GetType().ToString())
			{
				case "System.String":
					if (dbType != DbType.None)
					{
						if (new List<DbType>() { DbType.Integer, DbType.Float }.Contains(dbType))
						{
							object temp = (dbType == DbType.Integer) ? 
								Convert.ToInt32(value, CultureInfo.InvariantCulture) : 
								Convert.ToSingle(value, CultureInfo.InvariantCulture);
							return temp.ToString();
						}
					}
					return QuoteString(value.ToString());
				case "System.Guid":
					return QuoteString(value.ToString().ToLowerInvariant());
				case "System.Byte[]":
					throw new ArgumentException("All binary values must be passed into an sql parameter");
				case "System.Boolean":
					if(dbType == DbType.AnsiString || dbType == DbType.String) 
						return (Convert.ToBoolean(value, CultureInfo.InvariantCulture)) ? this.QuotedTrue : this.QuotedFalse;
					else
						return (Convert.ToBoolean(value, CultureInfo.InvariantCulture)) ? "1" : "0";
				case "System.Decimal":
					return value.ToString();
				case "System.DateTime":
					return QuotedDate((DateTime)value);
				default:
					return QuoteString(value.ToString());
			}
		}

		/// <summary>
		/// Quotes the string.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public virtual string QuoteString(string value)
		{
			return string.Format(CultureInfo.InvariantCulture, "'{0}'", value.Replace("'", "''"));
		}

		/// <summary>
		/// Quotes the name of the column.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public virtual string QuoteColumnName(string name)
		{
			return string.Format(CultureInfo.InvariantCulture, "[{0}]", name);
		}

		/// <summary>
		/// Quotes the name of the table.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public virtual string QuoteTableName(string name)
		{
			return string.Format(CultureInfo.InvariantCulture, "[{0}]", name);
		}

		/// <summary>
		/// Gets the quoted true value.
		/// </summary>
		/// <value>The quoted true.</value>
		public virtual string QuotedTrue
		{
			get { return "true"; }
		}

		/// <summary>
		/// Gets the quoted false value.
		/// </summary>
		/// <value>The quoted false.</value>
		public virtual string QuotedFalse
		{
			get { return "false"; }
		}

		/// <summary>
		/// Quoteds the date.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public virtual string QuotedDate(DateTime value)
		{
			return this.QuoteString(value.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture));
		}
	}
}
