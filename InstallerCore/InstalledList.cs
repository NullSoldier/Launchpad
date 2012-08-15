using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using InstallerCore;

namespace PluginInstaller
{
	public class InstalledList
	{
		public void Load (string path)
		{
			iniWrapper = new IniWrapper (path);
			iniWrapper.Load();

			iniWrapper.GetSection (
		}
		
		private IniWrapper iniWrapper;
	}

	public class InstalledFile
	{
		public DirectoryInfo Directory;
		public FileInfo File;
		public string Version;
	}
}
