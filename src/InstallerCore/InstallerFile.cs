using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InstallerCore
{
	public class InstallerFile
	{
		public InstallerFile(DirectoryInfo dir, FileInfo file, string version)
		{
			this.Directory = dir;
			this.File = file;
			this.Version = new Version (version);
		}

		public DirectoryInfo Directory
		{
			get;
			private set;
		}

		public FileInfo File
		{
			get;
			private set;
		}

		public Version Version
		{
			get;
			private set;
		}
	}
}
