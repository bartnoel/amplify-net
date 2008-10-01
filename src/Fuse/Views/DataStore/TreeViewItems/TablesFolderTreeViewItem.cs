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
	using System.Windows.Media.Imaging;
	using System.Windows.Threading;
	using System.Windows.Media;
	using System.Threading;

	using Amplify.Data;

	public class TablesFolderTreeViewItem : FolderTreeViewItem
	{
		public TablesFolderTreeViewItem()
			: base()
		{
			this.Text = "Tables";
		}

		

		public List<string> TableNames { get; set; }

		protected override void Refresh()
		{
			this.TableNames = this.Adapter.GetTableNames();
			base.Refresh();
		}

		protected override void EndRefresh()
		{
			this.Items.Clear();

			this.TableNames.ForEach(item => {
				this.Items.Add(new TableTreeViewItem(item) { 
					Adapter = this.Adapter,
					Text = item
				});
			});

			this.StatusTextBlock.Text = "";
		}
	}
}
