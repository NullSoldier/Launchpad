using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using LaunchPad.Helpers;
using LaunchPad.Properties;
using log4net;
using PluginCore;
using PluginCore.Helpers;
using PluginCore.Managers;
using PluginCore.Utilities;
using ProjectManager;
using ProjectManager.Projects.AS3;

namespace LaunchPad
{
	public class Launchpad : EventRouter, IPlugin
	{
		public static Analytics A { get; private set; }

		public void Initialize()
		{
			AppDomain.CurrentDomain.UnhandledException += (s, e) => {
				var ex = e.ExceptionObject as Exception;
				logger.Fatal (ex.Message, ex);
			};

			EventManager.AddEventHandler (this, EventType.Command);
			Log4NetHelper.ConfigureFromXML (Resources.log4net);
			SubDataEvent (SPPluginEvents.Enabled, OnPluginEnabled);
			SubDataEvent (SPPluginEvents.Disabled, OnPluginDisabled);
			SubDataEvent (ProjectManagerEvents.Project, OnProjectChanged);
			LoadSettings();

			logger = LogManager.GetLogger (typeof (Launchpad));
			version = Assembly.GetExecutingAssembly ().GetName ().Version;
			A = new Analytics (version);
			sp = new SPWrapper (LaunchpadPaths.SpaceportPath);
			watcher = new DeviceWatcher (sp);

			var mainForm = PluginBase.MainForm.MenuStrip.Parent.Parent;
			var menu = new SpaceportMenu (PluginBase.MainForm.MenuStrip);

			pc = new ProjectMenuController (this, sp, settings);
			uc = new UpdaterController (menu, mainForm, version, settings);
			spc = new SpaceportMenuController (menu, mainForm, version,
				this, settings, watcher);
			
			DeployListener.Listen (this, sp, settings, watcher);
			InstallListener.Listen (this, sp, settings);

			A.OnStarted();
		}

		public void Dispose()
		{
			watcher.Stop();
			uc.Dispose();
			sp.Dispose();
			SaveSettings();
		}

		private Version version;
		private SPWrapper sp;
		
		private UpdaterController uc;
		private SpaceportMenuController spc;
		private ProjectMenuController pc;

		private DeviceWatcher watcher;
		private Settings settings;
		private ILog logger;
		private bool isEnabled;

		private void EnableProject (bool enabled)
		{
			var eventType = enabled
				? SPPluginEvents.ProjectOpened
				: SPPluginEvents.ProjectClosed;
			var dataEvent = new DataEvent (EventType.Command,
				eventType, null);
			EventManager.DispatchEvent (this, dataEvent);
		}

		private void EnablePlugin (bool enabled)
		{
			// Ignore duplicate statuses, review
			if (isEnabled == enabled)
				return;

			var eventType = enabled
				? SPPluginEvents.Enabled
				: SPPluginEvents.Disabled;
			var dataEvent = new DataEvent (EventType.Command,
				eventType, null);

			isEnabled = enabled;
			TraceManager.AddAsync ((enabled ? "Enabling" : "Disabling") + " Spaceport plugin");
			EventManager.DispatchEvent (this, dataEvent);
		}

		private void OnProjectChanged(DataEvent e)
		{
			var proj = e.Data as AS3Project;
			if (proj != null)
			{
				TraceManager.AddAsync ("Spaceport switching to new project at " + proj.Directory);
				sp.ProjectDirectory = proj.Directory;
				EnablePlugin (enabled:true);
				EnableProject (enabled:true);
			} else {
				EnablePlugin (enabled:false);
				EnableProject (enabled:false);
			}
		}

		private void OnPluginEnabled (DataEvent e)
		{
			SubDataEvent (ProjectManagerEvents.TestProject, OnTestProject);
			SubDataEvent (ProjectManagerEvents.BuildProject, OnBuildProject);
			watcher.Start ();
		}

		private void OnPluginDisabled (DataEvent e)
		{
			UnsubDataEvent (ProjectManagerEvents.TestProject, OnTestProject);
			UnsubDataEvent (ProjectManagerEvents.BuildProject, OnBuildProject);
			watcher.Stop ();
		}

		private void OnTestProject (DataEvent e)
		{
			var clearEvent = new DataEvent (EventType.Command, "ResultsPanel.ClearResults", null);
			var deployEvent = new DataEvent (EventType.Command, SPPluginEvents.StartDeploy, null);
			EventManager.DispatchEvent (this, clearEvent);
			EventManager.DispatchEvent (this, deployEvent);

			if (!settings.DeployDefault)
				e.Handled = true;
		}

		private void OnBuildProject (DataEvent e)
		{
			var clearEvent = new DataEvent (EventType.Command, "ResultsPanel.ClearResults", null);
			var buildEvent = new DataEvent (EventType.Command, SPPluginEvents.StartBuild, null);
			EventManager.DispatchEvent (this, clearEvent);
			EventManager.DispatchEvent (this, buildEvent);

			if (!settings.DeployDefault)
				e.Handled = true;
		}

		private void LoadSettings()
		{
			settings = new Settings();
			settings.SettingsVersion = version;

			var path = LaunchpadPaths.SettingsPath;
			if (!File.Exists (path)) {
				SaveSettings();
			} else {
				try {
					settings = SettingsUpgrader.Convert (ObjectSerializer
						.Deserialize (path, settings));
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
