using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Launchpad.Helpers;
using UpdaterCore;
using UpdaterCore.Update;
using PluginCore;
using PluginCore.Helpers;
using PluginCore.Managers;
using Updater;
using log4net;

namespace Launchpad
{
	[Serializable]
	public class UpdaterHook : IDisposable
	{
		private readonly Version currentVersion;
		private const string remoteUpdateDir = "http://entitygames.net/games/updates/";
		private const string remoteUpdateFile = "http://entitygames.net/games/updates/update";
		private const string localUpdateRelative = "Launchpad\\updatecache\\";
		private const string localInstallerRelative = "Launchpad\\updater.exe";

		public UpdaterHook (Version currentVersion)
		{
			Check.ArgNull (currentVersion, "currentVersion");
			this.currentVersion = currentVersion;
			init();
		}

		public UpdateDownloader UpdateDownloader
		{
			get;
			private set;
		}

		public UpdateExtractor UpdateExtractor
		{
			get;
			private set;
		}

		public UpdateRunner UpdateRunner
		{
			get;
			private set;
		}

		public Version SpaceportVersion
		{
			get;
			private set;
		}

		public UpdateInformation FoundUpdate
		{
			get;
			private set;
		}

		public UpdateInformation WaitingUpdate
		{
			get;
			private set;
		}

		public bool InstallOnClose
		{
			get { return installOnClose; }
			set { installOnClose = value; }
		}

		/// <summary>
		/// Starts the update installer, and attempts to close FlashDevelop
		/// </summary>
		public void RestartForUpdate()
		{
			installOnClose = true;
			if (startInstaller())
				PluginBase.MainForm.CallCommand ("Exit", String.Empty);
		}

		/// <summary>
		/// Start the update runner that checks for updates
		/// </summary>
		public void StartUpdateRunner()
		{
			UpdateRunner.Start();
		}

		/// <summary>
		/// Stop the update runner that checks for updates
		/// </summary>
		public void StopUpdateRunner()
		{
			UpdateRunner.Stop();
		}

		/// <summary>
		/// Downloads an update from the web with the specified version
		/// </summary>
		public bool DownloadUpdate (Version version)
		{
			TraceManager.AddAsync ("Preparing to download version v" + version);

			Version versionOnDisk;
			UpdateDownloader.TryGetWaitingPatchOnDisk (out versionOnDisk);
			
			// Make sure we actually need to download the update
			if (versionOnDisk == null || versionOnDisk < version)
			{
				UpdateDownloader.Download (version);
				return true;
			}

			WaitingUpdate = FoundUpdate;
			return false;
		}

		/// <summary>
		/// Extracts an update located in updatecache with the
		/// specified version to updatecache/files
		/// </summary>
		public void ExtractVersion (Version version)
		{
			UpdateExtractor.Unzip (version);
		}

		/// <summary>
		/// Downloads the latest update information to FoundUpdate,
		/// but only if FoundUpdate hasn't been set yet
		/// </summary>
		/// <returns>True if update was found</returns>
		public bool DownloadUpdateInfo()
		{
			StopUpdateRunner();
			return UpdateRunner.TryCheckOnceForUpdate();
		}

		public void Dispose()
		{
			StopUpdateRunner();
		}

		private bool installOnClose;
		private ILog logger = LogManager.GetLogger (typeof (UpdaterHook));

		private void init()
		{
			UpdateRunner = new UpdateRunner (new Uri (remoteUpdateFile), currentVersion);
			UpdateRunner.UpdateFound += (o, e) => FoundUpdate = e.UpdateInfo;

			var localUpdateDir = Path.Combine (PathHelper.DataDir, localUpdateRelative);
			UpdateExtractor = new UpdateExtractor (localUpdateDir);
			
			UpdateDownloader = new UpdateDownloader (remoteUpdateDir, localUpdateDir);
			UpdateDownloader.Finished += (o, e) => WaitingUpdate = FoundUpdate;
		}

		private bool startInstaller()
		{
			// Use URI to avoid file name formatting differences
			string installerPath = Path.Combine (
				PathHelper.DataDir,
				localInstallerRelative);

			if (!File.Exists (installerPath)) {
				logger.Error ("Failed to start updater at " + installerPath);
				MessageBox.Show ("Updater failed to start, updater is missing."
					+ Environment.NewLine
					+ installerPath,
					"Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				return false;
			}

			string args = string.Format ("\"{0}\" \"{1}\"",
				WaitingUpdate.Version,
				Application.ExecutablePath);
			logger.DebugFormat ("Starting installer at {0} with arguments {1}", installerPath, args);
			Process.Start (installerPath, args);
			return true;
		}
	}
}
