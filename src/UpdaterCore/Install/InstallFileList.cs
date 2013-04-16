using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using UpdaterCore;

namespace Updater
{
	public class InstallFileList
	{
		/// <summary>
		/// Takes a path to a files directory where files have been extracted
		/// Ex: FlashDevelop/Data/updatecache/files
		/// </summary>
		public static IEnumerable<InstallerFile> BuildFileList (DirectoryInfo rootDir)
		{
			if (!rootDir.Exists)
				throw new FileNotFoundException ("Install file dir doesn't exist at " + Environment.NewLine + rootDir);

			string[] files = Directory.GetFiles (rootDir.FullName,
				"*", SearchOption.AllDirectories);

			foreach (string filePath in files) {
				// Prevent lazy loading issues <-- wtf?
				if (!File.Exists (filePath))
					continue;

				yield return new InstallerFile (
					new FileInfo (filePath).Directory,
					new FileInfo (filePath));
			}
		}
	}
}
