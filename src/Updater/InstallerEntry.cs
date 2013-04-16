using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using log4net;

namespace Updater
{
	public class InstallerEntry : IInstallerModeCallbacks
	{
		public void StartInvalidMode()
		{
			MessageBox.Show ("Not valid state, needs at least one parameter.");
		}

		// No GUI, this launches ghost updater to allow self-updating
		// 1. Copies self to ghost version
		// 2. Runs that version
		// 3. Then closes this version
		public void StartIntermediaryMode (
			FileInfo updaterAssemblyPath,
			Version version,
			FileInfo flashAssemblyPath)
		{
			string appDataDir = Environment.GetFolderPath (
				Environment.SpecialFolder.LocalApplicationData);
			string copyDest = Path.Combine (appDataDir,
				"Launchpad\\" + updaterAssemblyPath.Name);

			// Make sure ghost updater destination folder exists
			string parentDirectory = InstallerHelper
				.GetFileDirectory (copyDest);
			if (!Directory.Exists (parentDirectory))
				Directory.CreateDirectory (parentDirectory);

			logger.Debug ("Copying updater to ghost updater at " + copyDest);
			try {
				updaterAssemblyPath.CopyTo (copyDest, true);
			} catch (Exception ex) {
				failToStart ("Copying updater to ghost updater failed", ex);
				return;
			}

			string args = string.Format ("\"{0}\" \"{1}\" \"{2}\"",
				version,
				flashAssemblyPath,
				updaterAssemblyPath);
			logger.DebugFormat ("Starting ghost updater with arguments {0}", args);
			Process.Start (copyDest, args);
		}

		// GUI based installer mode
		public void StartSetupMode (
			FileInfo flashDevelopAssembly,
			DirectoryInfo updateCacheDir)
		{
			var waitingPackage = InstallerHelper
				.GetLatestWaitingUpdate (updateCacheDir.FullName);
			if (waitingPackage == null) {
				failToStart ("Update .zip package missing from " 
					+ updateCacheDir.FullName);
				return;
			}
			var packageName = new FileInfo (waitingPackage).Name;
			string packageVers = Path.GetFileNameWithoutExtension (packageName);
			var version = new Version (packageVers);

			startForm (
				version,
				flashDevelopAssembly,
				updateCacheDir,
				false);
		}

		// No Gui. Ghost mode installs update packages once old UpdateAssembly closes
		public void StartInstallerMode (
			Version version, 
			FileInfo flashDevelopAssembly, 
			FileInfo oldUpdateAssemblyPath,
			DirectoryInfo updateCacheDir)
		{
			AssemblyCloseDelayer.WaitForAssembliesAsync (() => 
				startForm (
					version,
					flashDevelopAssembly,
					updateCacheDir, 
					true),
				flashDevelopAssembly.FullName,
				oldUpdateAssemblyPath.FullName);
		}

		private readonly ILog logger = LogManager.GetLogger (typeof (InstallerEntry));

		private void startForm (
			Version versionToInstall,
			FileInfo flashDevelopAssembly,
			DirectoryInfo updateCacheDir,
			bool installerMode)
		{
			Application.EnableVisualStyles ();
			Application.SetCompatibleTextRenderingDefault (false);
			frmMain installerForm = new frmMain (versionToInstall,
				flashDevelopAssembly.FullName, updateCacheDir.FullName);

			if (installerMode)
				installerForm.RunInstaller();
			else
				installerForm.RunGUIMode();

			Application.Run ();
		}

		private void failToStart (string reason, Exception ex=null)
		{
			logger.Error (reason, ex);
			MessageBox.Show (reason,
				"Update Failed",
				MessageBoxButtons.OK,
				MessageBoxIcon.Error);
			Application.Exit();
		}
	}
}
