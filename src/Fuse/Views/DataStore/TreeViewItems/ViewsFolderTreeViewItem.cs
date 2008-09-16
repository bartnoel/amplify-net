﻿//-----------------------------------------------------------------------
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

	using Amplify.Data;

	using Fuse.Controls;

	public class ViewsFolderTreeViewItem : FolderTreeViewItem
	{
		public ViewsFolderTreeViewItem()
			: base()
		{
			this.Text = "Views";
		}

	

	}
}
