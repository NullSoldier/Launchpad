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
using SpaceportUpdaterPlugin;
using SpaceportUpdaterPlugin.Properties;
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

		private Control MainForm;
		private SpaceportPlugin spaceportPlugin;
		private SpaceportMenu spaceportMenu;
		private UpdateMenu updateMenu;
		private UpdaterController controller;

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
			get { return 1; }
		}

		public string Name
		{
			get { return Resources.PluginName; }
		}

		public string Guid
		{
			get { return Resources.PluginGuid; }
		}

		public string Help
		{
			get { return Resources.PluginHelp; }
		}

		public string Author
		{
			get { return Resources.PluginAuthor; }
		}

		public string Description
		{
			get { return Resources.PluginDescription; }
		}

		public object Settings
		{
			get { return null; }
		}
		#endregion
	}
}
