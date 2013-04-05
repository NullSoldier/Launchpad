﻿using System;
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
			this.projectMenu = projectMenu;

			AppProperties = new ToolStripMenuItem ("Spaceport App Properties");
			AppProperties.Image = Image.FromHbitmap (Resources.spaceportIcon.GetHbitmap());

			InstallToiOS = new ToolStripMenuItem ("Install to iOS");
			InstallToAndroid = new ToolStripMenuItem ("Install To Android");

			// Expand to show iOS and Android
			var install = new ToolStripDropDownButton();
			install.Text = "Install Project";
			install.DropDownItems.Add (InstallToiOS);
			install.DropDownItems.Add (InstallToAndroid);
			
			addItemAfter (install, projectMenu.CleanProject);
			addItemBefore (AppProperties, projectMenu.Properties);
		}

		public readonly ToolStripMenuItem AppProperties;
		public readonly ToolStripMenuItem InstallToiOS;
		public readonly ToolStripMenuItem InstallToAndroid;

		public bool ItemsEnabled
		{
			set {
				foreach (var i in items)
					i.Enabled = value;
			}
		}

		private readonly ProjectMenu projectMenu;
		private List<ToolStripItem> items = new List<ToolStripItem>();

		private void addItemBefore (ToolStripItem toAdd, ToolStripMenuItem before)
		{
			addItem (toAdd, projectMenu.DropDownItems.IndexOf (before));
		}

		private void addItemAfter (ToolStripItem toAdd, ToolStripMenuItem after)
		{
			addItem (toAdd, projectMenu.DropDownItems.IndexOf (after)+1);
		}

		private void addItem (ToolStripItem toAdd, int index)
		{
			projectMenu.DropDownItems.Insert (index, toAdd);
			items.Add (toAdd);
		}
	}
}