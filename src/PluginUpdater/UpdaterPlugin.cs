using System;
using System.Collections.Generic;
using System.Diagnostics;
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
			if (!PluginHelper.TryGetLoadedPlugin (Resources.SpaceportPluginGuid, out spaceportPlugin))
				throw new InvalidOperationException("Primary spaceport plugin was not loaded.");

			controller = new UpdaterController ();

			ThreadPool.QueueUserWorkItem (a => WaitForSpaceportPlugin());
		}

		private Control mainForm;
		private SpaceportPlugin spaceportPlugin;
		private SpaceportMenu spaceportMenu;
		private UpdateMenu updateMenu;
		private UpdaterController controller;

		private void Load()
		{
			spaceportMenu = spaceportPlugin.SpaceportMenu;
			mainForm = PluginBase.MainForm.MenuStrip.Parent.Parent; //TODO: convert to Find Parent Form
			
			HookIntoMainForm();

			controller.UpdateRunner.CheckUpdateStarted += UpdaterRunnerStarted;
			controller.UpdateRunner.CheckUpdateStopped += UpdaterRunnerStopped;
			controller.UpdateRunner.CheckUpdateFailed += UpdateRunnerFailed;
			controller.UpdateRunner.UpdateFound += UpdateFound;

			if (updateMenu.CheckUpdatesItem.Checked)
				controller.StartUpdateRunner (spaceportPlugin.Version);
		}

		private void UpdaterRunnerStarted(Object sender, EventArgs e)
		{
			TraceManager.AddAsync ("Spaceport updater runner started");
		}

		private void UpdaterRunnerStopped(Object sender, EventArgs e)
		{
			TraceManager.AddAsync ("Spaceport updater runner stopped");
		}

		private void UpdateRunnerFailed(Object sender, UpdateCheckerEventArgs e)
		{
			TraceManager.AddAsync (String.Format ("Spaceport failed to get update from {0}: {1}",
				e.CheckLocation, e.Exception.Message));
		}

		private void UpdateFound(object sender, UpdateCheckerEventArgs e)
		{
			TraceManager.AddAsync ("Update found with version v" + e.Version);
			mainForm.Invoke ((MethodInvoker)(() => updateMenu.SetUpdateEnabled (true)));
		}

		private void HookIntoMainForm()
		{
			var spaceportMenuItem = spaceportMenu.SpaceportItem;
			
			spaceportMenu = (SpaceportMenu)spaceportMenuItem.Tag;
			updateMenu = new UpdateMenu (spaceportMenu);

			updateMenu.UpdateItem.Click += UpdateSpaceport_Click;
			updateMenu.CheckUpdatesItem.CheckedChanged += CheckUpdates_CheckChanged;
			TraceManager.Add ("Spaceport updater plugin inserted into primary menu.");
		}

		private void UpdateSpaceport_Click (object sender, EventArgs e)
		{
			frmPatch patch = new frmPatch (controller);
			controller.GetUpdateInformation();
			patch.ShowDialog (PluginBase.MainForm);
		}

		private void CheckUpdates_CheckChanged (object sender, EventArgs e)
		{
			if (updateMenu.CheckUpdatesItem.Checked)
				controller.StartUpdateRunner (spaceportPlugin.Version);
			else
				controller.StopUpdateRunner();
		}

		#region IPlugin Methods
		private void WaitForSpaceportPlugin ()
		{
			mainForm = PluginBase.MainForm.MenuStrip.Parent.Parent;

			//TraceManager.AddAsync ("Waiting for Spaceport plugin to start.");
			while (!spaceportPlugin.IsInitialized || !mainForm.IsHandleCreated )
				Thread.Sleep (1);

			mainForm.Invoke (new MethodInvoker (Load));
		}

		public void Dispose()
		{
			TraceManager.AddAsync ("Destroying Spaceport Updater Plugin");
			controller.Dispose();
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
