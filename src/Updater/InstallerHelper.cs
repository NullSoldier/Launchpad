using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Updater
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

		public static string GetFileDirectory (string filePath)
		{
			return new FileInfo (filePath).DirectoryName;
		}
	}
}
