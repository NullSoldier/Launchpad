using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using PluginCore;
using PluginCore.Helpers;
using PluginCore.Managers;
using SpaceportPlugin.Properties;
using WeifenLuo.WinFormsUI.Docking;

namespace SpaceportPlugin
{
	public class PluginMain : IPlugin
	{
		private const string guid = "7b05efcc-d6e8-49c4-85b9-85ae9e22ead9";
		private const string name = "Spaceport Plugin";
		private const string help = "http://spaceport.io";
		private const string author = "Jason (Null) Spafford";
		private const string description = "A spaceport IDE plugin for Flash develop.";
		private const int apiLevel = 1;
		private Image icon;
		private object settingsObject;
		private DockContent mainPanel;

		public void Initialize()
		{
			TraceManager.AddAsync ("Starting Spaceport Plugin v0.00001");
			EventManager.AddEventHandler (this, EventType.FileSave);

			icon = Image.FromHbitmap (Resources.pluginIcon.GetHbitmap());
			mainPanel = PluginBase.MainForm.CreateDockablePanel (new MainUI(), guid, icon, DockState.Hidden);

			HookIntoMenu();
		}

		public void Dispose()
		{
			TraceManager.AddAsync ("Destroying Spaceport Plugin");
		}

		public void HandleEvent(object sender, NotifyEvent e, HandlingPriority priority)
		{
			if (e.Type == EventType.FileSave)
				TraceManager.AddAsync ("Spaceport Plugin detected file saved " + e);
		}

		private void HookIntoMenu()
		{
			var spaceportMenu = new ToolStripMenuItem ("Spaceport", icon);
			var testItem = new ToolStripMenuItem ("Make Spaceport Awesome");
			var updateSpaceport = new ToolStripButton ("Update Spaceport Plugin");
			var checkUpdates = new ToolStripMenuItem ("Check for updates automatically");
			var aboutSpaceport = new ToolStripMenuItem ("About");

			checkUpdates.Checked = true;
			checkUpdates.CheckOnClick = true;

			checkUpdates.Click += (s, a) => TraceManager.AddAsync ("Check updates changed: " + checkUpdates.Checked);
			aboutSpaceport.Click += (s, a) => Process.Start ("http://spaceport.io");
			testItem.Click += (s, a) => mainPanel.Show();

			spaceportMenu.DropDownItems.Add (testItem);
			spaceportMenu.DropDownItems.Add (new ToolStripSeparator ());
			spaceportMenu.DropDownItems.Add (updateSpaceport);
			spaceportMenu.DropDownItems.Add (checkUpdates);
			spaceportMenu.DropDownItems.Add (new ToolStripSeparator());
			spaceportMenu.DropDownItems.Add (aboutSpaceport);
			PluginBase.MainForm.MenuStrip.Items.Add (spaceportMenu);
		}

		#region Required Properties
		public int Api
		{
			get { return apiLevel; }
		}

		public string Name
		{
			get { return name; }
		}

		public string Guid
		{
			get { return guid; }
		}

		public string Help
		{
			get { return help; }
		}

		public string Author
		{
			get { return author; }
		}

		public string Description
		{
			get { return description; }
		}

		public object Settings
		{
			get { return settingsObject; }
		}
		#endregion
	}
}
