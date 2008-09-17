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

	using Amplify.Data;

	using Fuse.Controls;

	public class ConstraintsFolderTreeViewItem : FolderTreeViewItem 
	{

		public ConstraintsFolderTreeViewItem()
			:base()
		{
			this.Text = "Constraints";
		}

		public string TableName { get; set; }

		public List<ConstraintDefinition> Constraints { get; set; }

		protected override void Refresh()
		{
			this.Constraints = this.Adapter.GetConstraints(this.TableName);

			base.Refresh();
		}

		protected override void EndRefresh()
		{
			this.Items.Clear();

			this.Constraints.ForEach(item => {
				this.Items.Add(new ConstraintTreeViewItem(item));
			});

			base.EndRefresh();
		}
	}
}
