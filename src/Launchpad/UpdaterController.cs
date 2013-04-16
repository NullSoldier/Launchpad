using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using LaunchPad.Forms;
using LaunchPad.Helpers;
using UpdaterCore;
using UpdaterCore.Update;
using PluginCore;
using PluginCore.Helpers;
using PluginCore.Managers;
using log4net;

namespace LaunchPad
{
	public class UpdaterController
		: IDisposable
	{
		private const string REMOTE_MANIFEST_URI = "http://entitygames.net/games/updates/launchpad/update-A";
		private const string LOCAL_UPDATECACHE_DIR = "Launchpad\\updatecache\\";
		private const string LOCAL_UPDATER_FILE = "Launchpad\\updater.exe";

		public UpdaterController (
			SpaceportMenu menu, Control mainForm,
			Version pluginVersion, Settings settings)
		{
			this.menu = menu;
			this.mainForm = mainForm;
			this.settings = settings;

			var remoteManifestUri = new Uri (REMOTE_MANIFEST_URI);
			var localUpdateCacheDir = Path.Combine (PathHelper.DataDir, LOCAL_UPDATECACHE_DIR);

			UpdateChecker = new UpdateChecker (remoteManifestUri, pluginVersion);
			UpdateDownloader = new UpdateDownloader (localUpdateCacheDir);
			LoadListeners();

			if (menu.CheckUpdates.Checked)
				UpdateChecker.Start();
		}

		public readonly UpdateChecker UpdateChecker;
		public readonly UpdateDownloader UpdateDownloader;

		public void Dispose()
		{
			UpdateChecker.Stop();
		}

		public void DownloadUpdateManifest()
		{
			UpdateChecker.CheckForUpdateInfo();
		}

		public void DownloadUpdate (UpdateInformation updateInfo)
		{
			TraceManager.AddAsync ("Preparing to download Launchpad update with version v"
				+ updateInfo.Manifest.ProductVersion);
			UpdateDownloader.Download (updateInfo);
		}

		public void StartUpdating(Version version, bool closeNow)
		{
			if (StartInstaller (version) && closeNow)
				PluginBase.MainForm.CallCommand ("Exit", String.Empty);
		}

		private readonly SpaceportMenu menu;
		private readonly Control mainForm;
		private readonly Settings settings;
		private readonly ILog logger = LogManager.GetLogger (typeof (UpdaterController));

		private void LoadListeners()
		{
			menu.UpdateNow.Click += UpdateSpaceport_Clicked;
			menu.CheckUpdates.CheckedChanged += CheckUpdates_CheckChanged;

			UpdateChecker.CheckUpdateStarted += (s, e) => {
				TraceHelper.TraceInfo ("Started checking for updates automatically");
				logger.Info ("Spaceport updater runner started");
			};

			UpdateChecker.CheckUpdateStopped += (s, e) =>
				logger.Info ("Spaceport updater runner stopped");

			UpdateChecker.CheckUpdateFailed += (o, e) =>
				logger.Error ("Failed to get update from " + e.CheckLocation, e.Exception);

			UpdateChecker.UpdateFound += (s, e) => {
				UpdateChecker.Stop();
				var v = e.UpdateInfo.Manifest.ProductVersion;
				TraceManager.AddAsync ("Update found with version v" + v);
				logger.Info ("Update found with version v" + v);
			};
		}

		private void CheckUpdates_CheckChanged (object sender, EventArgs e)
		{
			settings.CheckForUpdates = menu.CheckUpdates.Checked;
			switch (settings.CheckForUpdates) {
				case true:  UpdateChecker.Start(); break;
				case false: UpdateChecker.Stop(); break;
			}
		}

		private void UpdateSpaceport_Clicked (object s, EventArgs e)
		{
			var patch = new frmPatch (this);
			patch.ShowDialog (mainForm);
		}

		private bool StartInstaller (Version version)
		{
			// Use URI to avoid file name formatting differences
			var installerPath = Path.Combine (
				PathHelper.DataDir, LOCAL_UPDATER_FILE);

			if (!File.Exists (installerPath)) {
				logger.Error ("Updater missing at " + installerPath);
				MessageBox.Show ("Updater failed to start, updater is missing."
					+ Environment.NewLine + installerPath,
					"Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				return false;
			}

			var args = string.Format ("\"{0}\" \"{1}\"",
				version,
				Application.ExecutablePath);
			logger.DebugFormat ("Starting installer at {0} with arguments {1}", installerPath, args);
			Process.Start (installerPath, args);
			return true;
		}
	}
}
