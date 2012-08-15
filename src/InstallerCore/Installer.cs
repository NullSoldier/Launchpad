using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace InstallerCore
{
	public class Installer
	{
		public

		public void Start (string installRoot, string flashDevelopRoot)
		{
			string installManifestPath = Path.Combine (installRoot, installManifestName);
			IniWrapper iniWrapper = new IniWrapper (installManifestPath);

			foreach (IniParser.KeyData kvp in iniWrapper.GetSection ("Files"))
			{
				string sourcePath = Path.Combine (installRoot, kvp.KeyName);
				string destPath = Path.Combine (flashDevelopRoot, kvp.Value);

				File.Copy (sourcePath, destPath, true);
			}
			// Fire installer finished event
		}

		private const string installManifestName = "installList.ini";
	}
}
