

namespace Amplify.Data
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;

	

	using Amplify.Linq;

	public class SelectQuery
	{
		private Hash properties;
		

		public SelectQuery(Adapter adapter) 
		{
			this.Adapter = adapter;
			this.Sql = "";
		}


		protected Adapter Adapter { get; set; }
		

		public object this[string propertyName]
		{
			get { return this.properties[propertyName]; }
			set { this.properties[propertyName] = value; }
		}

		protected string Sql { get; set; }
		
		public List<TableDefinition> Tables
		{
			get {
				List<TableDefinition> list = null;
				if (list == null)
					list = new List<TableDefinition>();

				return list;
			}
		}

		
		public SelectQuery EqualTo
		{
			get
			{
				this.Sql += " =";
				return this;
			}
		}

		public SelectQuery And
		{
			get
			{
				this.Sql += " AND";
				return this;
			}
		}

		public SelectQuery GroupExp
		{
			get
			{
				this.Sql += " (";
				return this;
			}
		}

		public SelectQuery AndGroup
		{
			get
			{
				this.Sql += " AND (";
				return this;
			}
		}

		public SelectQuery On
		{
			get
			{
				this.Sql += " ON";
				return this;
			}
		}

		public SelectQuery Left
		{
			get
			{
				this.Sql += " LEFT";
				return this;
			}
		}

		public SelectQuery Outer
		{
			get
			{
				this.Sql += " OUTER";
				return this;
			}
		}

		public SelectQuery Or
		{
			get
			{
				this.Sql += " OR";
				return this;
			}
		}

		public SelectQuery OrGroup
		{
			get
			{
				this.Sql += " OR (";
				return this;
			}
		}

		public SelectQuery EndGroup
		{
			get
			{
				this.Sql += " ) ";
				return this;
			}
		}

		public SelectQuery IsEqualTo
		{
			get
			{
				this.Sql += " =";
				return this;
			}
		}

		public SelectQuery IsGreaterThan
		{
			get
			{
				this.Sql += " >";
				return this;
			}
		}

		public SelectQuery IsBetweenValues(IComparable valueA, IComparable valueB)
		{
			if (valueA.GetType() != valueB.GetType())
				throw new InvalidCastException("value a and value b must be of the same type");
			this.Sql += string.Format(" BETWEEN {0} AND {1}",
				this.Adapter.Quote(valueA),
				this.Adapter.Quote(valueB));

			return this;
		}

		public SelectQuery IsGreaterThanOrEqualTo
		{
			get
			{
				this.Sql += " >=";
				return this;
			}
		}

		public SelectQuery IsLessThan
		{
			get
			{
				this.Sql += " <";
				return this;
			}
		}

		public SelectQuery IsLessThanOrEqualTo
		{
			get
			{
				this.Sql += " <=";
				return this;
			}
		}

		public SelectQuery Lte
		{
			get
			{
				this.Sql += " =";
				return this;
			}
		}

		public SelectQuery Equals
		{
			get
			{
				this.Sql += " =";
				return this;
			}
		}

		public SelectQuery Distinct
		{
			get
			{
				this.Sql += " DISTINCT";
				return this;
			}
		}

		public SelectQuery Join(string target)
		{
			this.Sql += string.Format(" JOIN {0}\t\n\n",
				this.Adapter.QuoteTableName(target));

			return this;
		}

		public SelectQuery Join(string target, string alias)
		{
			this.Sql += string.Format(" JOIN {0} AS {1}\t\n\n",
				this.Adapter.QuoteTableName(target), alias);

			return this;
		}

		public SelectQuery Select(string selection)
		{
			this.Sql += selection;
			return this;
		}

		public SelectQuery From(string target)
		{
			this.Sql += string.Format(" FROM {0}", this.Adapter.QuoteTableName(target));
			return this;
		}

		public SelectQuery From(string target, string alias)
		{
			this.Sql += string.Format(" FROM {0} AS {1}",
				this.Adapter.QuoteTableName(target), alias);
			return this;
		}

		public SelectQuery Where()
		{
			this.Sql += " WHERE";
			return this;
		}

		public SelectQuery Where(string clause, params object[] values)
		{
			int count = 0;

			clause = Regex.Replace(clause, "\\?", delegate(Match item) {
				object replacement = values[count].ToString();
				count++;
				return this.Adapter.Quote(replacement);
			});

			this.Sql += clause;
			return this;
		}

		public SelectQuery Column(string tableAlias, string column)
		{
			this.Sql += string.Format(" {0}.{1}", tableAlias, column);
			return this;
		}

		public SelectQuery Column(string column)
		{
			this.Sql += string.Format(" {0}", this.Adapter.QuoteColumnName(column));
			return this;
		}

		public SelectQuery Value(object value)
		{
			this.Sql += string.Format(" {0}", this.Adapter.Quote(value));
			return this;
		}

		public SelectQuery WhereColumn(string column)
		{
			this.Sql += string.Format(" {0}", this.Adapter.QuoteColumnName(column));
			return this;
		}

		public SelectQuery WhereColumn(string tableAlias, string column)
		{
			this.Sql += string.Format(" WHERE {0}.{1}", tableAlias, this.Adapter.QuoteColumnName(column));
			return this;
		}

		public IDataReader ToReader()
		{
			return this.Adapter.ExecuteReader(this.Sql);
		}

		public T ToSingle<T>()
		{
			T item = Activator.CreateInstance<T>();
			using (IDataReader dr = this.ToReader())
			{
				if (dr.Read())
					PropertyMapper.Map(dr, item);
			}
			return item;
		}

		public List<T> ToList<T>()
		{
			List<T> list = new List<T>();

			using(IDataReader dr = this.ToReader())
			{
				while (dr.Read())
				{
					T item = Activator.CreateInstance<T>();
					PropertyMapper.Map(dr, item);
					list.Add(item);
				}
			}

			return list;
		}

		public override string ToString()
		{
			return this.Sql;
		}

	}
}
