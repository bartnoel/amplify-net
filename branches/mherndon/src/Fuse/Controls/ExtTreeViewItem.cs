
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


		


		protected virtual void Edit()
		{
			this.Edit(this.Text);
		}

		protected virtual void Edit(string text)
		{
			this.TextBlock.Tag = this.Text;
			TextBox box = new TextBox() { Text = text };
			box.LostFocus += new RoutedEventHandler(TextBoxLostFocus);
			box.KeyDown += new System.Windows.Input.KeyEventHandler(TextBoxKeyDown);

			this.StackPanel.Children.Clear();
			this.StackPanel.Children.Add(this.Image);
			this.StackPanel.Children.Add(box);
		}

		protected virtual void EndEdit(string text)
		{
			this.TextBlock.Text = text;
			this.StackPanel.Children.Clear();
			this.StackPanel.Children.Add(this.Image);
			this.StackPanel.Children.Add(this.TextBlock);
			this.StackPanel.Children.Add(this.StatusTextBlock);
		}

		protected virtual void RevertEdit()
		{
			this.TextBlock.Text = this.TextBlock.Tag.ToString();
		}

		protected virtual void TextBoxKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			switch (e.Key)
			{
				case System.Windows.Input.Key.Enter:
					e.Handled = true;
					string text = ((TextBox)sender).Text;
					this.BeginRename(text);
					this.EndEdit(text);
					break;
				case System.Windows.Input.Key.Escape:
					e.Handled = true;
					this.EndEdit(this.TextBlock.Tag.ToString());
					break;
			}
		}

		protected virtual void TextBoxLostFocus(object sender, RoutedEventArgs e)
		{
			e.Handled = true;
			string text = ((TextBox)sender).Text;
			this.BeginRename(text);
			this.EndEdit(text);
		}

		protected virtual void BeginRename(string text)
		{
			Thread t = new Thread(() => { this.Rename(text); });
			t.IsBackground = true;
			t.Start();	
		}

		protected virtual void Rename(string name)
		{
			if (Dispatcher.Thread != System.Threading.Thread.CurrentThread)
			{
				this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { this.EndRename(); }));
				return;
			}
			this.EndRename();
		}


		protected virtual void EndRename()
		{

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
