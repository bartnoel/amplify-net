

namespace Fuse.Controls
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Windows;
	using System.Windows.Controls;
	using System.Media;
	using System.Threading;
	using System.Windows.Media;
	using System.Windows.Threading;

	public class TextTreeViewItem : TreeViewItem
	{
		private TextBox textBox;
		private StackPanel stackPanel;
		private TextBlock textBlock;

		public TextTreeViewItem()
		{
			this.StackPanel.Focusable = false;
			this.StackPanel.Orientation = Orientation.Horizontal;
			this.StackPanel.Children.Add(this.TextBlock);
			this.Header = this.StackPanel;
		}

		protected StackPanel StackPanel 
		{ 
			get {
				if (this.stackPanel == null)
					this.stackPanel = new StackPanel();
				return this.stackPanel;
			} 
		}

		protected TextBlock TextBlock
		{
			get {
				if (this.textBlock == null)
					this.textBlock = new TextBlock();
				return this.textBlock;
			}
		}

		protected TextBox TextBox 
		{
			get {
				if (this.textBox == null)
				{
					this.textBox = new TextBox();
					this.textBox.LostFocus += new RoutedEventHandler(TextBoxLostFocus);
					this.textBox.KeyDown += new System.Windows.Input.KeyEventHandler(TextBoxKeyDown);
				}
				return this.textBox;
			}
		}

		public string Text
		{
			get { return this.TextBlock.Text; }
			set { this.TextBlock.Text = value; }
		}


		protected virtual void EditText()
		{
			this.EditText(this.Text);
		}

		protected virtual void EditText(string text)
		{
			this.TextBlock.Tag = this.Text;
			
			this.StackPanel.Children.Clear();
			this.StackPanel.Children.Add(this.TextBox);
		}

		protected virtual void EndEdit(string text)
		{
			this.TextBlock.Text = text;
			this.StackPanel.Children.Clear();
			this.StackPanel.Children.Add(this.TextBlock);
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
	}
}
