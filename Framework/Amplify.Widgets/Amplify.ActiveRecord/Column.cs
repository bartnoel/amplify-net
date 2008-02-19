

namespace Amplify.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

	using Amplify.Linq;

    public abstract class Column : SchemaBase
    {
 
        public Column(string name, string @default) 
        {
			this.IsNullable = true;
            this.Name = name;
			this.Default = this.TypeCast(@default);    
            
        }

        public Column(string name, string @default, string sqlType) 
            : this(name, @default)
        {
			this.SqlType = sqlType;
			this.Limit = this.ExtractLimit(sqlType);
			this.Precision = this.ExtractPrecision(sqlType);
			this.Scale = this.ExtractScale(sqlType);
        }

        public Column(string name, string @default, string sqlType, bool isNullable) :
			this(name, @default, sqlType)
        {
            this.IsNullable = isNullable;
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public Type ClrType { get; set; }
        public string SqlType { get; set; }
        public int Limit { get; set; }
		public bool IsPrimaryKey { get; set; }
        public bool IsNullable { get; set; }
		public bool IsSpecial { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }
        public object Default { get; set; }

        public bool IsText {
            get {
                return new[] { @string, @text }.Contains(this.Type);
            }
        }

        public bool IsNumber
        {
            get {
                return new[] { @float, @integer, @decimal }.Contains(this.Type);
            }
        }

		public virtual string SimplifiedType(string fieldType)
		{
			switch (fieldType.ToLower())
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
					return datetime;
				case "timestamp":
					return timestamp;
				case "time":
					return time;
				case "date":
					return date;
				case "clob":
				case "text":
					return text;
				case "blob":
				case "binary":
					return binary;
				case "char":
				case "string":
					return @string;
				case "boolean":
					return boolean;
				default:
					return fieldType;
			}
		}


        public virtual object TypeCast(object value) 
        {
            if(value == null)
                return null;

            switch(this.Type.ToLower()) 
			{
				case @text:
				case @string:
					return value;
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
				case @binary:
					throw new InvalidOperationException("Casting to binary is not currently accepted");
				case @boolean:
					return Convert.ToBoolean(value);
				default:
					return value;
            }
        }

        protected int ExtractLimit(string sql)
        {
            Match m =  Regex.Match(sql, @"\((.*)\)");
            return (m == null) ? -1 : Convert.ToInt32(m.Groups[1].Value); 
        }

        protected int ExtractPrecision(string sql)
        {
            Match m = Regex.Match(sql, @"^(numeric|decimal|number)\((\d+)(,\d+)?\)", RegexOptions.IgnoreCase);
            return (m.Success) ? -1 : Convert.ToInt32(m.Groups[2].Value);
        }

        protected int ExtractScale(string sql)
        {
            if(Regex.Match(sql, @"^(numeric|decimal|number)\((\d+)\)", RegexOptions.IgnoreCase).Success) 
                return 0;
            Match m = Regex.Match(sql, @"^(numeric|decimal|number)\((\d+)(,(\d+))\)", RegexOptions.IgnoreCase);
            return (m.Success) ? -1 : Convert.ToInt32(m.Groups[4].Value);
        }


    }
}
