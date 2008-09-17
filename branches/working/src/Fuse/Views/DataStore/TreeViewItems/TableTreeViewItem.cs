//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Fuse.Views.DataStore.TreeViewItems
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
	using System.Windows.Threading;

	using Fuse.Models;
	using Fuse.Controls;

	public class TableTreeViewItem : ImageTreeViewItem 
	{
		private Adapter adapter;


		public TableTreeViewItem(string tableName)
			:base()
		{
			BitmapSource source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
				Properties.Resources.table.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

			this.Image.Height = 12;
			this.Image.Width = 12;
			this.Image.Source = source;

			this.ColumnsFolder = new ColumnsFolderTreeViewItem() { TableName = tableName };
			this.KeysFolder = new KeysFolderTreeViewItem() { TableName = tableName };
			this.ConstraintsFolder = new ConstraintsFolderTreeViewItem() { TableName = tableName };
			this.TriggersFolder = new TriggersFolderTreeViewItem();
			this.IndexesFolder = new IndexesFolderTreeViewItem();

			this.Items.Add(this.ColumnsFolder);
			this.Items.Add(this.KeysFolder);
			this.Items.Add(this.ConstraintsFolder);
			this.Items.Add(this.IndexesFolder);
			//this.Items.Add(this.TriggersFolder);

		}

		public Adapter Adapter
		{
			get { return this.adapter; }
			set {
				this.adapter = value;
				this.ColumnsFolder.Adapter = value;
				this.KeysFolder.Adapter = value;
				this.ConstraintsFolder.Adapter = value;
				this.IndexesFolder.Adapter = value;
			}
		}

		public ColumnsFolderTreeViewItem ColumnsFolder { get; set; }

		public KeysFolderTreeViewItem KeysFolder { get; set; }

		public ConstraintsFolderTreeViewItem ConstraintsFolder { get; set; }

		public TriggersFolderTreeViewItem TriggersFolder { get; set; }

		public IndexesFolderTreeViewItem IndexesFolder { get; set; }
	}
}
