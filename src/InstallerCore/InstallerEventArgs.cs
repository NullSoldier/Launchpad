using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InstallerCore
{
	public class InstallerEventArgs : EventArgs
	{
		public InstallerEventArgs(FileInfo fileInstalledInfo)
		{
			this.FileInstalled = fileInstalledInfo;
		}

		public FileInfo FileInstalled
		{
			get;
			private set;
		}
	}
}
