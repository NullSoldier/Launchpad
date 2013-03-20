using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UpdaterCore;

namespace PluginInstaller
{
	public static class InstallerHelper
	{
		/// <summary>
		/// Gets the path to zip of the latest update package in the updateCacheDir folder
		/// </summary>
		public static string GetLatestWaitingUpdate (string updateCacheDir)
		{
			string latestPath = null;
			Version latestVersion = null;

			foreach (string zipPath in Directory.GetFiles (updateCacheDir, "*.zip"))
			{
				var version = new Version (Path.GetFileNameWithoutExtension (zipPath));
				if (latestVersion == null || version > latestVersion)
				{
					latestVersion = version;
					latestPath = zipPath;
				}
			}

			return latestPath;
		}

		/// <summary>
		/// 
		/// </summary>
		public static void LogInstallFiles (string filesDirectory, Action<string> LogMessage)
		{
			var installList = new InstallFileList (filesDirectory);

			LogMessage ("Preparing to install: " + installList.Count + " files.");

			foreach (InstallerFile installerFile in installList.Files)
			{
				var file = installerFile.File;
				var filesDirIndex = file.FullName.IndexOf ("files") + 5;
				var filesRelativePath = file.FullName.Substring (filesDirIndex);

				LogMessage (string.Format ("* {0} ({1})", filesRelativePath, installerFile.Version));
			}
		}

		public static string GetFileDirectory(string filePath)
		{
			return new FileInfo (filePath).DirectoryName;
		}
	}
}
