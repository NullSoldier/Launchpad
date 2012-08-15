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
			SpaceportItem = new ToolStripMenuItem ("Spaceport");
			this.MakeAwesomeItem = new ToolStripMenuItem ("Make Spaceport Awesome");
			this.AboutItem = new ToolStripMenuItem ("About");

			SpaceportItem.DropDownItems.Add (MakeAwesomeItem);
			SpaceportItem.DropDownItems.Add (new ToolStripSeparator ());
			SpaceportItem.DropDownItems.Add (AboutItem);
			menuStripContainer.Items.Add (SpaceportItem);

			SpaceportItem.Tag = this;
		}

		public ToolStripMenuItem SpaceportItem
		{
			get;
			private set;
		}

		public ToolStripItem MakeAwesomeItem
		{
			get;
			private set;
		}

		public ToolStripMenuItem AboutItem
		{
			get;
			private set;
		}
	}
}
