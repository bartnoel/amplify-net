

namespace Amplify.Data
{
    using System;
    using System.Collections.Generic;
    
    using System.Text;
    using System.Text.RegularExpressions;

	using Amplify.Linq;
	using Amplify.Diagnostics;

    public abstract class Column : ColumnDefinition
    {
 
        public Column(string name, string @default) 
        {
			this.IsNull = true;
            this.Name = name;
			if (@default.StartsWith("("))
				@default = @default.TrimStart("(".ToCharArray()).TrimEnd(")".ToCharArray()).
					TrimStart("'".ToCharArray()).TrimEnd("'".ToCharArray());
			this.Default = this.TypeCast(@default);    
            
        }

        public Column(string name, string @default, string sqlType) 
            : this(name, @default)
        {
			this.SqlType = sqlType;
			this.Type = this.SimplifiedType(sqlType);
			this.Limit = this.ExtractLimit(sqlType);
			this.Precision = this.ExtractPrecision(sqlType);
			this.Scale = this.ExtractScale(sqlType);
        }

        public Column(string name, string @default, string sqlType, bool isNullable) :
			this(name, @default, sqlType)
        {
            this.IsNull = isNullable;
        }

        
        public Type ClrType { get; set; }
        public string SqlType { get; set; }

		public bool IsSpecial { get; set; }
        
        

        public bool IsText {
            get {
                string[] types = new string[] { @string, @text };
					foreach(string type in types)
						if(this.Type.ToLower() == type.ToLower())
							return true;
					return false;
            }
        }

        public bool IsNumber
        {
            get {
				string[] types = new string[] { @float, @integer, @decimal };
				foreach (string type in types)
					if (type.ToLower() == this.Type.ToLower())
						return true;
				return false;
            }
        }

		


        public virtual object TypeCast(object value) 
        {
            if(value == null)
                return null;

            switch(value.GetType().ToString().Replace("System.", "")) 
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

		protected virtual bool ExtractIsNull(string sql)
		{
			return !StringUtil.IsMatch(sql, "not null", RegexOptions.IgnoreCase);
		}

		protected virtual bool ExtractIsUnique(string sql)
		{
			return StringUtil.IsMatch(sql, "unique", RegexOptions.IgnoreCase);
		}

		protected virtual List<String> ExtractChecks(string sql)
		{
			List<string> list = new List<string>();
			MatchCollection ms = Regex.Matches(sql, @"^(check)\((\d+\=\<\>\!\w+\s+)\)", RegexOptions.IgnoreCase);
			if (ms.Count > 0)
			{
				foreach (Match m in ms)
					list.Add(m.Groups[2].Value);
			}
			return list; 
		}

	

        protected virtual int? ExtractPrecision(string sql)
        {
            Match m = Regex.Match(sql, @"^(numeric|decimal|number)\((\d+)(,\d+)?\)", RegexOptions.IgnoreCase);
			if (!m.Success)
				return null;
			return  Convert.ToInt32(m.Groups[2].Value);
        }

        protected virtual int? ExtractScale(string sql)
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
