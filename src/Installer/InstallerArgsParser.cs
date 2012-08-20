using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PluginInstaller
{
	public static class InstallerArgsParser
	{
		/// <summary>
		/// [0] Self Assembly
		/// [1] Version
		/// [2] FlashDevelop Assembly to wait for
		/// [3] Old updater assembly to wait for
		/// </summary>
		public static void Parse (string[] args, IInstallerModeCallbackSet callbackSet)
		{
			if (args.Length == 1) {
				var flashDevelopAssemblyPath = new FileInfo (getFlashDevelopPath());
				var updateCacheDir = new DirectoryInfo (Environment.CurrentDirectory);

				callbackSet.StartSetupMode (flashDevelopAssemblyPath, updateCacheDir);
			}

			else if (args.Length == 3) {
				var updaterAssemblyPath = new FileInfo (args[0]);
				var versionToInstall = new Version (args[1]);
				var flashAssemblyPath = new FileInfo (args[2]);

				callbackSet.StartIntermediaryMode (updaterAssemblyPath, versionToInstall, flashAssemblyPath);
			}

			else if (args.Length == 4) {
				var versionToInstall = new Version (args[1]);
				var flashAssemblyPath = new FileInfo (args[2]);
				var updaterAssemblyPath = new FileInfo (args[3]);

				callbackSet.StartInstallerMode (versionToInstall, flashAssemblyPath, updaterAssemblyPath);
			}

			else
				callbackSet.StartInvalidMode ();
		}

		//TODO: get from registry?
		private static string[] possibleFlashDevelopPaths = new string[] 
		{
			@"C:\Program Files\FlashDevelop\FlashDevelop.exe",
			@"C:\Program Files (x86)\FlashDevelop\FlashDevelop.exe",
			@"C:\Users\NullSoldier\Documents\Code Work Area\Projects\SpaceportPlugin\lib\FlashDevelop\FlashDevelop\Bin\Debug\FlashDevelop.exe"
		};

		private static string getFlashDevelopPath()
		{
			foreach (var path in possibleFlashDevelopPaths) {
				if (File.Exists (path))
					return path;
			}

			return possibleFlashDevelopPaths[2];
		}
	}
}
