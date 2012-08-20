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
		private const string localHoodViewerRelative = "Spaceport\\tools\\SpaceportHoodViewer.exe";

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

			//TraceManager.AddAsync ("Starting Spaceport Plugin v" + currentVersion);
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
			
			spaceportMenu.AboutItem.Click += About_Click;
			spaceportMenu.MakeAwesomeItem.Click += MakeAwesomeItem_Click;
		}

		private void MakeAwesomeItem_Click (object sender, EventArgs e)
		{
			if (hoodViewerProcess != null && !hoodViewerProcess.HasExited)
				return;

			string dataDir = Path.Combine (PathHelper.AppDir, "Data");
			string hoodViewerPath = Path.Combine (dataDir, localHoodViewerRelative);

			if (!File.Exists (hoodViewerPath)) {
				MessageBox.Show ("Hood viewer failed to start, hood missing." + Environment.NewLine + hoodViewerPath); return;
			}

			hoodViewerProcess = Process.Start (hoodViewerPath);
		}

		private Process hoodViewerProcess;

		private void About_Click (object sender, EventArgs e)
		{
			Process.Start ("http://spaceport.io");
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
