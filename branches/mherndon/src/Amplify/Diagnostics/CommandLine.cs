//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Diagnostics
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Text;
	using System.Threading;

	public class CommandLine : IDisposable
	{
		private Process process;
		private bool hasExited = false;
		private List<string> log;
		private string path;
		public event EventHandler<CommandLineDataArgs> OutputDataRecieved;
		public event EventHandler<CommandLineDataArgs> ErrorDataRecieved;


		public CommandLine()
		{
			this.log = new List<string>(300);
			this.Initialize(
				"cmd.exe",
				System.Environment.GetEnvironmentVariable("windir") + "\\system32\\"
				);
		}

		public CommandLine(string filename)
		{
			this.log = new List<string>(300);
			this.Initialize(filename);
		}

		public CommandLine(string filename, string directory)
		{
			this.log = new List<string>(300);
			this.Initialize(filename, directory);
		}

		public string WorkingPath
		{
			get { return this.path; }
		}

		private void Initialize(string filename)
		{
			this.Initialize(
				filename,
				null
			);
		}

		private void Initialize(string filename, string directory)
		{
			this.process = new Process();
			if(!string.IsNullOrEmpty(directory))
				this.process.StartInfo.WorkingDirectory = directory;
			this.process.StartInfo.FileName = filename;

			if (filename.ToLower().IndexOf("powershell") > -1)
				this.process.StartInfo.Arguments = "-command -";

			this.process.StartInfo.CreateNoWindow = true;
			this.process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

			this.process.StartInfo.UseShellExecute = false;
			this.process.StartInfo.RedirectStandardInput = true;
			this.process.StartInfo.RedirectStandardOutput = true;
			this.process.StartInfo.RedirectStandardError = true;
			this.process.ErrorDataReceived += new DataReceivedEventHandler(ErrorDataReceivedHandler);
			this.process.OutputDataReceived += new DataReceivedEventHandler(OutputDataReceivedHandler);
			this.path = this.process.StartInfo.WorkingDirectory;
		}

		public void Start()
		{
			this.process.Start();
			this.process.BeginOutputReadLine();
			this.process.BeginErrorReadLine();
		}

		private void OnOutputDataReceived(CommandLineDataArgs e)
		{
			EventHandler<CommandLineDataArgs> eh = this.OutputDataRecieved;
			if (eh != null)
				eh(this, e);
		}

		private void OnErrorDataReceived(CommandLineDataArgs e)
		{
			EventHandler<CommandLineDataArgs> eh = this.ErrorDataRecieved;
			if (eh != null)
				eh(this, e);
		}

		private void OutputDataReceivedHandler(object sender, DataReceivedEventArgs e)
		{
			if (e.Data != null)
			{
				this.log.Add(e.Data);
				this.OnOutputDataReceived(new CommandLineDataArgs() { Data = e.Data});
			}
		}

		private void ErrorDataReceivedHandler(object sender, DataReceivedEventArgs e)
		{
			if (e.Data != null)
			{
				this.log.Add(e.Data);
				this.OnErrorDataReceived(new CommandLineDataArgs() { Data = e.Data });
			}
		}

		public int Exit(bool wait)
		{
			if (!this.hasExited)
			{
				try
				{
					
					if (wait)
					{
						this.process.StandardInput.WriteLine("exit");
						this.process.WaitForExit();
					}
					else
						this.process.Kill();
					this.process.OutputDataReceived -= new DataReceivedEventHandler(OutputDataReceivedHandler);
					this.process.ErrorDataReceived -= new DataReceivedEventHandler(ErrorDataReceivedHandler);
					if (wait)
						return this.process.ExitCode;
					return 0;
				}
				catch
				{

				}
				finally
				{
					this.process.Dispose();
					this.process = null;
					this.hasExited = true;
				}
			}
			return -1;
		}



		public void Exec(string input)
		{
			if (input.StartsWith("cd"))
			{
				string temp = input.Replace("cd", "").Replace("\"", "").Trim();
				if (System.IO.Directory.Exists(temp))
					this.path = temp;
			}
			
			this.process.StandardInput.WriteLine(input);
		}

	

		#region IDisposable Members

		public void Dispose()
		{
			this.Exit(false);
		}

		#endregion
	}
}
