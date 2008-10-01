using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Fuse.Views.Layouts
{
	using System.Reflection;
	using System.IO;

	/// <summary>
	/// Interaction logic for Dashboard.xaml
	/// </summary>
	public partial class Dashboard : Window
	{
		public Dashboard()
		{
			InitializeComponent();
			this.RestoreLayout();
		}

		void OnLoaded(object sender, RoutedEventArgs e)
		{
			//throw new NotImplementedException();
		}

		

		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			MenuItem item = (MenuItem)sender;
			string command = item.Header.ToString().Replace("_", "").ToLower();

			switch (command)
			{
				case "exit":
					App.Current.Shutdown();
					break;
				case "save layout":
					this.SaveLayout();
					break;
				case "restore layout":
					this.RestoreLayout();
					break;
			}
			e.Handled = true;
		}

		private string GetLayoutPath(string name)
		{
			if(string.IsNullOrEmpty(name))
				name = "Layout";
			return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
				+ "\\Docking\\" + this.Name + "\\" + name + ".xml";
		}

		private void SaveLayout()
		{
			string path = this.GetLayoutPath(null);
			string folder = Path.GetDirectoryName(path);
			if (!Directory.Exists(folder))
				Directory.CreateDirectory(folder);

			this.Dock.SaveLayout(this.GetLayoutPath(null));
		}

		private void RestoreLayout()
		{
			string path = this.GetLayoutPath(null);
			if(File.Exists(path))
				this.Dock.RestoreLayout(path);
		}

		private void Connect_Click(object sender, RoutedEventArgs e)
		{
			
		}

		private void Migration_Click(object sender, RoutedEventArgs e)
		{
			Amplify.Data.MigrateCommand command = new Amplify.Data.MigrateCommand();
			command.Execute(new Amplify.Data.MigrationArgs() { 
				Assembly = this.GetType().Assembly,
				LowerNaming = true,
				Database = ""
			});
		}

		
	}
}
