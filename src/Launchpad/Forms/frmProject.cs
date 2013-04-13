using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using LaunchPad.Config;
using LaunchPad.Helpers;
using LaunchPad.Properties;
using UpdaterCore;
using Orientation = LaunchPad.Config.Orientation;

namespace LaunchPad.Forms
{
	public partial class frmProject : Form
	{
		public frmProject (SPWrapper sp)
		{
			Check.ArgNull (sp, "sp");
			this.sp = sp;
			InitializeComponent();

			configToFields = new Dictionary<string, Setting> {
				//BOTH
				{"id",					new TextSetting (inID)},
				{"display_name",		new TextSetting (inDisplayName)},
				{"authorization_key",	new TextSetting (inAuthKey)},
				{"version",				new TextSetting (inVersion)},
				{"loading_screen_file", new TextSetting (inLoading)},
				{"url_schemes",			new TextSetting (inURLSchemes)},
				{"orientations",		new ComboSetting<Orientation> (cmbOrientation,
					OrientationSerialize.fromOrientation, 
					OrientationSerialize.toOrientation)},
				// IOS
				//{"ios_device_families",	new ComboSetting (cmbDeviceFamily)},
				{"ios_resources",		new FileSetting (fileResourcesiOS)},
				{"ios_dev_identity",	new FileSetting (fileIdentity)},
				{"ios_add_icon_gloss",	new BoolSetting (inGlassEffect)},
				{"ios_dev_mobile_provision_file", new FileSetting (fileProvision)},
				{"ios_app_runs_in_background", new BoolSetting  (inBackground)},
				{"ios_device_families", new ComboSetting<DeviceFamily> (cmbFamiliesiOS,
					FamilySerialize.fromFamily,
					FamilySerialize.toFamily)},
				// ANDROID
				{"version_code",		new TextSetting (inVersionAndroid)},
				{"keystore_file",		new FileSetting (fileKeystore)},
				{"keystore_password_file",new FileSetting (fileKeystorePassword)},
				{"key_password_file",	new FileSetting (fileKeyPassword)},
				{"android_resources", new FileSetting (fileResourcesAndroid)}
			};
			fieldsToConfig = configToFields.ToDictionary (p => p.Value, p => p.Key);
		}

		private readonly SPWrapper sp;
		private readonly Dictionary<string, Setting> configToFields;
		private readonly Dictionary<Setting, string> fieldsToConfig;
		private readonly Dictionary<Setting, bool> changedFields = new Dictionary<Setting, bool>();
		
		private void onLoaded (object s, EventArgs ev)
		{
			Launchpad.A.PropertiesOpened();
			Icon = Icon.FromHandle (Resources.spaceportIcon.GetHicon ());

			var config = loadConfig();
			if (config != null)
				displayConfig (config);

			foreach (var kvp in fieldsToConfig) {
				kvp.Key.Changed = () => {
					if (!changedFields.ContainsKey (kvp.Key))
						changedFields.Add (kvp.Key, true);
				};
				if (kvp.Key.Type == SettingType.File)
					((FileSelector)kvp.Key.Control).UseRelativePathTo = sp.ProjectDirectory;
			}
		}

		private void btnSave_Click (object s, EventArgs ev)
		{
			string error;
			if (trySaveConfig (out error)) {
				Close();
			} else {
				MessageBox.Show ("Failed to save configuration:"
					+ Environment.NewLine
					+ error);
			}
		}

		private Dictionary<string, string> loadConfig()
		{
			IEnumerable<string> lines;
			if (!sp.TryGetOutput ("config --project --show", out lines)) {
				MessageBox.Show ("Failed to load configuration:"
					+ Environment.NewLine
					+ String.Join (Environment.NewLine, lines.ToArray()));
				return null;
			}
			var config = new Dictionary<string, string>();
			foreach (var line in lines) {
				var parts = line.Split (" = ");
				config.Add (parts[0], parts[1]);
			}
			return config;
		}

		private bool trySaveConfig (out string error)
		{
			foreach (Setting tup in changedFields.Keys) {
				var key = fieldsToConfig [tup];
				var value = tup.Value;

				var cmd = String.Format ("config --project {0} {1}", key, value);
				if (!sp.TryExecuteCmd (cmd, out error))
					return false;
			}
			changedFields.Clear();
			error = null;
			return true;
		}

		private void displayConfig (Dictionary<string, string> config)
		{
			foreach (var kvp in fieldsToConfig) {
				var hasKey = config.ContainsKey (kvp.Value);
				kvp.Key.Value = hasKey ? config [kvp.Value] : "";
			}
		}
	}
}
