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
	using System.Windows.Media.Imaging;
	using System.Windows.Threading;
	using System.Windows.Media;
	using System.Threading;

	using Amplify.Data;

	public class ColumnsFolderTreeViewItem : ExtTreeViewItem 
	{

		public ColumnsFolderTreeViewItem(Amplify.Data.ColumnDefinition columnDefinition)
		{
			
			System.Drawing.Icon icon = null;

			if (columnDefinition.IsPrimaryKey)
			{
				icon = Properties.Resources.primary_key;
			}
			else if (columnDefinition.ForeignKeys.Count > 0)
			{
				icon = Properties.Resources.foreign_key;
			}
			else
			{
				icon = Properties.Resources.column;
			}

			BitmapSource source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
				icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

			this.Image.Source = source;
		}

		public virtual Adapter Adapter { get; set; }
	}
}
