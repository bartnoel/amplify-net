

namespace Amplify.Data
{
    using System;
    using System.Collections.Generic;
    
    using System.Text;
    using System.Text.RegularExpressions;

	using Amplify.Linq;
	using Amplify.Diagnostics;

    public abstract class ColumnExtractor : ColumnDefinition
    {
 
        public ColumnExtractor(string name, string @default) 
        {
			this.IsNull = true;
            this.Name = name;
			if (@default.StartsWith("("))
				@default = @default.TrimStart("(".ToCharArray()).TrimEnd(")".ToCharArray()).
					TrimStart("'".ToCharArray()).TrimEnd("'".ToCharArray());
			
			this.DefaultValue = this.TypeCast(@default);
			this.Default = @default;
        }

        public ColumnExtractor(string name, string @default, string sqlType) 
            : this(name, @default)
        {
			
			this.SqlType = this.SimplifiedType(sqlType);
			this.Limit = this.ExtractLimit(sqlType);
			this.Precision = this.ExtractPrecision(sqlType);
			this.Scale = this.ExtractScale(sqlType);
        }

        public ColumnExtractor(string name, string @default, string sqlType, bool isNullable) :
			this(name, @default, sqlType)
        {
            this.IsNull = isNullable;
        }

       
        public Type ClrType { get; set; }
		public string SimplifedType { get; set; }
		public object DefaultValue { get; set; }
       

        public bool IsText {
            get {
                string[] types = new string[] { @string, @text };
					foreach(string type in types)
						if(this.SqlType.ToLower() == type.ToLower())
							return true;
					return false;
            }
        }

        public bool IsNumber
        {
            get {
				string[] types = new string[] { @float, @integer, @decimal };
				foreach (string type in types)
					if (type.ToLower() == this.SqlType.ToLower())
						return true;
				return false;
            }
        }

		public virtual string SimplifiedType(string fieldType)
		{
			switch (fieldType.ToLower().Substring(0, fieldType.IndexOf("(")))
			{
				case "int":
					return integer;
				case "float":
				case "double":
					return @float;
				case "decimal":
				case "numeric":
				case "number":
					return @decimal;
				case "datetime":
				case "timestamp":
				case "time":
				case "date":
					return datetime;
				case "rowversion":
					return rowversion;
				case "clob":
				case "text":
				case "ntext":
					return text;
				case "blob":
				case "binary":
					return binary;
				case "char":
				case "nchar":
					return @char;
				case "string":
				case "nvarchar":
				case "varchar":
					return @string;
				case "boolean":
				case "bit":
					return boolean;
				case "uniqueidentifier":
					return guid;
				case "xml":
					return xml;
				case "currency":
					return currency;
				default:
					return fieldType;
			}
		}


        public virtual object TypeCast(object value) 
        {
            if(value == null)
                return null;

            switch(value.GetType().Name.ToLower()) 
			{
				case @text:
				case @string:
					return (value as string);
				case @integer:
					return Convert.ToInt32(value);
				case @decimal:
					return Convert.ToDecimal(value);
				case @float:
					return Convert.ToSingle(value);
				case @date:
				case @time:
				case @datetime:
				case @timestamp:
					return Convert.ToDateTime(value);
				case @rowversion :
					return new RowVersion((byte[])value);
				case @binary:
					return ((byte[])value);
				case @boolean:
					return Convert.ToBoolean(value);
				default:
					return value;
            }
        }

        protected int? ExtractLimit(string sql)
        {
			Match m = Regex.Match(sql, @"\((.*)\)");;
		
			if (!m.Success)
				return null;
			string value = m.Groups[1].Value;
			if (string.IsNullOrEmpty(value))
				return null;
            return Convert.ToInt32(value); 
        }

        protected int? ExtractPrecision(string sql)
        {
            Match m = Regex.Match(sql, @"^(numeric|decimal|number)\((\d+)(,\d+)?\)", RegexOptions.IgnoreCase);
			if (!m.Success)
				return null;
			return  Convert.ToInt32(m.Groups[2].Value);
        }

        protected int? ExtractScale(string sql)
        {
            if(Regex.Match(sql, @"^(numeric|decimal|number)\((\d+)\)", RegexOptions.IgnoreCase).Success) 
                return null;
            Match m = Regex.Match(sql, @"^(numeric|decimal|number)\((\d+)(,(\d+))\)", RegexOptions.IgnoreCase);
			if (!m.Success)
				return null;
			return Convert.ToInt32(m.Groups[4].Value);
        }


    }
}
