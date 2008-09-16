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
using System.Windows.Interop;

using Fuse.Models;

namespace Fuse.Views.DataStore
{
	/// <summary>
	/// Interaction logic for DataStoreTreeView.xaml
	/// </summary>
	public partial class DataStoreTreeView : UserControl
	{
		public DataStoreTreeView()
		{
			InitializeComponent();

			BitmapSource source = Imaging.CreateBitmapSourceFromHIcon(
				Properties.Resources.database_add.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

			this.AddImage.Source = source;

			BitmapSource removeSource = Imaging.CreateBitmapSourceFromHIcon(
				Properties.Resources.database_delete.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

			this.RemoveImage.Source = removeSource;

			this.Add.Click += new RoutedEventHandler(Add_Click);
		}

		public Adapter Adapter { get; set; }

		void Add_Click(object sender, RoutedEventArgs e)
		{
			DataStoreLogin window = new DataStoreLogin();
			window.ShowDialog();

			Adapter adapter =	Adapter.Add(
				window.ConnectionStringName,
				window.ConnectionStringAdapter.Provider,
				window.ConnectionStringAdapter.ConnectionString);

			TreeViewItems.DataStoreTreeViewItem item = new TreeViewItems.MsSqlDatabaseTreeViewItem();
			item.Text = window.ConnectionStringName;
			item.Adapter = adapter;

			this.DataStoreView.Items.Add(item);
		}
	}
}
