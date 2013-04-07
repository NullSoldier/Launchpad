using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Launchpad.Forms;
using Launchpad.Helpers;
using PluginCore;
using UpdaterCore;

namespace Launchpad
{
	public class InstallListener
	{
		public InstallListener (
			EventRouter events, SPWrapper sp,
			Settings settings)
		{
			this.events = events;
			this.sp = sp;
			this.settings = settings;

			//TODO: convert to Find Parent Form
			mainForm = PluginBase.MainForm.MenuStrip.Parent.Parent;
			events.SubDataEvent (SPPluginEvents.StartInstall, onInstall);
		}

		private readonly EventRouter events;
		private readonly SPWrapper sp;
		private readonly Settings settings;
		private readonly Control mainForm;

		private bool started = false;
		private Process installProcess;
		private frmInstall form;
		private string lastError;
		private DevicePlatform installToPlatform;

		private void onInstall (DataEvent de)
		{
			if (started)
				throw new InvalidOperationException();

			form = new frmInstall (onStarted, onStopped);
			form.ShowDialog (mainForm);
		}

		private void onOutput (string o)
		{
			form.LogMessage (o);
		}

		private void onError (string o)
		{
			form.LogMessage (o);
			if (o != null) {
				lastError = o;
			}
		}

		private void onExited (int code, Process p)
		{
			started = false;
			form.FinishInstall();
			
			if (code != 0)
			{
				var error = installToPlatform == DevicePlatform.iOS
					? getDefaultError (lastError, code)
					: parseAndroidError (lastError, code);
				form.LogFatal (error);
			}
		}

		private void onStarted (DevicePlatform platform)
		{
			started = true;
			lastError = null;
			installToPlatform = platform;
			installProcess = sp.InstallToDevice (
				platform,
				onOutput,
				onError,
				onExited);
			TraceHelper.TraceProcessStart ("spaceport install", installProcess);
		}

		private void onStopped()
		{
			installProcess.Kill();
			started = false;
		}

		private string parseAndroidError (string error, int exitCode)
		{
			if (error == null) {
				return getDefaultError (error, exitCode);
			}
			if (error.Contains ("[INSTALL_PARSE_FAILED_INCONSISTENT_CERTIFICATES]"))
			{
				return "The project has changed in an unexpected way. "
				+ "Uninstall your project from Android first, and then "
				+ "try installing again.";
			}
			return getDefaultError (error, exitCode);
		}

		private string getDefaultError (string error, int exitCode)
		{
			var msg = "Install to device failed with exit code " + exitCode;
			if (error != null)
				msg += Environment.NewLine + error;
			return msg;
		}
	}
}
