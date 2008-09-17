

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


	public class TextTreeViewItem : TreeViewItem
	{

		public TextTreeViewItem()
		{
			this.StackPanel = new StackPanel();
			this.StackPanel.Focusable = false;
			this.StackPanel.Orientation = Orientation.Horizontal;
			this.TextBlock = new TextBlock();
			this.StackPanel.Children.Add(this.TextBlock);
			this.Header = this.StackPanel;
		}

		protected StackPanel StackPanel { get; set; }
		protected TextBlock TextBlock { get; set; }

		public string Text
		{
			get { return this.TextBlock.Text; }
			set { this.TextBlock.Text = value; }
		}
	}
}
