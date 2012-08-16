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
using SpaceportPlugin.Properties;
using WeifenLuo.WinFormsUI.Docking;

namespace PluginSpaceport
{
	public class SpaceportPlugin : IPlugin
	{
		public SpaceportMenu SpaceportMenu
		{
			get { return spaceportMenu; }
		}

		public Version Version
		{
			get { return currentVersion; }
		}

		public bool IsInitialized
		{
			get { return isInitialized; }
		}

		public void Initialize()
		{
			icon = Image.FromHbitmap (Resources.spaceportIcon.GetHbitmap());
			mainPanel = PluginBase.MainForm.CreateDockablePanel (new MainUI(), Guid, icon, DockState.Hidden);
			currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

			HookIntoMenu();

			TraceManager.AddAsync ("Starting Spaceport Plugin v" + currentVersion);
			EventManager.AddEventHandler (this, EventType.FileSave);

			isInitialized = true;
		}

		public void HandleEvent(object sender, NotifyEvent e, HandlingPriority priority)
		{
			if (e.Type == EventType.FileSave)
				TraceManager.AddAsync ("Spaceport Plugin detected file saved " + e);
		}

		public void Dispose()
		{
			TraceManager.AddAsync ("Destroying Spaceport Plugin");
		}

		private Image icon;
		private DockContent mainPanel;
		private SpaceportMenu spaceportMenu;
		private Version currentVersion;
		private object settingsObject = null;
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
			get { return 1; }
		}

		public string Name
		{
			get { return Resources.PluginName; }
		}

		public string Guid
		{
			get { return Resources.PluginGuid; }
		}

		public string Help
		{
			get { return Resources.PluginHelp; }
		}

		public string Author
		{
			get { return Resources.PluginAuthor; }
		}

		public string Description
		{
			get { return Resources.PluginDescription; }
		}

		public object Settings
		{
			get { return settingsObject; }
		}
		#endregion
	}
}
