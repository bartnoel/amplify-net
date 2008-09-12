

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


	public class ImageTreeViewItem : TextTreeViewItem
	{
		private ImageSource imageSource;


		public ImageTreeViewItem()
		{
			this.StackPanel = new StackPanel();
			this.TextBlock = new TextBlock();
			this.Image = new Image();
			this.StackPanel.Children.Add(this.Image);
			this.StackPanel.Children.Add(this.TextBlock);
			this.Header = this.StackPanel;
		}

		protected Image Image { get; set; }

		public ImageSource ImageSource
		{
			get { return this.imageSource; }
			set
			{
				this.imageSource = value;
				this.Image.Source = this.ImageSource;
			}
		}

		public ImageSource ImageSourceExpanded { get; set; }

		protected override void OnExpanded(RoutedEventArgs e)
		{
			base.OnExpanded(e);
			if (this.ImageSourceExpanded != null)
				this.Image.Source = this.ImageSourceExpanded;
		}

		protected override void OnCollapsed(RoutedEventArgs e)
		{
			base.OnCollapsed(e);
			if (this.ImageSource != null)
				this.Image.Source = this.ImageSource;
		}
	}
}
