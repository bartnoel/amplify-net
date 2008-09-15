using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuse.Models.SqlClient
{
	using System.Data;

	using Amplify.Data;
	using Amplify.Data.SqlClient;
	using sc = Amplify.Data.SqlClient;

	public class SqlAdapter : Fuse.Models.Adapter 
	{

		private sc.SqlAdapter Adapter { get; set; } 

		public SqlAdapter(string connectionString)
		{
			this.Adapter = new Amplify.Data.SqlClient.SqlAdapter(connectionString);
		}

		public override List<string> GetTableNames()
		{
			return this.Adapter.GetTableNames();
		}

		public override List<Amplify.Data.ColumnDefinition> GetColumns(string tableName)
		{
			return (List<Amplify.Data.ColumnDefinition>)this.Adapter.GetColumns(tableName);
		}

		public override List<Amplify.Data.KeyConstraint> GetKeys(string tableName)
		{
			List<KeyConstraint> keys = new List<KeyConstraint>();

			using (IDataReader dr = this.Adapter.ExecuteReader(string.Format(@"
									SELECT DISTINCT
										   cu.column_name AS [column_name],
										   tc.constraint_name AS [constraint_name],
										   tc.constraint_type AS [constraint_type],
										  
										 CASE tc.is_deferrable WHEN 'NO' THEN 0 ELSE 1 END AS is_deferrable,
										 CASE tc.initially_deferred WHEN 'NO' THEN 0 ELSE 1 END AS is_deferred,
										   cc.check_clause AS [check],
										   rc.delete_rule AS [on_delete],
										   rc.update_rule AS [on_update],
										   rc.match_option AS [match_type],
										   rcu.table_name  AS [reference_table], 
										   rcu.column_name AS [reference_column]

									  FROM INFORMATION_SCHEMA.COLUMNS c
										 
									  LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
										on  tc.table_name = c.table_name
									  LEFT OUTER JOIN INFORMATION_SCHEMA.CHECK_CONSTRAINTS cc
										on  cc.constraint_name = tc.constraint_name 
									  LEFT OUTER JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc 
										ON	rc.constraint_schema = tc.constraint_schema AND
											rc.constraint_catalog = tc.constraint_catalog AND 
											rc.constraint_name = tc.constraint_name
									  LEFT OUTER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE cu
										ON  cu.constraint_name = tc.constraint_name
									  
									  LEFT OUTER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE rcu
										ON rc.unique_constraint_schema = cu.constraint_schema 
										AND rc.unique_constraint_catalog = cu.constraint_catalog 
										AND rc.unique_constraint_name = cu.constraint_name 
									 
									 WHERE tc.constraint_catalog = DB_NAME()
									   AND c.table_name = {0} AND tc.constraint_type != 'CHECK' 
									   
									 ORDER BY [constraint name] ", this.Adapter.QuoteString(tableName))))
			{

				while (dr.Read())
				{
					KeyConstraint key = null;
					switch (dr.GetString(2).Trim())
					{
						case "FOREIGN KEY":
							Amplify.Data.ForeignKeyConstraint fk
								= new Amplify.Data.ForeignKeyConstraint();
							fk.Name = dr.GetString(dr.GetOrdinal("constraint_name"));
							fk.TableName = tableName;
							fk.ColumnNames.Add(dr.GetString(dr.GetOrdinal("column_name")));
							fk.ReferenceTableName = dr.GetString(dr.GetOrdinal("reference_table"));
							fk.ReferenceColumns.Add(dr.GetString(dr.GetOrdinal("reference_column")));
							 fk.OnDelete =
								 (ConstraintDeleteAction)Enum.Parse(
									typeof(ConstraintDeleteAction), dr.GetString(dr.GetOrdinal("on_delete")));
							fk.OnUpdate = 
								(ConstraintUpdateAction)Enum.Parse(
								typeof(ConstraintUpdateAction), dr.GetString(dr.GetOrdinal("on_update")));
							key = fk;
							break;
						case "PRIMARY KEY":
							PrimaryKeyConstraint pk = new PrimaryKeyConstraint();
							pk.TableName = tableName;
							pk.ColumnNames.Add(dr.GetString(dr.GetOrdinal("column_name")));
							pk.Name = dr.GetString(dr.GetOrdinal("constraint_name"));
							key = pk;
							break;
						case "UNIQUE":
							UniqueKeyConstraint un = new UniqueKeyConstraint();
							un.TableName = tableName;
							un.ColumnNames.Add(dr.GetString(dr.GetOrdinal("column_name")));
							un.Name = dr.GetString(dr.GetOrdinal("constraint_name"));
							key = un;
							break;
					}

					KeyConstraint keyInList = null;
					foreach (KeyConstraint item in keys)
						if (item.Name.ToLower() == key.Name.ToLower())
							keyInList = item;

					if (keyInList != null)
						keyInList.ColumnNames.AddRange(key.ColumnNames);
					else
					{
						if (key is PrimaryKeyConstraint)
							keys.Insert(0, key);
						else
							keys.Add(key);
					}
				}
			}
			return keys;
		}

		public override List<Amplify.Data.ConstraintDefinition> GetConstraints(string tableName)
		{
			List<ConstraintDefinition> constraints = new List<ConstraintDefinition>();
			using(IDataReader dr = this.Adapter.ExecuteReader(string.Format(@"
					  
						SELECT 
							d.name as constraint_name, 
							c.name as default_column_name, 
							ci.COLUMN_DEFAULT as [default], 
							cu.column_name as check_column_name,  
							cc.check_clause AS [check] 
						FROM sysobjects t
						JOIN sysobjects d 
							ON d.parent_obj = t.id
						LEFT OUTER JOIN syscolumns c 
							ON d.parent_obj = c.id AND c.cdefault = d.id
						LEFT OUTER JOIN INFORMATION_SCHEMA.CHECK_CONSTRAINTS cc
							ON cc.constraint_name = d.name
						LEFT OUTER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE cu
							ON cu.constraint_name = d.name
						LEFT OUTER JOIN INFORMATION_SCHEMA.COLUMNS ci
							ON c.name = ci.column_name and t.name = ci.table_name
						WHERE 
							t.name = {0} AND d.xtype ='D' OR d.xtype = 'C'
						ORDER BY d.constraint_name
				", this.Adapter.QuoteString(tableName))))
			{
				while(dr.Read()) {
					ConstraintDefinition constraint = null;
					string type = dr.GetString(dr.GetOrdinal("type"));
					switch (type.ToUpper())
					{
						case "C":
							CheckConstraint ck = new CheckConstraint();
							ck.ColumnNames.Add(dr.GetString(dr.GetOrdinal("check_column_name")));
							ck.TableName = tableName;
							ck.Name = dr.GetString(dr.GetOrdinal("constraint_name"));
							ck.Value = dr.GetString(dr.GetOrdinal("check"));
							constraint = ck;
							break;

						case "D":
							DefaultConstraint df = new DefaultConstraint();
							df.ColumnNames.Add(dr.GetString(dr.GetOrdinal("default_column_name")));
							df.TableName = tableName;
							df.Name = dr.GetString(dr.GetOrdinal("constraint_name"));
							df.Value = dr.GetString(dr.GetOrdinal("default"));
							break;
					}

					constraints.Add(constraint);
				}
			}
			return constraints;
		}

		public override List<Amplify.Data.IndexDefinition> GetIndexes(string tableName)
		{
			return this.Adapter.GetIndexes(tableName);
		}

		public override List<Amplify.Data.TriggerDefinition> GetTriggers(string tableName)
		{
			throw new NotImplementedException();
		}
	}
}
