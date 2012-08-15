using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace InstallerCore
{
	// Install file
	// Upgrade file
	// Delete file
	public interface IInstallerAction
	{
		string FileName { get; }
		InstallAction InstallType { get; }

		void Perform (string path);
		void Load (string workingPath, string flashDataPath);
	}

	[Flags]
	public enum InstallAction
	{
		Install = 0,
		Replace = 1,
		Upgrade = 2,
		Delete = 4,
		InstallOrReplace = 8,
		UpgradeOrReplace = 16
	}
}
