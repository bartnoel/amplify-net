//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Fuse.Controls
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Media.Imaging;
	using System.Media;
	using System.Windows.Media;

	public class FolderTreeViewItem : ExtTreeViewItem
	{


		public FolderTreeViewItem()
			: base()
		{
			BitmapSource source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
				Properties.Resources.folder.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

			this.Image.Source = source;
		}
	}
}
