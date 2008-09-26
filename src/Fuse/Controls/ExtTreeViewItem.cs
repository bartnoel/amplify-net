
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
				this.BeginRefresh();
				this.IsInitialized = true;
			}
		}


		



		


	

		protected virtual void BeginRefresh()
		{
			this.StatusTextBlock.Text = "(loading...)";
			Thread t = new Thread(() => { this.Refresh(); });
			t.IsBackground = true;
			t.Start();
		}

		protected virtual void Refresh()
		{
			if (Dispatcher.Thread != System.Threading.Thread.CurrentThread)
			{
				this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { this.EndRefresh(); }));
				return;
			}
			this.EndRefresh();
		}

		protected virtual void EndRefresh()
		{		
			this.StatusTextBlock.Text = "";
		}
	}
}
