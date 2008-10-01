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
	using System.Windows.Interop;

	using Fuse.Controls;

	using Amplify.Data;


	public class KeyTreeViewItem :ImageTreeViewItem 
	{

		public KeyTreeViewItem(KeyConstraint keyConstraint)
			: base()
		{
			System.Drawing.Icon icon = null;

			if (keyConstraint is PrimaryKeyConstraint)
				icon = Properties.Resources.primary_key;
			if (keyConstraint is ForeignKeyConstraint)
				icon = Properties.Resources.foreign_key;
			if (keyConstraint is UniqueKeyConstraint)
				icon = Properties.Resources.unique;

			this.Text = keyConstraint.Name;

			this.Image.Source = Imaging.CreateBitmapSourceFromHIcon(
				icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
		}

	}
}
