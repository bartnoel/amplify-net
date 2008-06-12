//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Data.SqlClient
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;

	using Amplify.Diagnostics; 
	using Amplify.Linq;
	

	public class SqlColumn : Column 
	{

		public SqlColumn(string name, string @default, string sqlType, bool isNullable, bool isPrimary): 
			base(name, @default, sqlType, isNullable) {
			this.IsPrimaryKey = isPrimary;
			this.IsSpecial = sqlType.IsMatch("text|ntext|image", RegexOptions.IgnoreCase);
			//the limit for nvarchar comes back as limit * 2
			this.Limit = (this.Type == @string && sqlType.Trim().StartsWith("n")) ? this.Limit / 2 : this.Limit; 
			this.Limit = (this.Type != @float && this.Type != @string) ? null : this.Limit; 
		}
	}
}
