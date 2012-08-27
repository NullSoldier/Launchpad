using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using log4net;

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
			LogArguments (args);

			if (args.Length == 1) {
				var flashDevelopAssemblyPath = new FileInfo (getFlashDevelopPath());
				var updateCacheDir = new DirectoryInfo (Environment.CurrentDirectory);

				logger.Info ("Starting installer in GUI setup mode.");
				callbackSet.StartSetupMode (flashDevelopAssemblyPath, updateCacheDir);
			}

			else if (args.Length == 3) {
				var updaterAssemblyPath = new FileInfo (args[0]);
				var versionToInstall = new Version (args[1]);
				var flashAssemblyPath = new FileInfo (args[2]);

				logger.Debug ("Starting installer in intermediary mode.");
				callbackSet.StartIntermediaryMode (updaterAssemblyPath, versionToInstall, flashAssemblyPath);
			}

			else if (args.Length == 4) {
				var versionToInstall = new Version (args[1]);
				var flashAssemblyPath = new FileInfo (args[2]);
				var updaterAssemblyPath = new FileInfo (args[3]);

				logger.Info ("Starting installer in update installer mode");
				callbackSet.StartInstallerMode (versionToInstall, flashAssemblyPath, updaterAssemblyPath);
			}

			else
				callbackSet.StartInvalidMode ();
		}

		private static readonly ILog logger = LogManager.GetLogger (typeof (InstallerEntry));

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

		private static void LogArguments (string[] args)
		{
			for (int i=0; i<args.Length; i++)
				logger.Info ("Argument[" + i + "]: " + args[i]);
		}
	}
}
