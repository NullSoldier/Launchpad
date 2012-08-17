using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PluginInstaller
{
	public static class Program
	{
		public static Version VersionToInstall = null;
		public static bool WaitForFlashDevelopClose = false;
		public static string FlashDevelopRoot = String.Empty;

		[STAThread]
		static void Main()
		{
			string[] args = Environment.GetCommandLineArgs();
			if (args.Length == 1)
			{
				WaitForFlashDevelopClose = true;
				VersionToInstall = new Version (args[0]);
				FlashDevelopRoot = args[1];
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new frmMain());
		}
	}
}
