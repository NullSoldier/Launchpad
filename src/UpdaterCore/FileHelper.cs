using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using PluginCore.Helpers;

namespace UpdaterCore
{
	public static class FileHelper
	{
		public static void EnsureFileDirExists (FileInfo file)
		{
			string destDir = file.Directory.FullName;
			if (!Directory.Exists (destDir))
				Directory.CreateDirectory (destDir);
		}

		public static void EnsureFileDirExists (string filePath)
		{
			EnsureFileDirExists (new FileInfo (filePath));
		}

		public static DirectoryInfo AppendDir(this DirectoryInfo dir,
			string relativeDir)
		{
			var dirPath = Path.Combine (dir.FullName, relativeDir);
			return new DirectoryInfo (dirPath);
		}
	}
}
