using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace PluginInstaller
{
	public class InstallerEntry : IInstallerModeCallbackSet
	{
		public void StartInvalidMode()
		{
			MessageBox.Show ("Not valid state, needs at least one parameter.");
		}

		public void StartIntermediaryMode (FileInfo updaterAssemblyPath, Version version, FileInfo flashAssemblyPath)
		{
			string appDataDir = Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData);
			string copyDestination = Path.Combine (appDataDir, "Spaceport\\" + updaterAssemblyPath.Name);

			string parentDirectory = InstallerHelper.GetFileDirectory (copyDestination);
			if (!Directory.Exists (parentDirectory))
				Directory.CreateDirectory (parentDirectory);

			updaterAssemblyPath.CopyTo (copyDestination, true);

			string args = string.Format ("\"{0}\" \"{1}\" \"{2}\"", version, flashAssemblyPath, updaterAssemblyPath);
			Process.Start (copyDestination, args);
		}

		public void StartSetupMode (FileInfo flashDevelopAssemblyPath, DirectoryInfo updateCacheDir)
		{
			var waitingPackage = InstallerHelper.GetLatestWaitingUpdate (updateCacheDir.FullName);
			if (waitingPackage == null) {
				MessageBox.Show ("Update .zip package missing in root directory, exiting"); return;
			}
			string versionStr = Path.GetFileNameWithoutExtension (new FileInfo (waitingPackage).Name);
			Version waitingVersion = new Version (versionStr);

			Application.EnableVisualStyles ();
			Application.SetCompatibleTextRenderingDefault (false);
			frmMain installerForm = new frmMain (waitingVersion, flashDevelopAssemblyPath.FullName, updateCacheDir.FullName);
			installerForm.Show();
			Application.Run ();
		}

		public void StartInstallerMode(Version version, FileInfo flashAssemblyPath, FileInfo oldUpdateAssemblyPath)
		{
			
		}
	}
}
