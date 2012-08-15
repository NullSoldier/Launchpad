using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;
using System.Threading;
using PluginCommon;
using PluginCore;
using PluginCore.Managers;
using PluginSpaceport;
using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;

namespace PluginUpdater
{
	public class UpdaterPlugin : IPlugin
	{
		private const string guid = "f9319e74-26a8-4d85-a91d-17d05a5a8846";
		private const string name = "Spaceport Updater Plugin";
		private const string help = "http://spaceport.io";
		private const string author = "Jason (Null) Spafford";
		private const string description = "A spaceport IDE plugin to check for, and update the Spaceport plugin.";
		private const int apiLevel = 1;
		private const string spaceportPluginGuid = "7b05efcc-d6e8-49c4-85b9-85ae9e22ead9";
		private object settingsObject = null;
		private SpaceportPlugin spaceportPlugin;
		private SpaceportMenu spaceportMenu;
		private UpdateMenu updateMenu;

		public void Initialize()
		{
			spaceportPlugin = PluginHelper.CheckPluginLoaded<SpaceportPlugin> (spaceportPluginGuid);
			if (spaceportPlugin == null)
				throw new InvalidOperationException ("The primary spaceport plugin was not loaded.");

			ThreadPool.QueueUserWorkItem ((a) => WaitForSpaceportPlugin());
		}

		private void WaitForSpaceportPlugin ()
		{
			var form = PluginBase.MainForm.MenuStrip.Parent.Parent;

			TraceManager.AddAsync ("Waiting for Spaceport plugin to start.");
			while (!spaceportPlugin.IsInitialized || !form.IsHandleCreated )
				Thread.Sleep (1);

			PluginBase.MainForm.MenuStrip.Parent.Parent.Invoke (new MethodInvoker (Load));
		}

		public void Dispose()
		{
			TraceManager.AddAsync ("Destroying Spaceport Updater Plugin");
		}

		public void HandleEvent(object sender, NotifyEvent e, HandlingPriority priority)
		{
			throw new NotImplementedException();
		}

		private void Load()
		{
			TraceManager.AddAsync ("Starting Spaceport Update Plugin v0.1");

			spaceportMenu = spaceportPlugin.SpaceportMenu;
			HookIntoMenu();
		}

		private void HookIntoMenu()
		{
			var spaceportMenuItem = spaceportMenu.SpaceportItem;
			
			spaceportMenu = (SpaceportMenu)spaceportMenuItem.Tag;
			updateMenu = new UpdateMenu (spaceportMenu);

			updateMenu.UpdateItem.Click += (s, e) => TraceManager.Add ("Update plugin clicked.");
			
			TraceManager.Add ("Spaceport updater plugin inserted into primary menu.");
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
