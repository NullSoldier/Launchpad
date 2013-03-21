using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using PluginCore;

namespace PluginCommon
{
	public class SpaceportMenu
	{
		public SpaceportMenu (MenuStrip menuStripContainer)
		{
			var root = new ToolStripMenuItem ("Spaceport");
			this.SelectTargets = new ToolStripMenuItem ("Select deploy targets");
			this.UpdateNow = new ToolStripMenuItem ("Update Spaceport Plugin");
			this.CheckUpdates = new ToolStripMenuItem ("Check for updates automatically");
			this.About = new ToolStripMenuItem ("About");

			root.DropDownItems.Add (SelectTargets);
			root.DropDownItems.Add (new ToolStripSeparator ());
			root.DropDownItems.Add (UpdateNow);
			root.DropDownItems.Add (CheckUpdates);
			root.DropDownItems.Add (new ToolStripSeparator ());
			root.DropDownItems.Add (About);

			menuStripContainer.Items.Add (root);
		}

		public readonly ToolStripItem SelectTargets;
		public readonly ToolStripItem UpdateNow;
		public readonly ToolStripMenuItem CheckUpdates;
		public readonly ToolStripItem About;
		
		public void SetUpdateEnabled(bool isEnabled)
		{
			CheckUpdates.Enabled = isEnabled;
		}
	}
}
