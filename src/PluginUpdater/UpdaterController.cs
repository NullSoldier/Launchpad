using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using InstallerCore;
using PluginCore.Managers;
using PluginSpaceport;
using PluginUpdater;

namespace SpaceportUpdaterPlugin
{
	public class UpdaterController
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
			updateRunner.Start();
		}

		public void StopUpdateRunner()
		{
			updateRunner.Stop();
		}

		public void DownloadUpdate()
		{
		}

		private UpdateRunner updateRunner;

		private void Init()
		{
			updateRunner = new UpdateRunner (new Uri (updateURL), SpaceportVersion);
			updateRunner.UpdateFound += (o, e) => FoundVersion = e.Version;
		}
	}
}
