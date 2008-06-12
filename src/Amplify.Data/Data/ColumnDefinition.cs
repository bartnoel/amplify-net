using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data
{

	using Amplify.Linq;

    public class ColumnDefinition : SchemaBase
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int? Limit { get; set; }
        public int? Precision { get; set; }
        public int? Scale { get; set; }
        public object Default { get; set; }
        public bool IsNull { get; set; } 
		protected Adapter Adapter { get; set; }

		public ColumnDefinition(Adapter adapter)
		{
			this.Adapter = adapter;
		}
		
		protected virtual string TypeToSql()
		{
			return this.Adapter.TypeToSql(this.Type, this.Limit, this.Precision, this.Scale);
		}

		protected virtual string ToSql() 
		{
			string sql = "{0} {1}".Fuse(this.Adapter.QuoteColumnName(this.Name), this.TypeToSql());
			if(this.Type != "PrimaryKey")
				sql += this.Adapter.AddColumnOptions(Hash.New(Null => this.IsNull, Default => this.Default));
			return sql;
		}
	
        public override string  ToString()
        {
			return this.ToSql();        
        }
     
    }
}
