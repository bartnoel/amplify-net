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

using Amplify.Data;

namespace Fuse.Views.DataStore.Partial
{
	/// <summary>
	/// Interaction logic for TreeView.xaml
	/// </summary>
	public partial class TreeView : UserControl
	{
		public TreeView()
		{
			InitializeComponent();
			BitmapSource source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
				Properties.Resources.database.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

			this.AddImage.Source = source;
			this.AddImage.Width = 16;
			this.AddImage.Height = 16;

			this.Add.Click += new RoutedEventHandler(Add_Click);
		}

		public Adapter Adapter { get; set; }

		void Add_Click(object sender, RoutedEventArgs e)
		{
			DataStore.ConnectionString window = new ConnectionString();
			window.ShowDialog();

			Adapter adapter =	Adapter.Add(
				window.ConnectionStringName,
				window.ConnectionStringAdapter.Provider, 
				window.ConnectionStringAdapter.ConnectionString);

			Controls.DataStoreTreeViewItem item = new Fuse.Controls.MsSqlDatabaseTreeViewItem();
			item.Text = window.ConnectionStringName;
			item.Adapter = adapter;

			this.DataStoreView.Items.Add(item);
		}
	}
}
