//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Tasks
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using Microsoft.Win32;

	public class JavaInfo
	{
		private static JavaInfo instance = null;

		protected JavaInfo()
		{
			this.CurrentVersion = "";
			this.Installed = false;
			this.Path = "";
		}

		public string CurrentVersion { get; set; }
		
		public bool Installed { get; set; }
		
		public string Path { get; set; }


		public static JavaInfo Get()
		{
			if (instance == null)
			{
				RegistryKey key = Registry.LocalMachine;
				JavaInfo info = new JavaInfo();

				string location = @"Software\JavaSoft\Java Runtime Environment";
				RegistryKey environment = key.OpenSubKey(location);
				if (environment != null)
				{

					info.Installed = true;
					object value = environment.GetValue("CurrentVersion");
					if (value != null)
					{
						info.CurrentVersion = value.ToString();
						RegistryKey currentVersion = environment.OpenSubKey(info.CurrentVersion);
						if (currentVersion != null)
						{
							value = currentVersion.GetValue("JavaHome");

							if (value != null)
								info.Path = value.ToString();
						}
					}
				}

				instance = info;
			}

			return instance;
		}
	}
}
