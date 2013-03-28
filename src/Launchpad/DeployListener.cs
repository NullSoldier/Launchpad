
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Launchpad.Helpers;
using PluginCore;
using UpdaterCore;

namespace Launchpad
{
	public class DeployListener
	{
		public DeployListener (
			EventRouter events, SPWrapper sp,
			Settings settings, DeviceWatcher watcher)
		{
			this.sp = sp;
			this.settings = settings;
			this.watcher = watcher;
			
			events.SubDataEvent (SPPluginEvents.StartDeploy, de => startDeploy());
			events.SubDataEvent (SPPluginEvents.StartBuild, de => startBuild());
		}

		private readonly DeviceWatcher watcher;
		private readonly Settings settings;
		private readonly SPWrapper sp;
		private Process build;
		private Dictionary<int, Process> pushes = new Dictionary<int, Process>(); 

		private void startDeploy()
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
		
		private void startPush (IEnumerable<Target> targets)
		{
			cancelPushes();

			var p = sp.RunOnTargets (targets,
				processPushOutput,
				TraceHelper.TraceError,
				(exitCode, process) => {
					if (exitCode != 0) {
						TraceHelper.TraceError ("Push process ("+process.Id+") terminated for with exit code " + exitCode);
					}
					pushes.Remove (process.Id);
				});
			if (p != null) {
				pushes.Add (p.Id, p);
				TraceHelper.Trace ("Deploy to devices " + targets.Count() + " process ("+p.Id+") started", TraceType.ProcessStart);
			} else {
				TraceHelper.TraceError ("Push to devices failed");
			}
		}

		private void startBuild ()
		{
			var target = this.watcher.Active
				.Intersect (this.settings.DeviceTargets)
				.FirstOrDefault(t => t.Platform != DevicePlatform.FlashPlayer);

			if (target != null || settings.DeploySim)
				buildProject (null);
		}

		private void buildProject (Action finished)
		{
			cancelBuild();
			
			build = sp.Build (
				TraceHelper.TraceInfo,
				processBuildOutput,
				(exitCode, process) => processBuildResult (exitCode, finished));

			TraceHelper.Trace ("Build process ("+build.Id+") started.", TraceType.ProcessStart);
		}

		private void processBuildResult (int exitCode, Action success)
		{
			switch (exitCode) {
				case 0:
					if (success != null)
						success();
					break;
				default:
					TraceHelper.TraceError ("Failed to build with exit code " + exitCode);
					break;
			}
		}

		private Regex errorRegex = new Regex ("(?<file>.*?):(?<line>[0-9]*):(?<col>[0-9]*):\\s?(?<type>(Error)?(Warning)?):(?<msg>.*\t?)", RegexOptions.Compiled);
		private void processPushOutput (String o)
		{
			if (o != null && !o.StartsWith ("Javascript:"))
				TraceHelper.TraceInfo (o);
		}

		private void processBuildOutput (String o)
		{
			if (o == null)
				return;

			var traceType = errorRegex.IsMatch(o) 
				? TraceType.Error : TraceType.Info;
			TraceHelper.Trace (o, traceType);
		}

		private void cancelBuild()
		{
			var buildRunning = build != null && !build.HasExited;
			if (buildRunning) {
				TraceHelper.TraceInfo ("Canceling previous build request");
				build.Kill();
				build = null;
			}
		}

		private void cancelPushes()
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
