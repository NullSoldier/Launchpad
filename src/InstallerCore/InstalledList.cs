using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using InstallerCore;

namespace PluginInstaller
{
	public class InstalledList
	{
		public void Load (string installManifestPath)
		{
			if (!File.Exists (installManifestPath))
				throw new FileNotFoundException ("Not file manifest list at " + installManifestPath);

			iniWrapper = new IniWrapper (installManifestPath);
			iniWrapper.Load();
			fileSection = iniWrapper.GetSection ("files");

			var manifestDirectory = new DirectoryInfo (installManifestPath);
			BuildFileList (manifestDirectory.Parent.FullName);
		}

		public IEnumerable<InstallerFile> Files
		{
			get { return files; }
		}

		public int Count
		{
			get { return fileSection.Count; }
		}

		private IniWrapper iniWrapper;
		private IniParser.KeyDataCollection fileSection;
		private List<InstallerFile> files;

		private void BuildFileList (string installManifestDir)
		{
			DirectoryInfo manifestDir = new DirectoryInfo (installManifestDir);
			files = new List<InstallerFile> (fileSection.Count);

			foreach (IniParser.KeyData kvp in fileSection)
			{
				string filePath = Path.Combine (manifestDir.FullName, kvp.KeyName);
				string version = FileVersionInfo.GetVersionInfo (filePath).FileVersion;

				if (version == null)
					version = "0.0.0";

				files.Add (new InstallerFile (manifestDir, new FileInfo (filePath), version));
			}
		}
	}
}
