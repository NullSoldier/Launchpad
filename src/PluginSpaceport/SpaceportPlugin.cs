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
				Exception ex = (Exception)args.ExceptionObject;
				logger.Fatal (ex.Message, ex);
			};

			//TODO: remove this, for testing
			sp.ProjectDirectory = @"C:\Users\Jason\Desktop\Projects\sample_games\city_game";
		}

		public void Dispose()
		{
			spc.Dispose ();
			SaveSettings ();
		}

		private SPWrapper sp;
		private SpaceportController spc;
		private ILog logger;
		private Settings settings;

		private readonly Version VERSION = new Version (0, 1);
		private readonly string SETTINGS_PATH = @"Spaceport\Settings.fdb";

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
