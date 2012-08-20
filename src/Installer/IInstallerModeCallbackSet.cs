using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PluginInstaller
{
	public interface IInstallerModeCallbackSet
	{
		void StartInvalidMode();
		void StartSetupMode (FileInfo flashDevelopAssemblyPath, DirectoryInfo updateCacheDir);
		void StartIntermediaryMode (FileInfo updaterAssemblyPath, Version version, FileInfo flashAssemblyPath);
		void StartInstallerMode (Version version, FileInfo flashAssemblyPath, FileInfo oldUpdateAssemblyPath);
	}
}
