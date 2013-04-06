using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UpdaterCore
{
	public static class FDHelper
	{
		public static DirectoryInfo GetDataDir (FileInfo flashDevelop)
		{
			if (!flashDevelop.Exists)
				throw new ArgumentException ();

			var dir = flashDevelop.DirectoryName;
			var local = Path.Combine (dir, ".local");

			// Standalone mode means everything must go in the FD directory
			var isStandalone = File.Exists (local);
			if (isStandalone) {
				return new DirectoryInfo (Path.Combine (dir, "Data"));
			} else {
				string userAppDir = Environment.GetFolderPath (
					Environment.SpecialFolder.LocalApplicationData);
				var dataDir = Path.Combine (userAppDir, "FlashDevelop\\Data");
				return new DirectoryInfo (dataDir);
			}
		}
	}
}
