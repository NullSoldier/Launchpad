using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UpdaterCore
{
	public class InstallerFile
	{
		public InstallerFile(DirectoryInfo dir, FileInfo file)
		{
			Directory = dir;
			File = file;
		}

		public readonly DirectoryInfo Directory;
		public readonly FileInfo File;
	}
}
