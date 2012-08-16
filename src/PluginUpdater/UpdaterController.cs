using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using InstallerCore;
using InstallerCore.Update;
using PluginCore.Managers;
using PluginSpaceport;
using PluginUpdater;

namespace SpaceportUpdaterPlugin
{
	public class UpdaterController : IDisposable
	{
		private const string updateURL = "http://entitygames.net/games/updates/update";

		public UpdaterController (SpaceportPlugin spaceportPlugin)
		{
			UpdaterVersion = Assembly.GetExecutingAssembly().GetName().Version;
			SpaceportVersion = spaceportPlugin.Version;

			Init();
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

		public Version FoundVersion
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

		public void DownloadUpdate (Version version)
		{
			StopUpdateRunner();

			if (FoundVersion == null)
			{
				Version versionFound;
				UpdateRunner.TryCheckOnceForUpdate (out versionFound);
			}
		}

		public UpdateInformation GetUpdateInformation()
		{
			if (FoundVersion == null)
			{
				Version versionFound;
				UpdateRunner.TryCheckOnceForUpdate (out versionFound);
			}
		}

		public void Dispose()
		{
			StopUpdateRunner();
		}

		private void Init()
		{
			UpdateRunner = new UpdateRunner (new Uri (updateURL), SpaceportVersion);
			UpdateRunner.UpdateFound += (o, e) => FoundVersion = e.Version;
		}
	}
}
