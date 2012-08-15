using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using PluginCommon;
using PluginCore;
using PluginCore.Helpers;
using PluginCore.Managers;
using SpaceportPlugin.Properties;
using WeifenLuo.WinFormsUI.Docking;

namespace SpaceportPlugin
{
	public class SpaceportPlugin : IPlugin
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
		private SpaceportMenu spaceportMenu;

		public void Initialize()
		{
			TraceManager.AddAsync ("Starting Spaceport Plugin v0.00002");
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
			spaceportMenu = new SpaceportMenu (PluginBase.MainForm.MenuStrip);
			
			spaceportMenu.AboutItem.Click += (s, a) => Process.Start ("http://spaceport.io");
			spaceportMenu.MakeAwesomeItem.Click += (s, a) => mainPanel.Show();
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
