﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PluginCommon
{
	public class UpdateMenu
	{
		public UpdateMenu (SpaceportMenu menu)
		{
			UpdateItem = new ToolStripButton ("Update Spaceport Plugin");
			CheckUpdatesItem = new ToolStripMenuItem ("Check for updates automatically");

			UpdateItem.Enabled = false;
			CheckUpdatesItem.Checked = true;
			CheckUpdatesItem.CheckOnClick = true;
			
			var updateIndex = menu.SpaceportItem.DropDownItems.IndexOf (menu.AboutItem) - 1;
			menu.SpaceportItem.DropDownItems.Insert (updateIndex, CheckUpdatesItem);
			menu.SpaceportItem.DropDownItems.Insert (updateIndex, UpdateItem);
			menu.SpaceportItem.DropDownItems.Insert (updateIndex, new ToolStripSeparator());
		}

		public ToolStripButton UpdateItem
		{
			get;
			private set;
		}

		public ToolStripMenuItem CheckUpdatesItem
		{
			get;
			private set;
		}

		public void SetUpdateEnabled (bool enabled)
		{
			UpdateItem.Enabled = enabled;
		}
	}
}
