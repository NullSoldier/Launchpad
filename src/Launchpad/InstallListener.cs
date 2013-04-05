using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Launchpad.Forms;
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
			form.FinishInstall ();
			
			if (code != 0) {
				while (lastError == null)
					Thread.Sleep (1);

				form.LogFatal ("Install to device failed with exit code "
					+ code
					+ Environment.NewLine
					+ lastError);
			}
		}

		private void onStarted (DevicePlatform platform)
		{
			started = true;
			lastError = null;
			installProcess = sp.InstallToDevice (
				platform,
				onOutput,
				onError,
				onExited);
		}

		private void onStopped()
		{
			installProcess.Kill();
			started = false;
		}
	}
}
