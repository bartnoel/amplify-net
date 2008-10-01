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

	public class ConstraintTreeViewItem : ImageTreeViewItem 
	{

		public ConstraintTreeViewItem(ConstraintDefinition constraint)
			:base()
		{
			System.Drawing.Icon icon = null;

			if (constraint is DefaultConstraint)
				icon = Properties.Resources.table_attribute;
			if (constraint is CheckConstraint)
				icon = Properties.Resources.table_attribute;

			this.Text = constraint.Name;

			if(icon != null)
				this.Image.Source = Imaging.CreateBitmapSourceFromHIcon(
					icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

			ContextMenu menu = new ContextMenu();
			MenuItem item = new MenuItem() { Header = "Rename" };
			item.Click +=new RoutedEventHandler(item_Click);
			menu.Items.Add(item);

			this.ContextMenu = menu;
		}

		void  item_Click(object sender, RoutedEventArgs e)
		{
			this.StackPanel.Children.Clear();
			this.StackPanel.Children.Add(this.Image);
			TextBox box = new TextBox() { Text = this.Text };
			box.LostFocus +=new RoutedEventHandler(box_LostFocus);
			this.StackPanel.Children.Add(box);

 			//this.Header = this.StackPanel;
		}

		void  box_LostFocus(object sender, RoutedEventArgs e)
		{
			TextBox box = (TextBox) sender;
			this.Text = box.Text;
			this.StackPanel.Children.Clear();
			this.StackPanel.Children.Add(this.Image);
			this.StackPanel.Children.Add(this.TextBlock);
		}

	
	}
}
