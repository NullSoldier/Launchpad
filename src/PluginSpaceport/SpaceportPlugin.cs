using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using PluginCommon;
using PluginCore;
using PluginCore.Helpers;
using PluginCore.Managers;
using PluginSpaceport.Properties;
using WeifenLuo.WinFormsUI.Docking;

namespace PluginSpaceport
{
	public class SpaceportPlugin : IPlugin
	{
		public bool IsInitialized
		{
			get { return isInitialized; }
		}

		public SpaceportMenu SpaceportMenu
		{
			get { return spaceportMenu; }
		}

		public Version Version
		{
			get { return currentVersion; }
		}

		public void Initialize()
		{
			icon = Image.FromHbitmap (Resources.spaceportIcon.GetHbitmap());
			mainPanel = PluginBase.MainForm.CreateDockablePanel (new MainUI(), guid, icon, DockState.Hidden);
			currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

			HookIntoMenu();

			TraceManager.AddAsync ("Starting Spaceport Plugin v" + currentVersion);
			EventManager.AddEventHandler (this, EventType.FileSave);

			isInitialized = true;
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

		private const string guid = "7b05efcc-d6e8-49c4-85b9-85ae9e22ead9";
		private const string name = "Spaceport Plugin";
		private const string help = "http://spaceport.io";
		private const string author = "Jason (Null) Spafford";
		private const string description = "A spaceport IDE plugin for Flash develop.";
		private const int apiLevel = 1;
		private object settingsObject = null;
		private Image icon;
		private DockContent mainPanel;
		private SpaceportMenu spaceportMenu;
		private Version currentVersion;
		private bool isInitialized;

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
