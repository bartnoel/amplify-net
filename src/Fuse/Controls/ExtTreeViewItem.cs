
namespace Fuse.Controls
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Windows;
	using System.Windows.Controls;
	using System.Media;
	using System.Windows.Threading;
	using System.Windows.Media;
	using System.Threading;


	public class ExtTreeViewItem : ImageTreeViewItem 
	{
		public ExtTreeViewItem():base()
		{
			this.StatusTextBlock = new TextBlock();
			this.StackPanel.Children.Add(this.StatusTextBlock);
			
		}

		protected TextBlock StatusTextBlock { get; set; }
		
		protected bool IsInitialized { get; set; }

		

		protected override void OnExpanded(RoutedEventArgs e)
		{
			base.OnExpanded(e);
			if (!this.IsInitialized)
			{
				this.Refresh();
				this.IsInitialized = true;
			}
		}

		protected virtual void Refresh()
		{
			this.StartRefresh();
		}

		protected virtual void StartRefresh()
		{
			this.StatusTextBlock.Text = "(loading...)";
			Thread t = new Thread(() => Load());
			t.IsBackground = true;
			t.Start();
		}

		protected virtual void Load()
		{

			if (Dispatcher.Thread != System.Threading.Thread.CurrentThread)
			{
				this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { this.EndRefresh(); }));
				return;
			}
		}

		protected virtual void EndRefresh()
		{		
			this.StatusTextBlock.Text = "";
		}
	}
}
