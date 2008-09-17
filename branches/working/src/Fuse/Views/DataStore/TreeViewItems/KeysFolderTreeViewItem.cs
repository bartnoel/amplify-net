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

	using Fuse.Controls;

	using Amplify.Data;

	public class KeysFolderTreeViewItem : FolderTreeViewItem 
	{

		public KeysFolderTreeViewItem()
			: base()
		{
			this.Text = "Keys";
		}

		public string TableName { get; set; }

		protected List<KeyConstraint> Keys { get; set; }

		protected override void Load()
		{
			this.Keys = this.Adapter.GetKeys(this.TableName);
			base.Load();
		}

		protected override void EndRefresh()
		{
			this.Items.Clear();

			this.Keys.ForEach(item => {
				this.Items.Add(new KeyTreeViewItem(item));
			});

			base.EndRefresh();
		}
	}
}
