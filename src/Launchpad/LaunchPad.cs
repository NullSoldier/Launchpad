using System;
using System.IO;
using System.Reflection;
using Launchpad.Helpers;
using Launchpad.Properties;
using log4net;
using PluginCore;
using PluginCore.Helpers;
using PluginCore.Managers;
using PluginCore.Utilities;
using ProjectManager;
using ProjectManager.Projects.AS3;

namespace Launchpad
{
	public class LaunchPad : EventRouter, IPlugin
	{
		public void Initialize()
		{
			AppDomain.CurrentDomain.UnhandledException += (s, e) => {
				var ex = e.ExceptionObject as Exception;
				logger.Fatal (ex.Message, ex);
			};

			EventManager.AddEventHandler (this, EventType.Command);
			Log4NetHelper.ConfigureFromXML (Resources.log4net);
			SubDataEvent (ProjectManagerEvents.Project, ProjectChanged);
			LoadSettings();

			logger = LogManager.GetLogger (typeof (LaunchPad));
			version = Assembly.GetExecutingAssembly().GetName().Version;
			sp = new SPWrapper (LaunchpadPaths.SpaceportPath);
			watcher = new DeviceWatcher (sp);
			spc = new SpaceportController (this, settings, watcher, version);
			pc = new ProjectMenuController (this, sp, settings);
			deployer = new DeployListener (this, sp, settings, watcher);
			installer = new InstallListener (this, sp, settings);
		}

		public void Dispose()
		{
			sp.Dispose();
			spc.Dispose();
			SaveSettings();
		}

		private Version version;
		private SPWrapper sp;
		private SpaceportController spc;
		private ProjectMenuController pc;
		private DeviceWatcher watcher;
		private Settings settings;
		private DeployListener deployer;
		private InstallListener installer;
		private ILog logger;
		private bool isEnabled;

		private void ProjectChanged (DataEvent e)
		{
			var proj = e.Data as AS3Project;
			if (proj != null) {
				TraceManager.AddAsync ("Spaceport switching to new project at " + proj.Directory);
				sp.ProjectDirectory = proj.Directory;
				EnablePlugin (enabled:true);
				EnableProject (enabled:true);
			} else {
				EnablePlugin (enabled:false);
				EnableProject (enabled:false);
			}
		}

		private void EnableProject (bool enabled)
		{
			var eventType = enabled
				? SPPluginEvents.ProjectOpened
				: SPPluginEvents.ProjectClosed;
			var dataEvent = new DataEvent (EventType.Command,
				eventType, enabled);
			EventManager.DispatchEvent (this, dataEvent);
		}

		private void EnablePlugin (bool enabled)
		{
			if (isEnabled != enabled) { // Ignore duplicate statuses, review
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

			var path = LaunchpadPaths.SettingsPath;
			if (!File.Exists (path)) {
				SaveSettings();
			} else {
				try {
					settings = (Settings) ObjectSerializer
						.Deserialize (path, settings);
				} catch (Exception ex) {
					ErrorManager.ShowError ("Launchpad failed to load settings, erasing settings", ex);
					SaveSettings();
				}
			}
		}

		private void SaveSettings()
		{
			var path = LaunchpadPaths.SettingsPath;
			LaunchpadPaths.CreateParentFolder (path);
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
