using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace PluginInstaller
{
	public static class Program
	{
		public static Version VersionToInstall = null;
		public static bool WaitForFlashDevelopClose = false;
		public static string FlashDevelopRoot = String.Empty;
		public static string FlashDevelopAssembly = String.Empty;

		[STAThread]
		static void Main()
		{
			string[] args = Environment.GetCommandLineArgs();
			if (args.Length >= 2)
			{
				WaitForFlashDevelopClose = true;
				VersionToInstall = new Version (args[1]);
				FlashDevelopAssembly = args[2];
				FlashDevelopRoot = new FileInfo (FlashDevelopAssembly).Directory.FullName;
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			frmMain installerForm = new frmMain();
			Application.Run();
		}
	}
}
