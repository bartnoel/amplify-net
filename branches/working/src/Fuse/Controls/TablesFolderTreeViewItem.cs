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

	public class TablesFolderTreeViewItem : FolderTreeViewItem
	{
		public TablesFolderTreeViewItem()
			: base()
		{
			this.Text = "Tables";
		}

		

		public List<string> TableNames { get; set; }

		public Adapter Adapter { get; set; }

		protected override void Load()
		{
			this.TableNames = this.Adapter.GetTableNames();
			this.EndRefresh();
		}

		protected override void EndRefresh()
		{
			if (Dispatcher.Thread != System.Threading.Thread.CurrentThread)
			{
				this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { this.EndRefresh(); }));
				return;
			}

			this.TableNames.ForEach(item => {
				this.Items.Add(new TableTreeViewItem() { 
					Adapter = this.Adapter,
					Text = item
				});
			});

			this.StatusTextBlock.Text = "";
		}
	}
}
