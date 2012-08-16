using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
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
		private const string updateURL = "http://entitygames.net/games/updates/update";
		private const string localUpdateRelative = "/Spaceport/updatecache";

		public UpdaterController (SpaceportPlugin spaceportPlugin)
		{
			UpdaterVersion = Assembly.GetExecutingAssembly().GetName().Version;
			SpaceportVersion = spaceportPlugin.Version;

			Init();
		}

		public UpdateDownloader UpdateDownloader
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

		public UpdateInformation WaitingUpdate
		{
			get;
			private set;
		}

		public void StartUpdateRunner()
		{
			UpdateRunner.Start();
		}

		public void StopUpdateRunner()
		{
			UpdateRunner.Stop();
		}

		public bool DownloadUpdate()
		{
			TraceManager.AddAsync ("Preparing to download version v" + WaitingUpdate.Version);

			Version versionOnDisk;
			UpdateDownloader.TryGetWaitingPatchOnDisk (out versionOnDisk);
			
			// Make sure we actually need to download the update
			if (versionOnDisk == null || versionOnDisk < WaitingUpdate.Version)
			{
				UpdateDownloader.Download (WaitingUpdate.Version);
				return true;
			}

			return false;
		}
		public void GetUpdateInformation()
		{
			// We already have update information
			if (WaitingUpdate != null)
				return;

			StopUpdateRunner();

			// don't do anything with the result because anything that
			// needs it will be listening to UpdateRunner events
			UpdateInformation waitingUpdate;
			UpdateRunner.TryCheckOnceForUpdate (out waitingUpdate);
		}

		public void Dispose()
		{
			StopUpdateRunner();
		}

		private void Init()
		{
			UpdateRunner = new UpdateRunner (new Uri (updateURL), SpaceportVersion);
			UpdateRunner.UpdateFound += (o, e) => WaitingUpdate = e.UpdateInfo;

			UpdateDownloader = new UpdateDownloader (new Uri (updateURL), new Uri (PathHelper.DataDir + "/"));
		}
	}
}
