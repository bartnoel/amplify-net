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
	using System.Windows.Threading; 

	using Amplify.Data;

	public class TableTreeViewItem : ImageTreeViewItem 
	{

		public TableTreeViewItem()
			:base()
		{
			BitmapSource source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
				Properties.Resources.table.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

			this.Image.Source = source;
		}

		public Adapter Adapter { get; set; }

		public ColumnsFolderTreeViewItem ColumnsFolder { get; set; }

		public KeysFolderTreeViewItem KeysFolder { get; set; }

		public ConstraintsFolderTreeViewItem ConstraintsFolder { get; set; }

		public TriggersFolderTreeViewItem TriggersFolder { get; set; }

		public IndexesFolderTreeViewItem IndexsFolder { get; set; }
	}
}
