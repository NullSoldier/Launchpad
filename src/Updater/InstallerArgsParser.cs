using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UpdaterCore;
using log4net;

namespace Updater
{
	public static class InstallerArgsParser
	{
		/// <summary>
		/// [0] Self Assembly
		/// [1] Version
		/// [2] FlashDevelop Assembly to wait for
		/// [3] Old updater assembly to wait for
		/// </summary>
		public static void Parse (string[] args, IInstallerModeCallbacks callbacks)
		{
			LogArguments (args);

			if (args.Length == 1) {
				var flashDevelopAssemblyPath = new FileInfo (getFlashDevelopPath());
				var updateCacheDir = new DirectoryInfo (Environment.CurrentDirectory);

				logger.Info ("Starting installer in GUI setup mode.");
				callbacks.StartSetupMode (
					flashDevelopAssemblyPath,
					updateCacheDir);
			}
			else if (args.Length == 3) {
				var updaterAssemblyPath = new FileInfo (args[0]);
				var versionToInstall = new Version (args[1]);
				var flashAssemblyPath = new FileInfo (args[2]);

				logger.Debug ("Starting installer in intermediary mode.");
				callbacks.StartIntermediaryMode (
					updaterAssemblyPath,
					versionToInstall,
					flashAssemblyPath);
			}
			else if (args.Length == 4) {
				var versionToInstall = new Version (args[1]);
				var flashAssemblyPath = new FileInfo (args[2]);
				var updaterAssemblyPath = new FileInfo (args[3]);

				var dataDir = FDHelper.GetDataDir (flashAssemblyPath);
				var cacheDir = dataDir.AppendDir (RELATIVE_CACHE);

				logger.Info ("Starting installer in update installer mode");
				callbacks.StartInstallerMode (
					versionToInstall,
					flashAssemblyPath,
					updaterAssemblyPath,
					cacheDir);
			}
			else
				callbacks.StartInvalidMode ();
		}

		private const string DATA_SUBFOLDER = "Launchpad";
		private const string RELATIVE_CACHE = DATA_SUBFOLDER + "\\updatecache";
		private static readonly ILog logger = LogManager.GetLogger (typeof (InstallerEntry));

		private static string getFlashDevelopPath()
		{
			//TODO: get from registry
			var possiblePaths = new[] {
				@"C:\Program Files\FlashDevelop\FlashDevelop.exe",
				@"C:\Program Files (x86)\FlashDevelop\FlashDevelop.exe",
				@"C:\Users\Jason\Desktop\LaunchpadDebug\src\FlashDevelop\FlashDevelop\Bin\Debug\FlashDevelop.exe"
			};
			var path = possiblePaths.FirstOrDefault (File.Exists);
			return path ?? possiblePaths[2];
		}

		private static void LogArguments (string[] args)
		{
			for (int i=0; i<args.Length; i++)
				logger.Info ("Argument[" + i + "]: " + args[i]);
		}
	}
}
