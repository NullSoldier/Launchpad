using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;
using InstallerCore;
using PluginCommon;
using PluginCore;
using PluginCore.Managers;
using PluginSpaceport;
using SpaceportUpdaterPlugin;
using SpaceportUpdaterPlugin.Properties;
using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;
using log4net;
using log4net.Config;

namespace PluginUpdater
{
	public class UpdaterPlugin : IPlugin
	{
		public void Initialize()
		{
			InitializeLogger();
			AppDomain.CurrentDomain.UnhandledException += onUnhandledException;

			if (!PluginHelper.TryGetLoadedPlugin (Resources.SpaceportPluginGuid, out spaceportPlugin))
			{
				logger.Error ("Primary spaceport plugin was not loaded, exiting");
				return;
			}

			controller = new UpdaterController();

			ThreadPool.QueueUserWorkItem (a => WaitForSpaceportPlugin());
		}

		private void InitializeLogger()
		{
			var xmlDocument = new XmlDocument();
			xmlDocument.LoadXml (Resources.log4net);
			XmlConfigurator.Configure (xmlDocument.DocumentElement);
		}

		private Control mainForm;
		private SpaceportPlugin spaceportPlugin;
		private SpaceportMenu spaceportMenu;
		private UpdateMenu updateMenu;
		private UpdaterController controller;
		private ILog logger = LogManager.GetLogger (typeof(UpdaterPlugin));

		private void Load()
		{
			spaceportMenu = spaceportPlugin.SpaceportMenu;
			mainForm = PluginBase.MainForm.MenuStrip.Parent.Parent; //TODO: convert to Find Parent Form
			
			HookIntoMainForm();

			controller.UpdateRunner.CheckUpdateStarted += UpdaterRunnerStarted;
			controller.UpdateRunner.CheckUpdateStopped += UpdaterRunnerStopped;
			controller.UpdateRunner.CheckUpdateFailed += UpdateRunnerFailed;
			controller.UpdateRunner.UpdateFound += UpdateFound;

			if (updateMenu.CheckUpdatesItem.Checked)
				controller.StartUpdateRunner (spaceportPlugin.Version);
		}

		private void UpdaterRunnerStarted(Object sender, EventArgs e)
		{
			logger.Info ("Spaceport updater runner started");
		}

		private void UpdaterRunnerStopped(Object sender, EventArgs e)
		{
			logger.Info ("Spaceport updater runner stopped");
		}

		private void UpdateRunnerFailed(Object sender, UpdateCheckerEventArgs e)
		{
			logger.Error ("Failed to get update from " + e.CheckLocation, e.Exception);
		}

		private void UpdateFound(object sender, UpdateCheckerEventArgs e)
		{
			logger.Info ("Update found with version v" + e.Version);
			TraceManager.AddAsync ("Update found with version v" + e.Version);
			mainForm.Invoke ((MethodInvoker)(() => updateMenu.SetUpdateEnabled (true)));
		}

		private void HookIntoMainForm()
		{
			var spaceportMenuItem = spaceportMenu.SpaceportItem;
			
			spaceportMenu = (SpaceportMenu)spaceportMenuItem.Tag;
			updateMenu = new UpdateMenu (spaceportMenu);

			updateMenu.UpdateItem.Click += UpdateSpaceport_Click;
			updateMenu.CheckUpdatesItem.CheckedChanged += CheckUpdates_CheckChanged;
			logger.Info("Spaceport updater plugin inserted into primary menu.");
		}

		private void UpdateSpaceport_Click (object sender, EventArgs e)
		{
			frmPatch patch = new frmPatch (controller);
			controller.GetUpdateInformation();
			patch.ShowDialog (PluginBase.MainForm);
		}

		private void CheckUpdates_CheckChanged (object sender, EventArgs e)
		{
			if (updateMenu.CheckUpdatesItem.Checked)
				controller.StartUpdateRunner (spaceportPlugin.Version);
			else
				controller.StopUpdateRunner();
		}

		private void WaitForSpaceportPlugin()
		{
			mainForm = PluginBase.MainForm.MenuStrip.Parent.Parent;

			logger.Info ("Waiting for Spaceport Plugin toload.");
			while (!spaceportPlugin.IsInitialized || !mainForm.IsHandleCreated)
				Thread.Sleep (1);

			mainForm.Invoke ((IEnumerableHelper.Action)Load);
		}

		private void onUnhandledException(object sender, UnhandledExceptionEventArgs ev)
		{
			Exception ex = (Exception)ev.ExceptionObject;
			LogManager.GetLogger (sender.GetType ()).Fatal (ex.Message, ex);
		}

		#region IPlugin Methods

		public void Dispose()
		{
			TraceManager.AddAsync ("Destroying Spaceport Updater Plugin");
			controller.Dispose();
		}

		public void HandleEvent(object sender, NotifyEvent e, HandlingPriority priority)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region IPlugin Properties
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
