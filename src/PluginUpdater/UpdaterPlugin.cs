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
		public void Initialize()
		{
			spaceportPlugin = PluginHelper.CheckPluginLoaded<SpaceportPlugin> (spaceportPluginGuid);
			spaceportPluginVersion = spaceportPlugin.Version;
			currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
			MainForm = PluginBase.MainForm.MenuStrip.Parent.Parent; //TODO: convert to Find Parent Form
			
			ThreadPool.QueueUserWorkItem ((a) => WaitForSpaceportPlugin());
		}

		private const string spaceportPluginGuid = "7b05efcc-d6e8-49c4-85b9-85ae9e22ead9";
		private const string guid = "f9319e74-26a8-4d85-a91d-17d05a5a8846";
		private const string name = "Spaceport Updater Plugin";
		private const string help = "http://spaceport.io";
		private const string author = "Jason (Null) Spafford";
		private const string description = "A spaceport IDE plugin to check for, and update the Spaceport plugin.";
		private const int apiLevel = 1;
		private const string updateURL = "http://entitygames.net/games/updates/update";
		private object settingsObject = null;
		
		private Control MainForm;
		private SpaceportPlugin spaceportPlugin;
		private SpaceportMenu spaceportMenu;
		private UpdateMenu updateMenu;
		private UpdateRunner updateRunner;

		private Version spaceportPluginVersion;
		private Version currentVersion;
		private Version foundVersion;

		private void Load()
		{
			TraceManager.AddAsync ("Starting Spaceport Update Plugin v" + currentVersion);
			spaceportMenu = spaceportPlugin.SpaceportMenu;
			HookIntoMainForm();

			updateRunner = new UpdateRunner (new Uri (updateURL), spaceportPluginVersion);
			updateRunner.CheckUpdateStarted += (s, a) => TraceManager.AddAsync ("Spaceport updater runner started"); 
			updateRunner.CheckUpdateStopped += (s, a) => TraceManager.AddAsync ("Spaceport updater runner stopped");
			updateRunner.CheckUpdateFailed += (s, a) => TraceManager.AddAsync (String.Format ("Spaceport failed to get update from {0}: {1}",
				updateURL, ((Exception)a.ExceptionObject).Message));
			updateRunner.UpdateFound += (s, a) => {
				foundVersion = a.Version;
				MainForm.Invoke (new MethodInvoker (UpdateFound));
			};

			if (updateMenu.CheckUpdatesItem.Checked)
				updateRunner.Start();
		}

		private void UpdateFound()
		{
			TraceManager.AddAsync ("Update found with version v" + foundVersion);
			updateMenu.SetUpdateEnabled (true);
		}

		private void DownloadUpdate()
		{
		}

		private void HookIntoMainForm()
		{
			var spaceportMenuItem = spaceportMenu.SpaceportItem;
			
			spaceportMenu = (SpaceportMenu)spaceportMenuItem.Tag;
			updateMenu = new UpdateMenu (spaceportMenu);

			updateMenu.UpdateItem.Click += (s, e) => DownloadUpdate();
			updateMenu.CheckUpdatesItem.CheckedChanged += (s, e) => {
				if (updateMenu.CheckUpdatesItem.Checked) updateRunner.Start();
				else updateRunner.Stop();
			};
			TraceManager.Add ("Spaceport updater plugin inserted into primary menu.");
		}

		#region IPlugin Methods
		private void WaitForSpaceportPlugin ()
		{
			var form = PluginBase.MainForm.MenuStrip.Parent.Parent;

			TraceManager.AddAsync ("Waiting for Spaceport plugin to start.");
			while (!spaceportPlugin.IsInitialized || !form.IsHandleCreated )
				Thread.Sleep (1);

			MainForm.Invoke (new MethodInvoker (Load));
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
		#endregion

		#region IPlugin Properties
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
