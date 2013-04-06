using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Updater
{
	public interface IInstallerModeCallbacks
	{
		void StartInvalidMode();

		void StartSetupMode (
			FileInfo flashDevelopAssembly,
			DirectoryInfo updateCacheDir);

		void StartIntermediaryMode (
			FileInfo updaterAssemblyPath,
			Version version,
			FileInfo flashAssemblyPath);

		void StartInstallerMode (
			Version version,
			FileInfo flashDevelopAssembly,
			FileInfo oldUpdateAssemblyPath,
			DirectoryInfo updateCacheDir);
	}
}
