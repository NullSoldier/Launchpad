using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using InstallerCore;
using PluginCommon;
using PluginCore;
using PluginCore.Helpers;
using PluginCore.Managers;
using PluginCore.Utilities;
using PluginSpaceport.Helpers;
using PluginSpaceport.Properties;
using ProjectManager;
using ProjectManager.Projects;
using ProjectManager.Projects.AS3;
using SpaceportUpdaterPlugin;
using WeifenLuo.WinFormsUI.Docking;
using log4net;
using log4net.Config;

namespace PluginSpaceport
{
	public class SpaceportPlugin : EventRouter, IPlugin
	{
		public void Initialize()
		{
			EventManager.AddEventHandler (this, EventType.Command);
			Log4NetHelper.ConfigureFromXML (Resources.log4net);
			SubDataEvent (ProjectManagerEvents.Project, ProjectChanged);
			LoadSettings ();

			//TODO: figure out what to do when this fails
			if (String.IsNullOrEmpty (settings.SpaceportInstallDir))
				settings.SpaceportInstallDir = @"C:\Program Files (x86)\Spaceport";

			//TODO: check this exists, and lock it until close
			var pushPath = Path.Combine (settings.SpaceportInstallDir,
				Resources.SpaceportPushName);

			sp = new SPWrapper (pushPath);
			logger = LogManager.GetLogger (typeof (SpaceportPlugin));
			spc = new SpaceportController (this, sp, settings, VERSION);

			AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
			{
				var ex = (Exception)args.ExceptionObject;
				logger.Fatal (ex.Message, ex);
			};
		}

		public void Dispose()
		{
			spc.Dispose();
			SaveSettings();
		}

		private SPWrapper sp;
		private SpaceportController spc;
		private ILog logger;
		private Settings settings;
		private bool isEnabled;

		private readonly Version VERSION = new Version (0, 1);
		private const string SETTINGS_PATH = @"Spaceport\Settings.fdb";

		private void ProjectChanged (DataEvent e)
		{
			var p = e.Data as AS3Project;
			if (p != null) {
				TraceManager.AddAsync ("Spaceport switching to new project at " + p.Directory);
				sp.ProjectDirectory = p.Directory;
				EnablePlugin (enabled:true);
			} else {
				EnablePlugin (enabled:false);
			}
		}

		private void EnablePlugin (bool enabled)
		{
			if (isEnabled != enabled) // Ignore duplicate statuses, review
			{
				isEnabled = enabled;
				TraceManager.AddAsync ((enabled ? "Enabling" : "Disabling")
					+ " Spaceport plugin");

				var eventType = isEnabled
					? SPPluginEvents.Enabled
					: SPPluginEvents.Disabled;
				var e = new DataEvent (EventType.Command, eventType, enabled);
				EventManager.DispatchEvent (this, e);
			}
		}

		private void LoadSettings()
		{
			settings = new Settings();
			var path = Path.Combine (PathHelper.DataDir, SETTINGS_PATH);

			if (!File.Exists (path)) {
				SaveSettings();
			} else {
				settings = (Settings) ObjectSerializer
					.Deserialize (path, settings);
			}
		}

		private void SaveSettings()
		{
			var path = Path.Combine (PathHelper.DataDir, SETTINGS_PATH);
			ObjectSerializer.Serialize (path, settings);
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
			get { return settings; }
		}
		#endregion
	}
}
