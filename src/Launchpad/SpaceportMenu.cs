using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using PluginCore;

namespace Launchpad
{
	public class SpaceportMenu
	{
		public SpaceportMenu (MenuStrip menuStripContainer)
		{
			SelectTargets = new ToolStripMenuItem ("Select Deploy Targets");
			UpdateNow = new ToolStripMenuItem ("Update Spaceport Plugin");
			CheckUpdates = new ToolStripMenuItem ("Check for Updates Automatically");
			SpaceportWebsite = new ToolStripMenuItem ("Visit Spaceport Website");
			About = new ToolStripMenuItem ("About");

			CheckUpdates.CheckOnClick = true;

			var root = new ToolStripMenuItem ("Spaceport");
			root.DropDownItems.Add (SelectTargets);
			root.DropDownItems.Add (new ToolStripSeparator ());
			root.DropDownItems.Add (UpdateNow);
			root.DropDownItems.Add (CheckUpdates);
			root.DropDownItems.Add (new ToolStripSeparator ());
			root.DropDownItems.Add (SpaceportWebsite);
			root.DropDownItems.Add (About);

			menuStripContainer.Items.Add (root);
		}

		public readonly ToolStripItem SelectTargets;
		public readonly ToolStripItem UpdateNow;
		public readonly ToolStripMenuItem CheckUpdates;
		public readonly ToolStripItem SpaceportWebsite;
		public readonly ToolStripItem About;
		
		public void SetUpdateEnabled(bool isEnabled)
		{
			CheckUpdates.Enabled = isEnabled;
		}
	}
}
