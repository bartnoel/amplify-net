using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Test
{
	using System.Configuration;
	using System.Reflection;
	using System.Data.SqlServerCe;

	using MbUnit.Framework;
	using Amplify.Data;
	using Amplify.Linq;

	public partial class Form1 : Form
	{
		protected Adapter Adapter { get; set;}


		public Form1()
		{
			InitializeComponent();
			Test test = new Test();

			try
			{
				MethodInfo[] methods = test.GetType().GetMethods();
				foreach (MethodInfo method in methods)
					if (method.GetCustomAttributes(typeof(TestAttribute), true).Length > 0)
						method.Invoke(test, null);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		
		}


		public class Test
		{
			public Adapter Adapter { get; set; }

			public Test()
			{
				this.Adapter = this.Adapter = Adapter.Add(ConfigurationManager.ConnectionStrings["ce_test"]); 
			}
			
			[Test]
			public void SelectAll()
			{
				Hash hash = Hash.New();
				using (IDataReader dr = this.Adapter.Select("SELECT * FROM [SELECT]"))
				{
					while (dr.Read())
					{
						hash.Add(dr.GetGuid(0).ToString(), dr.GetString(1));
					}
				}

				hash.Count.ShouldBe(5);
				hash.Values.First().ShouldBe("Value1");
			}

			[Test]
			public void SelectCount()
			{
				Hash hash = Hash.New();
				using (IDataReader dr = this.Adapter.Select("SELECT COUNT(*) FROM [SELECT]"))
				{
					while (dr.Read())
					{
						hash.Add("count", dr.GetInt32(0));
					}
				}

				hash.Count.ShouldBe(1);
				hash["count"].ShouldBe(5);
			}

			[Test] 
			public void InsertAndDelete()
			{
				try
				{
					Guid id = Guid.NewGuid();
					object id3 = this.Adapter.Insert("SELECT", new[] {"Id", "Name"},  new object[] { id, "Value6"});

					using (IDataReader dr = this.Adapter.Select("SELECT COUNT(*) FROM [SELECT]"))
					{
						dr.Read();
						dr.GetInt32(0).ShouldBe(6);
					}

					this.Adapter.Update("SELECT", new[] { "Id", "Name" }, new object[] { id, "Value7" });

					using (IDataReader dr = this.Adapter.Select("SELECT [Name] FROM [SELECT] WHERE Id = {0}", id))
					{
						dr.Read();
						dr.GetString(0).ShouldBe("Value7");
					}


					this.Adapter.Delete("SELECT", id);


					using (IDataReader dr = this.Adapter.Select("SELECT COUNT(*) FROM [SELECT]"))
					{
						dr.Read();
						dr.GetInt32(0).ShouldBe(5);
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString());
				}
			}

			[Test]
			public void GetTables()
			{
				List<string> tables	= this.Adapter.GetTableNames();
				string tableList = "";
				foreach (string table in tables)
					tableList += table + "\n";

				tableList.ShouldContain("Select");
			}

			[Test]
			public void GetColumns()
			{
				var query =	this.Adapter.GetColumns("Select");
				query.ShouldNotBeNull();
				query.Count().ShouldBe(2);

				Column column = query.First();
				column.IsNullable.ShouldBe(false);
				column.IsNumber.ShouldBe(false);
				column.IsPrimaryKey.ShouldBe(true);
				column.IsText.ShouldBe(false);
				column.Limit.ShouldBeNull();
				column.Name.ShouldBe("Id");
			}

		}
			

	}
}
