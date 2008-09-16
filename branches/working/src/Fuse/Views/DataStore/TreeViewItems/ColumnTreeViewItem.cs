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


	public class ColumnTreeViewItem : Fuse.Controls.ExtTreeViewItem 
	{

		public ColumnTreeViewItem(Amplify.Data.ColumnDefinition columnDefinition)
		{
			System.Drawing.Icon icon = null;
			this.Text += columnDefinition.Name + " (";

			if (columnDefinition.IsPrimaryKey)
			{
				icon = Properties.Resources.primary_key;
				this.Text += "PK, ";
			}
			if (columnDefinition.ForeignKeys.Count > 0)
			{
				if(icon == null)
					icon = Properties.Resources.foreign_key;
				this.Text += "FK, ";
			}
			if (columnDefinition.IsUnique)
			{
				if(icon == null)
					icon = Properties.Resources.unique;
				this.Text += "UN, ";
			}

			this.Text += columnDefinition.Type;

			if (columnDefinition.Limit.HasValue)
				this.Text += string.Format("({0})", columnDefinition.Limit.Value);

			if(columnDefinition.Precision.HasValue) {
				this.Text += string.Format("({0}", columnDefinition.Precision.Value);
				if (columnDefinition.Scale.HasValue)
					this.Text += string.Format(",{0}", columnDefinition.Scale.Value);
				this.Text += "), ";
			}

			if (columnDefinition.IsNull)
				this.Text += "null)";
			else
				this.Text += "not null)";


			if (icon == null)
			{
				switch (columnDefinition.DbType)
				{
					case Amplify.Data.DbTypes.Blob:
						icon = Properties.Resources.image_field;
						break;
					case Amplify.Data.DbTypes.Binary:
						icon = Properties.Resources.binary_field;
						break;
					case Amplify.Data.DbTypes.Boolean:
					case Amplify.Data.DbTypes.Byte:
						icon = Properties.Resources.boolean_field;
						break;
					case Amplify.Data.DbTypes.String:
					case Amplify.Data.DbTypes.Text:
					case Amplify.Data.DbTypes.AnsiString:
					case Amplify.Data.DbTypes.AnsiText:
						icon = Properties.Resources.text_field;
						break;
					case Amplify.Data.DbTypes.Date:
					case Amplify.Data.DbTypes.DateTime:
					case Amplify.Data.DbTypes.DateTime2:
					case Amplify.Data.DbTypes.DateTimeOffset:
					case Amplify.Data.DbTypes.Time:
					case Amplify.Data.DbTypes.TimeStamp:
						icon = Properties.Resources.date_field;
						break;
					case Amplify.Data.DbTypes.VarNumeric:
					case Amplify.Data.DbTypes.UInt64:
					case Amplify.Data.DbTypes.UInt32:
					case Amplify.Data.DbTypes.UInt16:
					case Amplify.Data.DbTypes.Real:
					case Amplify.Data.DbTypes.Integer:
					case Amplify.Data.DbTypes.Int64:
					case Amplify.Data.DbTypes.Int32:
					case Amplify.Data.DbTypes.Int16:
					case Amplify.Data.DbTypes.Float:
					case Amplify.Data.DbTypes.Double:
					case Amplify.Data.DbTypes.Decimal:
						icon = Properties.Resources.numeric_field;

						break;
					default:
						icon = Properties.Resources.column;

						break;
				}
			}

			this.Image.Source = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
		}
	}
}
