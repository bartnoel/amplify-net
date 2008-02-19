using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Amplify.Data.Sql
{
	using Amplify.Linq;

	public class SqlColumn : Column 
	{
		

		public SqlColumn(string name, string @default, string sqlType, bool isNullable, bool isPrimary): 
			base(name, @default, sqlType, isNullable) {
			this.IsPrimaryKey = isPrimary;
			this.IsSpecial = sqlType.Match("text|ntext|image", RegexOptions.IgnoreCase);
			this.Limit = (this.Type == @float || this.Type == @string) ? -1 : this.Limit; 
		}

		
	}
}
