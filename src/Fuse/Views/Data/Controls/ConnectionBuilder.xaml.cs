using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Fuse.Views.Data.Controls
{
	
	
	using Amplify.Data;
	using Amplify.Linq;

	/// <summary>
	/// Interaction logic for ConnectionBuilder.xaml
	/// </summary>
	public partial class ConnectionBuilder : UserControl
	{

		public ConnectionBuilder()
		{
			InitializeComponent();
			this.Server = new Models.ServerSchema();
			this.Grid.DataContext = this.Server;


			foreach (ComboBoxItem item in this.LoginType.Items)
				if (item.Content.ToString().ToLower() == this.Server.LoginType)
					item.IsSelected = true;

			foreach (ComboBoxItem item in this.ServerType.Items)
				if (item.Content.ToString().ToLower() == this.Server.Type.ToLower())
					item.IsSelected = true;

		}

		public Models.ServerSchema Server
		{
			get;
			set;
		}

		private void Connect_Click(object sender, RoutedEventArgs e)
		{

			string connectionString = this.Server.ToString();

			Adapter adapter = Adapter.Add(connectionString);
			MessageBox.Show(connectionString);


			try
			{
				string[] databases = adapter.GetDatabases();
				string output = "";
				foreach (string database in databases)
				{
					output += "{0},\n".Fuse(database);
				}
				MessageBox.Show(output.TrimEnd(",\n".ToCharArray()));
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void LoginType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(this.Server != null)
				this.Server.LoginType = ((ComboBoxItem)this.LoginType.SelectedItem).Content.ToString().ToLower(); 
		}

		private void ServerType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(this.Server != null)
				this.Server.Type = ((ComboBoxItem)this.ServerType.SelectedItem).Content.ToString().ToLower(); 
		}

		
	}
}
