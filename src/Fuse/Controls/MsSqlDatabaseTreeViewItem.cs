using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuse.Controls
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Windows;
	using System.Windows.Controls;
	using System.Media;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;

	using Amplify.Data;

	public class MsSqlDatabaseTreeViewItem : DataStoreTreeViewItem 
	{
		private Adapter adapter;

		public MsSqlDatabaseTreeViewItem() : base()
		{

			this.TablesFolder = new TablesFolderTreeViewItem();
			this.ViewsFolder = new ViewsFolderTreeViewItem();

			this.Items.Add(this.TablesFolder);
			this.Items.Add(this.ViewsFolder);
		}

		public override Adapter Adapter
		{
			get { return this.adapter; }
			set {
				this.adapter = value;
				this.TablesFolder.Adapter = value;
				this.ViewsFolder.Adapter = value;
			}
		}

		protected TablesFolderTreeViewItem TablesFolder { get; set; }

		protected ViewsFolderTreeViewItem ViewsFolder { get; set; }

	

	}
}
