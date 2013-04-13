using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UpdaterCore
{
	public class InstallerFile
	{
		public InstallerFile(DirectoryInfo dir, FileInfo file, Version version)
		{
			this.Directory = dir;
			this.File = file;
			this.Version = version;
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
