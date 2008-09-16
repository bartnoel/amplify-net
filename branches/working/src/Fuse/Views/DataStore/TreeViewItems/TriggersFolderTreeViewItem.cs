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

	public class TriggersFolderTreeViewItem : FolderTreeViewItem 
	{

		public TriggersFolderTreeViewItem()
			: base()
		{
			this.Text = "Triggers";
		}

		protected override void Load()
		{
			base.Load();
		}

		protected override void EndRefresh()
		{
			base.EndRefresh();
		}

	}
}
