﻿using System;
using System.IO;
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
			EventManager.AddEventHandler (this, EventType.Command);
			Log4NetHelper.ConfigureFromXML (Resources.log4net);
			SubDataEvent (ProjectManagerEvents.Project, ProjectChanged);
			LoadSettings ();

			sp = new SPWrapper (LaunchpadPaths.SpaceportPath);
			logger = LogManager.GetLogger (typeof (LaunchPad));
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

			var path = LaunchpadPaths.SettingsPath;
			if (!File.Exists (path)) {
				SaveSettings();
			} else {
				settings = (Settings) ObjectSerializer
					.Deserialize (path, settings);
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
