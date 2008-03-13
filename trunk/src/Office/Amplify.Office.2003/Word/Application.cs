namespace Amplify.Office.v2003.Word
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Text;
	
	using Office;
	using WI = Microsoft.Office.Interop.Word;

	using Amplify.Office.Word;

	public class Application : IWordApplication
	{
		private WI.ApplicationClass application;
		private IDocumentsList documents;
		private IWindowList windows;
		private Process process;

		private event WindowActivateEventHandler windowActivateHandler;
		
		private static readonly object s_object = new object();


		public Application() : 
			this(false) { }

		public Application(bool automation)
			:this(automation, false) { }

		public Application(bool automation, bool trackProcess)
		{
			List<Process> processes = null;
			if (trackProcess)
				processes = new List<Process>(Process.GetProcessesByName("WINWORD"));
			
			this.application = new WI.ApplicationClass();
		
			this.application.Visible = !automation;
			this.application.ScreenUpdating = !automation;
			if (!automation)
				this.application.DisplayAlerts = Microsoft.Office.Interop.Word.WdAlertLevel.wdAlertsNone;
			
			if(trackProcess) 
			{
				Process[] items = Process.GetProcessesByName("WINWORD");
				foreach (Process item in processes)
					if (!processes.Contains(item))
						this.process = item;
			}
		}


		
		public event WindowActivateEventHandler WindowActivate
		{
			add {
				lock (s_object)
				{
					this.windowActivateHandler += value;
					this.application.WindowActivate += new Microsoft.Office.Interop.Word.ApplicationEvents4_WindowActivateEventHandler(application_WindowActivate);
				}
			}
			remove {
				lock (s_object)
				{
					this.windowActivateHandler -= value;
					Delegate del = (Delegate)this.windowActivateHandler;
					if (del.GetInvocationList().Length == 0)
					{
						this.application.WindowActivate -= new Microsoft.Office.Interop.Word.ApplicationEvents4_WindowActivateEventHandler(application_WindowActivate);
					}
				}
			}
		}

		void application_WindowActivate(Microsoft.Office.Interop.Word.Document Doc, Microsoft.Office.Interop.Word.Window Wn)
		{
			WindowActivateEventHandler eh = this.windowActivateHandler;
			if (eh != null)
				eh(this.Documents[Doc.Name], this.Windows[Wn.Index]);
		}

		protected internal WI.Application Interop
		{
			get { return this.application; }
		}

		public IWindowList Windows
		{
			get {
				if (this.windows == null)
					this.windows = new WindowList(this.application);
				return this.windows;
			}
		}

		public IDocumentsList Documents
		{
			get
			{ 
				if (this.documents == null)
					this.documents = new DocumentsList(application);
				return this.documents;
			}
		}

		public string Printer
		{
			get { return this.application.ActivePrinter; }
			set { this.application.ActivePrinter = value; }
		}

		public void Quit(bool saveChanges)
		{
			object save = saveChanges;
			object empty = Type.Missing;
			application.Quit(ref save, ref empty, ref empty);
		}

		public void Quit()
		{
			object empty = Type.Missing;			
			application.Quit(ref empty, ref empty, ref empty);
		}

		public void Kill()
		{
			if (this.process == null)
				throw new InvalidOperationException("You must track the process in order to use the 'Kill' method");
			this.process.Kill();
		}



		#region IDisposable Members

		public void Dispose()
		{
			if (this.documents != null)
				this.documents.Dispose();
			this.Quit();
			this.application = null;
		}

		#endregion
	}
}
