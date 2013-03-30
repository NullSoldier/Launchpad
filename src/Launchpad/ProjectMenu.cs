using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Launchpad.Helpers;
using Launchpad.Properties;
using ProjectManager.Controls;

namespace Launchpad
{
	public class ProjectMenuEx
	{
		public ProjectMenuEx (ProjectMenu projectMenu)
		{
			Check.ArgNull (projectMenu, "projectMenu");
			AppProperties = new ToolStripMenuItem ("Spaceport App Properties");
			AppProperties.Image = Image.FromHbitmap (Resources.spaceportIcon.GetHbitmap());
			items.Add (AppProperties);

			var propertiesIndex = projectMenu.DropDownItems.IndexOf (projectMenu.Properties);
			projectMenu.DropDownItems.Insert (propertiesIndex, AppProperties);
		}

		public readonly ToolStripMenuItem AppProperties;

		public bool ItemsEnabled
		{
			set {
				foreach (var i in items)
					i.Enabled = value;
			}
		}

		private List<ToolStripItem> items = new List<ToolStripItem>();
	}
}
