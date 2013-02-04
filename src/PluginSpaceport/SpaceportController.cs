using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using PluginCommon;
using PluginCore;
using PluginCore.Managers;
using PluginSpaceport.Helpers;
using PluginSpaceport.Properties;
using ProjectManager;
using ProjectManager.Projects;
using SpaceportUpdaterPlugin;
using log4net;

namespace PluginSpaceport
{
	public class SpaceportController
		: IDisposable
	{
		public SpaceportController (
			EventRouter events, SPWrapper sp,
			Settings settings, Version version)
		{
			this.sp = sp;
			this.settings = settings;
			this.version = version;
			this.events = events;
			this.updater = new UpdaterHook();

			Initialize();
		}

		public void Dispose()
		{
			watcher.Stop();
		}

		private Settings settings;
		private SPWrapper sp;
		private EventRouter events;
		private SpaceportMenu menu;
		private SPDeviceWatcher watcher;
		private Image icon;
		private Control form;
		private ILog logger;

		private readonly UpdaterHook updater = new UpdaterHook();
		private readonly Version version;

		private void Initialize()
		{
			//TODO: convert to Find Parent Form
			form = PluginBase.MainForm.MenuStrip.Parent.Parent;
			icon = Image.FromHbitmap (Resources.spaceportIcon.GetHbitmap ());
			logger = LogManager.GetLogger (typeof (SpaceportController));

			events.SubDataEvent (SPPluginEvents.Enabled, PluginEnabled);
			events.SubDataEvent (SPPluginEvents.Disabled, PluginDisabled);

			menu = new SpaceportMenu (PluginBase.MainForm.MenuStrip);
			menu.SelectTargets.Click += SelectTargets_Clicked;
			menu.About.Click += About_Clicked;
			menu.UpdateNow.Click += UpdateSpaceport_Clicked;
			menu.CheckUpdates.CheckedChanged += CheckUpdates_CheckChanged;

			watcher = new SPDeviceWatcher (sp);
			watcher.Start();

			// Subscribe to updater hooks for the UI
			updater.UpdateRunner.CheckUpdateStarted += (s, ev) =>
				logger.Info ("Spaceport updater runner started");
			updater.UpdateRunner.CheckUpdateStopped += (s, ev) =>
				logger.Info ("Spaceport updater runner stopped");
			updater.UpdateRunner.CheckUpdateFailed += (s, ev) =>
				logger.Error ("Failed to get update from " + ev.CheckLocation, ev.Exception);
			updater.UpdateRunner.UpdateFound += (s, ev) => {
				logger.Info ("Update found with version v" + ev.Version);
				TraceManager.AddAsync ("Update found with version v" + ev.Version);
				form.Invoke ((MethodInvoker)(() => menu.SetUpdateEnabled (true)));
			};

			if (menu.CheckUpdates.Checked)
				updater.StartUpdateRunner (version);
		}


		private void PluginEnabled (DataEvent e)
		{
			events.SubDataEvent (ProjectManagerEvents.TestProject, TestProject);
			watcher.Start();
		}

		private void PluginDisabled (DataEvent e)
		{
			events.UnsubDataEvent (ProjectManagerEvents.TestProject, TestProject);
			watcher.Stop ();
		}

		private void TestProject (DataEvent e)
		{
			var s = watcher.Active.Intersect (settings.DeviceTargets);
			var flashFound = false;

			foreach (var t in s) 
			{
				if (t.Platform == DevicePlatform.FlashPlayer) {
					flashFound = true;
					continue;
				}
				sp.RunOnTarget (t, TraceManager.AddAsync);
			}
			if (!flashFound) {
				e.Handled = true;
			}
		}

		private void SelectTargets_Clicked (object s, EventArgs ev)
		{
			var f = new frmTargets (watcher, settings);
			f.Show (form);
		}

		private void UpdateSpaceport_Clicked (object s, EventArgs e)
		{
			frmPatch patch = new frmPatch (updater);
			updater.GetUpdateInformation ();
			patch.ShowDialog (PluginBase.MainForm);
		}

		private void CheckUpdates_CheckChanged(object sender, EventArgs e)
		{
			switch (menu.CheckUpdates.Checked)
			{
				case true: updater.StartUpdateRunner (version); break;
				case false: updater.StopUpdateRunner (); break;
				default:
					throw new InvalidOperationException ();
			}
		}

		private void About_Clicked(object s, EventArgs ev)
		{
			Process.Start ("http://getneo.spaceport.io");
		}
	}
}
