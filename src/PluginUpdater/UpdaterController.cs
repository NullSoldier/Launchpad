﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading;
using InstallerCore;
using InstallerCore.Update;
using PluginCore;
using PluginCore.Helpers;
using PluginCore.Managers;
using PluginSpaceport;
using PluginUpdater;

namespace SpaceportUpdaterPlugin
{
	public class UpdaterController : IDisposable
	{
		private const string remoteUpdateDir = "http://entitygames.net/games/updates/";
		private const string remoteUpdateFile = "http://entitygames.net/games/updates/update";
		private const string localUpdateRelative = "Spaceport\\updatecache\\";
		private const string localInstallerRelative = "Spaceport\\tools\\PluginInstaller.exe";

		public UpdaterController (SpaceportPlugin spaceportPlugin)
		{
			UpdaterVersion = Assembly.GetExecutingAssembly().GetName().Version;
			SpaceportVersion = spaceportPlugin.Version;

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

		public Version UpdaterVersion
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
			startInstaller();
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
		/// but only if WaitingUpdate hasn't been set yet
		/// </summary>
		public void GetUpdateInformation()
		{
			// We already have update information
			if (FoundUpdate != null)
				return;

			StopUpdateRunner();

			// don't do anything with the result because anything that
			// needs it will be listening to UpdateRunner events
			UpdateInformation foundUpdate;
			UpdateRunner.TryCheckOnceForUpdate (out foundUpdate);
		}

		public void Dispose()
		{
			StopUpdateRunner();
		}

		private bool installOnClose;

		private void init()
		{
			UpdateRunner = new UpdateRunner (new Uri (remoteUpdateFile), SpaceportVersion);
			UpdateRunner.UpdateFound += (o, e) => FoundUpdate = e.UpdateInfo;
			
			var localUpdateDir = Path.Combine (PathHelper.DataDir, localUpdateRelative);
			UpdateExtractor = new UpdateExtractor (localUpdateDir);
			
			UpdateDownloader = new UpdateDownloader (remoteUpdateDir, localUpdateDir);
			UpdateDownloader.Finished += (o, e) => WaitingUpdate = FoundUpdate;
		}

		private void startInstaller()
		{
			var flashAssemblyUri = new Uri (Assembly.GetEntryAssembly ().CodeBase);
			string installerPath = Path.Combine (PathHelper.DataDir, localInstallerRelative);

			// [0] = Version, [1] = FlashDevelop root
			string[] args = new[] { WaitingUpdate.Version.ToString(), flashAssemblyUri.LocalPath };

			Thread thread = new Thread (() =>
			{
				var domainSetup = AppDomain.CurrentDomain.SetupInformation;
				domainSetup.ShadowCopyFiles = "true";

				var domain = AppDomain.CreateDomain ("SpaceportInstallerDomain", null, domainSetup);
				domain.ExecuteAssembly (installerPath, null, args);
			});
			thread.Start();
		}
	}
}
