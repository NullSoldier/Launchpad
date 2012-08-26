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
		private const string localHoodViewerRelative = @"Spaceport\tools\SpaceportHoodViewer.exe";

		public SpaceportMenu SpaceportMenu
		{
			get;
			private set;
		}

		public Version Version
		{
			get;
			private set;
		}

		public bool IsInitialized
		{
			get;
			private set;
		}

		public void Initialize()
		{
			icon = Image.FromHbitmap (Resources.spaceportIcon.GetHbitmap());
			PluginBase.MainForm.CreateDockablePanel (new MainUI(), Guid, icon, DockState.Hidden);

			HookIntoMenu();
			IsInitialized = true;
		}

		public void HandleEvent(object sender, NotifyEvent e, HandlingPriority priority)
		{
		}

		public void Dispose()
		{
			icon.Dispose();
		}

		private Image icon;
		private Process hoodViewerProcess;

		private void HookIntoMenu()
		{
			SpaceportMenu = new SpaceportMenu (PluginBase.MainForm.MenuStrip);

			SpaceportMenu.MakeAwesomeItem.Click += MakeAwesomeItem_Click;
			SpaceportMenu.AboutItem.Click += About_Click;
		}

		private void MakeAwesomeItem_Click (object sender, EventArgs e)
		{
			if (IsHoodRunning())
				return;

			try
			{
				string dataDir = Path.Combine (PathHelper.AppDir, "Data");
				string hoodViewerPath = Path.Combine (dataDir, localHoodViewerRelative);

				hoodViewerProcess = Process.Start (hoodViewerPath);
			}
			catch (FileNotFoundException ex)
			{
				MessageBox.Show ("Hood viewer failed to start, hood missing." + Environment.NewLine + ex.FileName);
			}
		}

		private void About_Click(object sender, EventArgs e)
		{
			Process.Start ("http://spaceport.io");
		}

		private bool IsHoodRunning()
		{
			return hoodViewerProcess != null && !hoodViewerProcess.HasExited;
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
			get { return null; }
		}
		#endregion
	}
}
