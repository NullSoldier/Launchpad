using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Launchpad.Forms;
using PluginCore;
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

		private void loadMenu (ProjectMenu menuItem)
		{
			menu = new ProjectMenuEx (menuItem);
			menu.AppProperties.Click += ProjectSettings_Clicked;
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
	}
}
