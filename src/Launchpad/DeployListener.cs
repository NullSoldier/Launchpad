
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using LaunchPad.Helpers;
using PluginCore;
using UpdaterCore;

namespace LaunchPad
{
	public static class DeployListener
	{
		public static void Listen (
			EventRouter events, SPWrapper sp,
			Settings settings, DeviceWatcher watcher)
		{
			DeployListener.sp = sp;
			DeployListener.settings = settings;
			DeployListener.watcher = watcher;
			
			events.SubDataEvent (SPPluginEvents.StartDeploy, de => startDeploy());
			events.SubDataEvent (SPPluginEvents.StartBuild, de => startBuild());
		}

		private static Process build;
		private static DeviceWatcher watcher;
		private static Settings settings;
		private static SPWrapper sp;
		private static Dictionary<int, Process> pushes = new Dictionary<int, Process> ();
		private static Regex errorRegex = new Regex ("(?<file>.*?):(?<line>[0-9]*):(?<col>[0-9]*):\\s?(?<type>(Error)?(Warning)?):(?<msg>.*\t?)", RegexOptions.Compiled);

		private static void startDeploy()
		{
			var targets = watcher.Active
				.Intersect (settings.DeviceTargets)
				.ConcatIf (settings.DeploySim, new Target ("sim", DevicePlatform.Sim))
				.Where (t => t.Platform != DevicePlatform.FlashPlayer)
				.ToArray();

			if (targets.Length > 0) {
				buildProject (() => startPush (targets));
			} else {
				TraceHelper.TraceError ("Failed to start spaceport app, no devices to push to found.");
			}
		}
		
		private static void startPush (IEnumerable<Target> targets)
		{
			cancelPushes();

			var p = sp.RunOnTargets (targets,
				onPushOutput,
				TraceHelper.TraceError,
				(exitCode, process) => {
					if (exitCode != 0) {
						TraceHelper.TraceProcessError ("Push", process);
					}
					pushes.Remove (process.Id);
				});
			// Did it start successfully?
			if (p != null) {
				pushes.Add (p.Id, p);
				TraceHelper.TraceProcessStart ("Deploy to devices " + targets.Count(), p);
			} else {
				TraceHelper.TraceError ("Push to devices failed");
			}
		}

		private static void startBuild ()
		{
			var target = watcher.Active
				.Intersect (settings.DeviceTargets)
				.FirstOrDefault(t => t.Platform != DevicePlatform.FlashPlayer);

			if (target != null || settings.DeploySim)
				buildProject (null);
		}

		private static void buildProject (Action finished)
		{
			cancelBuild();
			
			build = sp.Build (
				TraceHelper.TraceInfo,
				onBuildOutput,
				(exitCode, process) => onBuildResult (exitCode, finished));

			TraceHelper.TraceProcessStart ("Build", build);
		}

		private static void onBuildResult (int exitCode, Action success)
		{
			switch (exitCode) {
				case 0:
					TraceHelper.TraceInfo ("(sp)Build succeeded");
					if (success != null)
						success();
					break;
				default:
					TraceHelper.TraceError ("Failed to build with exit code " + exitCode);
					break;
			}
		}

		private static void onPushOutput (String o)
		{
			if (o != null && !o.StartsWith ("Javascript:"))
				TraceHelper.Trace (o, TraceType.Debug);
		}

		private static void onBuildOutput (String o)
		{
			if (o == null)
				return;

			var match = errorRegex.Match (o);
			if (match.Success) {
				var formatted = String.Format ("{0}({1}): col: {2} {3}:{4}",
					match.Groups["file"],
					match.Groups["line"],
					match.Groups["col"],
					match.Groups["type"],
					match.Groups["msg"]);
				TraceHelper.TraceError (formatted);
			} else {
				TraceHelper.TraceInfo (o);
			}
		}

		private static void cancelBuild()
		{
			var buildRunning = build != null && !build.HasExited;
			if (buildRunning) {
				TraceHelper.TraceInfo ("Canceling previous build request");
				build.Kill();
				build = null;
			}
		}

		private static void cancelPushes()
		{
			if (pushes.Count > 0)
				TraceHelper.TraceInfo ("Canceling previous push request(s)");

			while (pushes.Count > 0) {
				var p = pushes.Values.First();
				pushes.Remove (p.Id);
				p.Kill();
			}
		}
	}
}
