using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Amplify.Data;


namespace Fuse.Views.DataStore
{
	/// <summary>
	/// Interaction logic for DataStoreLogin.xaml
	/// </summary>
	public partial class DataStoreLogin : Window
	{
		public DataStoreLogin()
		{
			InitializeComponent();
		}

		public ConnectionStringAdapter ConnectionStringAdapter { get; set; }

		public String ConnectionStringName { get; set; }

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			string type = ((ComboBoxItem)this.Type.SelectedValue).Content as string;

			switch (type.ToLower())
			{
				case "ms sql":
					this.ConnectionStringAdapter = new Amplify.Data.SqlClient.SqlConnectionString();
					this.ConnectionStringAdapter["Integrated Security"] = true;
					break;
			}

			this.ConnectionStringName = this.ConnectionName.Text;
			this.ConnectionStringAdapter.Host = this.Server.Text;

			if (!string.IsNullOrEmpty(this.Password.Password))
				this.ConnectionStringAdapter.Password = this.Password.Password;

			if (!string.IsNullOrEmpty(this.Username.Text))
				this.ConnectionStringAdapter.Username = this.Username.Text;

			this.ConnectionStringAdapter.Database = this.Database.Text;
			this.Close();
		}
	}
}
