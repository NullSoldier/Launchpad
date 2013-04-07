using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Launchpad.Forms;
using PluginCore;
using PluginCore.Managers;
using ProjectManager;
using ProjectManager.Controls;
using UpdaterCore;

namespace Launchpad
{
	public class ProjectMenuController : IDisposable
	{
		public ProjectMenuController (EventRouter events, SPWrapper sp,
			Settings settings)
		{
			this.events = events;
			this.sp = sp;
			this.settings = settings;
			this.form = PluginBase.MainForm.MenuStrip.Parent.Parent;

			events.SubDataEvent (SPPluginEvents.ProjectOpened, projectOpened);
			events.SubDataEvent (SPPluginEvents.ProjectClosed, projectClosed);
			
			waitingThread = new Thread (waitForProjectMenu);
			waitingThread.Name = "Waiting for Project Menu Thread";
			waitingThread.Start();
		}

		public void Dispose()
		{
			waitingThread.Abort();
		}

		private EventRouter events;
		private SPWrapper sp;
		private Settings settings;
		private ProjectMenuEx menu;
		private Thread waitingThread;
		private Control form;

		private void projectOpened (DataEvent dataEvent)
		{
			menu.ItemsEnabled = true;
		}

		private void projectClosed (DataEvent obj)
		{
			menu.ItemsEnabled = false;
		}

		private void loadMenu (ProjectMenu menuItem)
		{
			menu = new ProjectMenuEx (menuItem);
			menu.AppProperties.Click += ProjectSettings_Clicked;
			menu.InstallProject.Click += onInstall_Clicked;
		}

		private void waitForProjectMenu()
		{
			while (true) {
				var projectMenu = findProjectMenu();
				if (projectMenu != null && form.IsHandleCreated) {
					form.Invoke ((Action)(() => loadMenu (projectMenu)));
					break;
				}
				Thread.Sleep (1);
			}
		}

		private ProjectMenu findProjectMenu()
		{
			// Darn custom collections that don't implement IEnumreable
			foreach (ToolStripItem i in PluginBase.MainForm.MenuStrip.Items) {
				if (i is ProjectMenu)
					return (ProjectMenu)i;
			}
			return null;
		}

		private void ProjectSettings_Clicked (object s, EventArgs ev)
		{
			var frmProperties = new frmProject (sp);
			frmProperties.ShowDialog (form);
		}

		private void onInstall_Clicked (object s, EventArgs e)
		{
			var installEvent = new DataEvent (EventType.Command,
				SPPluginEvents.StartInstall, null);

			EventManager.DispatchEvent (this, installEvent);
		}
	}
}
