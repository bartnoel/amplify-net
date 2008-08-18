
namespace Amplify.Tasks
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Text;

	using Microsoft.Build.Framework;
	using Microsoft.Build.Utilities;

	using Microsoft.Win32;

	/// <summary>
	/// Yui Compressor Task. 
	/// </summary>
	public class YuiCompressorTask : Task 
	{

		/// <summary>
		/// Yuis the compressor.
		/// </summary>
		public YuiCompressorTask()
		{
			this.Charset = "utf8";
			this.CompressorPath = "";
			this.Version = "2.3.6";
			this.LineBreak = -1;
			this.MergeScriptsPath = "";
			this.MergeStylesheetsPath = "";
		}

		/// <summary>
		/// Gets or sets the scripts.
		/// </summary>
		/// <value>The scripts.</value>
		public ITaskItem[] Scripts { get; set; }

		/// <summary>
		/// Gets or sets the stylesheets.
		/// </summary>
		/// <value>The stylesheets.</value>
		public ITaskItem[] Stylesheets { get; set; }

		/// <summary>
		/// Gets or sets the line break.
		/// </summary>
		/// <value>The line break.</value>
		public int LineBreak { get; set; }

		/// <summary>
		/// Gets or sets the compressor path.
		/// </summary>
		/// <value>The compressor path.</value>
		public string CompressorPath { get; set; }

		/// <summary>
		/// Gets or sets the merge scripts path.
		/// </summary>
		/// <value>The merge scripts path.</value>
		public string MergeScriptsPath { get; set; }

		/// <summary>
		/// Gets or sets the merge stylesheets path.
		/// </summary>
		/// <value>The merge stylesheets path.</value>
		public string MergeStylesheetsPath { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [show warings].
		/// </summary>
		/// <value><c>true</c> if [show warings]; otherwise, <c>false</c>.</value>
		public bool ShowWarings { get; set; }

		/// <summary>
		/// Gets or sets the charset, defaults to utf-8
		/// </summary>
		/// <value>The charset.</value>
		public string Charset { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [no munge].
		/// </summary>
		/// <value><c>true</c> if [no munge]; otherwise, <c>false</c>.</value>
		public bool NoMunge { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [preserve semi].
		/// </summary>
		/// <value><c>true</c> if [preserve semi]; otherwise, <c>false</c>.</value>
		public bool PreserveSemi { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [disable optimizations].
		/// </summary>
		/// <value><c>true</c> if [disable optimizations]; otherwise, <c>false</c>.</value>
		public bool DisableOptimizations { get; set; }

		/// <summary>
		/// Gets or sets the version, defaults to &quot;2.3.6&quot;.
		/// </summary>
		/// <value>The version, defaults to &quot;2.3.6&quot;.</value>
		public string Version { get; set; }

		/// <summary>
		/// Executes this instance.
		/// </summary>
		/// <returns></returns>
		public override bool Execute()
		{
			JavaInfo info = JavaInfo.Get();
			if (!info.Installed)
			{
				Log.LogError("Java is not installed");
				return false;
			}

			string path = info.Path + "\\bin\\java.exe";

			foreach (ITaskItem item in this.Scripts)
			{
				this.Compress(item.ItemSpec, path);
			}

			foreach (ITaskItem item in this.Stylesheets)
			{
				this.Compress(item.ItemSpec, path);
			}

			if (!string.IsNullOrEmpty(this.MergeScriptsPath))
			{
				string text = "";
				foreach (ITaskItem item in this.Scripts)
				{
					string file = item.ItemSpec, ext = Path.GetExtension(file);
					text += File.ReadAllText(file.Replace(ext, "-min" + ext)) + "\n";
				}
				File.WriteAllText(this.MergeScriptsPath, text);
			}


			if (!string.IsNullOrEmpty(this.MergeStylesheetsPath))
			{
				string text = "";
				foreach (ITaskItem item in this.Stylesheets)
				{
					string file = item.ItemSpec, ext = Path.GetExtension(file);
					text += File.ReadAllText(file.Replace(ext, "-min" + ext)) + "\n";
				}
				File.WriteAllText(this.MergeStylesheetsPath, text);
			}
			
			return true;
		}

		private void Compress(string fileToCompress, string javapath)
		{

			Process process = new Process();
			string arguments = "", ext = Path.GetExtension(fileToCompress);

			string global = (this.ShowWarings ? " --verbose " : "") + ((this.LineBreak > -1) ? " --line-break " + this.LineBreak + " ": "") ;
			string javascript = (this.NoMunge ? " --nomunge " : "") + (this.PreserveSemi ? " --preserve-semi " : "") + (this.DisableOptimizations? " --disable-optimizations " : "");

			arguments = string.Format("-jar \"{0}\" --type {1} --charset {2}  {3} -o \"{4}\" \"{5}\"",
				new object[] {
					string.Format("{0}yuicompressor-{1}.jar",this.CompressorPath, this.Version),
					ext.Replace(".",""),
					this.Charset,
					global + (ext.Contains(".js") ? javascript : ""),
					fileToCompress.Replace(ext, "-min" + ext),
					fileToCompress
				});

			process.StartInfo = new ProcessStartInfo()
			{
				FileName = javapath,
				Arguments = arguments,
				UseShellExecute = false, 
				CreateNoWindow = true,
				RedirectStandardOutput = true,
				RedirectStandardError = true 
			};

			process.Start();
			process.WaitForExit(1000);

			if (this.ShowWarings)
			{
				string[] warnings = process.StandardError.ReadToEnd()
								.Replace("\r", String.Empty)
								.Split(new string[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);

				foreach (string warning in warnings)
					Log.LogWarning(null, null, null, fileToCompress, -1, -1, -1, -1, -1, warning, null); 
			}
		}
	}
}
