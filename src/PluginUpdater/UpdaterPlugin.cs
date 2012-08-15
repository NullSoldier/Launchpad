using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading;
using InstallerCore;
using PluginCommon;
using PluginCore;
using PluginCore.Managers;
using PluginSpaceport;
using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;

namespace PluginUpdater
{
	public class UpdaterPlugin : IPlugin
	{
		private const string spaceportPluginGuid = "7b05efcc-d6e8-49c4-85b9-85ae9e22ead9";
		private const string guid = "f9319e74-26a8-4d85-a91d-17d05a5a8846";
		private const string name = "Spaceport Updater Plugin";
		private const string help = "http://spaceport.io";
		private const string author = "Jason (Null) Spafford";
		private const string description = "A spaceport IDE plugin to check for, and update the Spaceport plugin.";
		private const int apiLevel = 1;
		private const string updateURL = "http://entitygames.net/games/updates";
		private object settingsObject = null;
		private SpaceportPlugin spaceportPlugin;
		private SpaceportMenu spaceportMenu;
		private UpdateMenu updateMenu;
		private UpdateRunner updateRunner;
		private Version spaceportPluginVersion;
		private Version currentVersion;

		public void Initialize()
		{
			spaceportPlugin = PluginHelper.CheckPluginLoaded<SpaceportPlugin> (spaceportPluginGuid);
			spaceportPluginVersion = spaceportPlugin.Version;
			currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
			
			ThreadPool.QueueUserWorkItem ((a) => WaitForSpaceportPlugin());
		}

		private void WaitForSpaceportPlugin ()
		{
			var form = PluginBase.MainForm.MenuStrip.Parent.Parent;

			TraceManager.AddAsync ("Waiting for Spaceport plugin to start.");
			while (!spaceportPlugin.IsInitialized || !form.IsHandleCreated )
				Thread.Sleep (1);

			PluginBase.MainForm.MenuStrip.Parent.Parent.Invoke (new MethodInvoker (Load));
		}

		public void Dispose()
		{
			TraceManager.AddAsync ("Destroying Spaceport Updater Plugin");
			updateRunner.Stop();
		}

		public void HandleEvent(object sender, NotifyEvent e, HandlingPriority priority)
		{
			throw new NotImplementedException();
		}

		private void Load()
		{
			TraceManager.AddAsync ("Starting Spaceport Update Plugin v" + currentVersion);
			spaceportMenu = spaceportPlugin.SpaceportMenu;
			HookIntoMenu();

			updateRunner = new UpdateRunner (new Uri (updateURL));
			updateRunner.CheckUpdateStarted += (s, a) => TraceManager.AddAsync ("Spaceport updater runner started"); 
			updateRunner.CheckUpdateStopped += (s, a) => TraceManager.AddAsync ("Spaceport updater runner stopped");
			updateRunner.UpdateFound += (s, a) => TraceManager.AddAsync ("Update found!");
			updateRunner.CheckUpdateFailed += (s, a) => TraceManager.AddAsync (String.Format ("Spaceport failed to get update from {0}: {1}",
				updateURL, ((Exception)a.ExceptionObject).Message));

			if (updateMenu.CheckUpdatesItem.Checked)
				updateRunner.Start();
		}

		private void HookIntoMenu()
		{
			var spaceportMenuItem = spaceportMenu.SpaceportItem;
			
			spaceportMenu = (SpaceportMenu)spaceportMenuItem.Tag;
			updateMenu = new UpdateMenu (spaceportMenu);

			updateMenu.UpdateItem.Click += (s, e) => TraceManager.Add ("Update plugin clicked.");
			updateMenu.CheckUpdatesItem.CheckedChanged += (s, e) => {
				if (updateMenu.CheckUpdatesItem.Checked)
					updateRunner.Start();
				else
					updateRunner.Stop();
			};
			
			TraceManager.Add ("Spaceport updater plugin inserted into primary menu.");
		}

		#region Required Properties
		public int Api
		{
			get { return apiLevel; }
		}

		public string Name
		{
			get { return name; }
		}

		public string Guid
		{
			get { return guid; }
		}

		public string Help
		{
			get { return help; }
		}

		public string Author
		{
			get { return author; }
		}

		public string Description
		{
			get { return description; }
		}

		public object Settings
		{
			get { return settingsObject; }
		}
		#endregion
	}
}
