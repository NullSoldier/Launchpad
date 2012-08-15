using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace InstallerCore.Actions
{
	public abstract class BaseInstallerAction : IInstallerAction
	{
		public abstract string FileName { get; }
		public abstract InstallAction InstallType { get; }
		public abstract void Perform(string path);

		public void Load (string workingPath, string flashDataPath)
		{
			WorkPath = new FileInfo (workingPath);
			FlashDataPath = new FileInfo (flashDataPath);
		}

		protected FileInfo WorkPath;
		protected FileInfo FlashDataPath;
	}
}
