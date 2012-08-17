using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InstallerCore
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
	}
}
