using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Launchpad.Helpers;
using PluginCore.Managers;
using UpdaterCore;
using log4net;

namespace Launchpad
{
	public class UpdaterHookController
	{
		public static void Attach (UpdaterHook updater,
			Control mainForm, SpaceportMenu menu, ILog logger)
		{
			UpdaterHookController.mainForm = mainForm;
			UpdaterHookController.menu = menu;
			UpdaterHookController.logger = logger;

			updater.UpdateRunner.CheckUpdateStarted += onRunnerStarted;
			updater.UpdateRunner.CheckUpdateStopped += onRunnerStopped;
			updater.UpdateRunner.CheckUpdateFailed += onUpdateFailed;
			updater.UpdateRunner.UpdateFound += onUpdateFound;
		}

		private static Control mainForm;
		private static ILog logger;
		private static SpaceportMenu menu;

		private static void onUpdateFound (object s, UpdateCheckerEventArgs ev)
		{
			logger.Info ("Update found with version v" + ev.Version);
			TraceManager.AddAsync ("Update found with version v" + ev.Version); 
			mainForm.Invoke (new Action (() => menu.SetUpdateEnabled (true)));
		}

		private static void onUpdateFailed (object s, UpdateCheckerEventArgs ev)
		{
			logger.Error ("Failed to get update from " +
				ev.CheckLocation,
				ev.Exception);
		}

		private static void onRunnerStarted(object s, EventArgs ev)
		{
			TraceHelper.TraceInfo ("Started checking for updates automatically");
			logger.Info ("Spaceport updater runner started");
		}

		private static void onRunnerStopped (object s, EventArgs ev)
		{
			logger.Info ("Spaceport updater runner stopped");
		}
	}
}
