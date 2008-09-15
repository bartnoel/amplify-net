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
	using System.Media;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;

	using Amplify.Data;

	public class DataStoreTreeViewItem : ImageTreeViewItem 
	{

		public DataStoreTreeViewItem()
			: base()
		{

			BitmapSource source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
				Properties.Resources.database.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

			this.Image.Source = source;
		}

		public virtual Adapter Adapter { get; set; }
	}
}
