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

	using Fuse.Controls;

	public class ColumnsFolderTreeViewItem : FolderTreeViewItem 
	{

		public ColumnsFolderTreeViewItem()
			:base()
		{
			this.Text = "Columns";
			this.Items.Add(new TreeViewItem() { Visibility = Visibility.Hidden });
		}

		internal protected string TableName { get; set; }
		
		protected List<Amplify.Data.ColumnDefinition> Columns { get; set; }

		protected override void Refresh()
		{
			this.Columns = this.Adapter.GetColumns(this.TableName);
			base.Refresh();
		}

		protected override void EndRefresh()
		{
			this.Items.Clear();

			this.Columns.ForEach( item => {
				this.Items.Add(new ColumnTreeViewItem(item));
			});


			base.EndRefresh();
		}
	}
}
