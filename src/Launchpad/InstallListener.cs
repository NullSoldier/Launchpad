using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using LaunchPad.Forms;
using LaunchPad.Helpers;
using PluginCore;
using UpdaterCore;

namespace LaunchPad
{
	public static class InstallListener
	{
		public static void Listen(
			EventRouter events, SPWrapper sp,
			Settings settings)
		{
			InstallListener.events = events;
			InstallListener.sp = sp;
			InstallListener.settings = settings;

			//TODO: convert to Find Parent Form
			mainForm = PluginBase.MainForm.MenuStrip.Parent.Parent;
			events.SubDataEvent (SPPluginEvents.StartInstall, onInstall);
		}

		private static bool started = false;
		private static Process installProcess;
		private static frmInstall form;
		private static string lastError;
		private static DevicePlatform installToPlatform;

		private static EventRouter events;
		private static SPWrapper sp;
		private static Settings settings;
		private static Control mainForm;

		private static void onInstall (DataEvent de)
		{
			if (started)
				throw new InvalidOperationException();

			form = new frmInstall (onStarted, onStopped);
			form.ShowDialog (mainForm);
		}

		private static void onOutput (string o)
		{
			form.LogMessage (o);
		}

		private static void onError (string o)
		{
			form.LogMessage (o);
			if (o != null) {
				lastError = o;
			}
		}

		private static void onExited (int code, Process p)
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

		private static void onStarted (DevicePlatform platform)
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

		private static void onStopped()
		{
			installProcess.Kill();
			started = false;
		}

		private static string parseAndroidError (string error, int exitCode)
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

		private static string getDefaultError (string error, int exitCode)
		{
			var msg = "Install to device failed with exit code " + exitCode;
			if (error != null)
				msg += Environment.NewLine + error;
			return msg;
		}
	}
}
