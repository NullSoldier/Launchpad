using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Launchpad.Forms;
using UpdaterCore;
using Launchpad;
using PluginCore;
using PluginCore.Managers;
using Launchpad.Helpers;
using Launchpad.Properties;
using ProjectManager;
using ProjectManager.Projects;
using log4net;

namespace Launchpad
{
	public class SpaceportController
		: IDisposable
	{
		public SpaceportController (
			EventRouter events, Settings settings,
			DeviceWatcher watcher, Version version)
		{
			this.settings = settings;
			this.version = version;
			this.events = events;
			this.watcher = watcher;
			this.updater = new UpdaterHook (version);

			Initialize();
		}

		public void Dispose()
		{
			updater.StopUpdateRunner();
			watcher.Stop();
		}

		private readonly Settings settings;
		private readonly EventRouter events;
		private readonly DeviceWatcher watcher;
		private readonly UpdaterHook updater;
		private readonly Version version;
		private SpaceportMenu menu;
		private Image icon;
		private Control mainForm;
		private ILog logger;

		private void Initialize()
		{
			//TODO: convert to Find Parent Form
			mainForm = PluginBase.MainForm.MenuStrip.Parent.Parent;
			icon = Image.FromHbitmap (Resources.spaceportIcon.GetHbitmap ());
			logger = LogManager.GetLogger (typeof (SpaceportController));

			events.SubDataEvent (SPPluginEvents.Enabled, PluginEnabled);
			events.SubDataEvent (SPPluginEvents.Disabled, PluginDisabled);

			menu = new SpaceportMenu (PluginBase.MainForm.MenuStrip);
			menu.CheckUpdates.Checked = settings.CheckForUpdates;
			menu.SelectTargets.Click += SelectTargets_Clicked;
			menu.About.Click += About_Clicked;
			menu.SpaceportWebsite.Click += SpaceportWebsite_Clicked;
			menu.UpdateNow.Click += UpdateSpaceport_Clicked;
			menu.CheckUpdates.CheckedChanged += CheckUpdates_CheckChanged;

			// Subscribe to updater hooks for the UI
			UpdaterHookController.Attach (updater, mainForm, menu, logger);

			// Hack: We want to do this once it's visible
			mainForm.VisibleChanged += (s, a) => {
				if (menu.CheckUpdates.Checked)
					updater.StartUpdateRunner ();
			};
		}

		private void PluginEnabled (DataEvent e)
		{
			events.SubDataEvent (ProjectManagerEvents.TestProject, TestProject);
			events.SubDataEvent (ProjectManagerEvents.BuildProject, BuildProject);
			watcher.Start();
		}

		private void PluginDisabled (DataEvent e)
		{
			events.UnsubDataEvent (ProjectManagerEvents.TestProject, TestProject);
			events.UnsubDataEvent (ProjectManagerEvents.BuildProject, BuildProject);
			watcher.Stop ();
		}

		private void TestProject (DataEvent e)
		{
			var clearEvent = new DataEvent (EventType.Command, "ResultsPanel.ClearResults", null);
			var testEvent = new DataEvent (EventType.Command, SPPluginEvents.StartDeploy, null);
			EventManager.DispatchEvent (this, clearEvent);
			EventManager.DispatchEvent (this, testEvent);

			if (!settings.DeployDefault)
				e.Handled = true;
		}

		private void BuildProject (DataEvent e)
		{
			var clearEvent = new DataEvent (EventType.Command, "ResultsPanel.ClearResults", null);
			var buildEvent = new DataEvent (EventType.Command, SPPluginEvents.StartBuild, null);
			EventManager.DispatchEvent (this, clearEvent);
			EventManager.DispatchEvent (this, buildEvent);

			if (!settings.DeployDefault)
				e.Handled = true;
		}

		private void EnableAutoUpdate (bool enabled)
		{
			switch (settings.CheckForUpdates = enabled) {
				case true: updater.StartUpdateRunner (); break;
				case false: updater.StopUpdateRunner (); break;
			}
		}

		private void SelectTargets_Clicked (object s, EventArgs ev)
		{
			var f = new frmTargets (watcher, settings);
			f.Show (mainForm);
		}

		private void UpdateSpaceport_Clicked (object s, EventArgs e)
		{
			var patch = new frmPatch (updater);
			patch.ShowDialog (PluginBase.MainForm);
		}

		private void CheckUpdates_CheckChanged (object sender, EventArgs e)
		{
			EnableAutoUpdate (menu.CheckUpdates.Checked);
		}

		private void About_Clicked (object s, EventArgs ev)
		{
			Process.Start ("http://entitygames.net/index.php?v=launchpad");
		}

		private void SpaceportWebsite_Clicked (object s, EventArgs ev)
		{
			Process.Start ("http://spaceport.io");
		}

		private void onOutput (string o)
		{
			TraceManager.AddAsync ("Spaceport: " + o);
		}
	}
}
