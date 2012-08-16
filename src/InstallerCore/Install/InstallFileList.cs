using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using InstallerCore;

namespace PluginInstaller
{
	public class InstallFileList
	{
		public void Load (string installDir)
		{
			if (!Directory.Exists (installDir))
				throw new FileNotFoundException ("Files directory not found at " + installDir);

			BuildFileList (installDir);
		}

		public ReadOnlyCollection<InstallerFile> Files
		{
			get { return readOnlyFiles; }
		}

		public int Count
		{
			get { return files.Count; }
		}

		private ReadOnlyCollection<InstallerFile> readOnlyFiles; 
		private List<InstallerFile> files;

		private void BuildFileList (string installManifestDir)
		{
			DirectoryInfo filesRoot = new DirectoryInfo (installManifestDir);
			files = new List<InstallerFile> ();

			foreach (FileInfo file in filesRoot.GetFiles("*", SearchOption.AllDirectories))
			{
				string version = FileVersionInfo.GetVersionInfo (file.FullName).FileVersion;
				var fileDir = new DirectoryInfo (file.FullName).Parent;

				if (version == null)
					version = "0.0.0";

				files.Add (new InstallerFile (fileDir, file, version));
			}

			readOnlyFiles = new ReadOnlyCollection<InstallerFile> (files);
		}
	}
}
