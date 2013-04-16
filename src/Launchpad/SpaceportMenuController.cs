using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace LaunchPad
{
	public class SpaceportMenuController
	{
		public SpaceportMenuController (
			SpaceportMenu menu, Control mainForm,
			Version version, EventRouter events,
			Settings settings, DeviceWatcher watcher)
		{
			this.mainForm = mainForm;
			this.settings = settings;
			this.watcher = watcher;

			menu.SelectTargets.Click += SelectTargets_Clicked;
			menu.About.Click += About_Clicked;
			menu.SpaceportWebsite.Click += SpaceportWebsite_Clicked;
		}

		private readonly Settings settings;
		private readonly DeviceWatcher watcher;
		private readonly Control mainForm;

		private void SelectTargets_Clicked (object s, EventArgs ev)
		{
			var f = new frmTargets (watcher, settings);
			f.Show (mainForm);
		}

		private void About_Clicked (object s, EventArgs ev)
		{
			Process.Start ("http://entitygames.net/index.php?v=launchpad");
			Launchpad.A.AboutUs();
		}

		private void SpaceportWebsite_Clicked (object s, EventArgs ev)
		{
			Process.Start ("http://spaceport.io");
			Launchpad.A.SpaceportWebsite();
		}
	}
}
