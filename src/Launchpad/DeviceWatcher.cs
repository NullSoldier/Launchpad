using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Timers;
using Launchpad.Helpers;
using Launchpad.Observable;
using PluginCore;
using PluginCore.Managers;

namespace Launchpad
{
	public class DeviceWatcher
		: IObservable<Target>
	{
		public DeviceWatcher (SPWrapper sp)
		{
			this.sp = sp;

			timer = new Timer (2000);
			timer.AutoReset = true;
			timer.Elapsed += onTimerElapsed;
		}

		public readonly List<Target> Active = new List<Target>();

		public void Start()
		{
			timer.Start ();
		}

		public void Stop()
		{
			timer.Stop ();
		}

		public IDisposable Subscribe (IObserver<Target> o)
		{
			subs.Add (o);
			foreach (var t in Active) {
				o.NotifyAdded (t);
			}
			return new Unsubscriber<Target> (subs, o);
		}

		private Process listProcess;
		private readonly Timer timer;
		private readonly SPWrapper sp;
		private readonly List<IObserver<Target>> subs = new List<IObserver<Target>>();

		private void processDevice (Target target, DiscoveryStatus status)
		{
			switch (status) {
				case DiscoveryStatus.FOUND:
					Active.Add (target);
					subs.ForEach (s => s.NotifyAdded (target));
					break;
				case DiscoveryStatus.LOST:
					Active.Remove (target);
					subs.ForEach (s => s.NotifyRemoved (target));
					break;
			}
		}

		private void onTimerElapsed (object s, ElapsedEventArgs ev)
		{
			if (listProcess == null || listProcess.HasExited) {
				listProcess = startListProcess();
			}
		}

		private Process startListProcess()
		{
			Active.Clear();
			var p = sp.StartGetDevicesNames (processDevice);
			TraceHelper.TraceProcessStart ("device watcher", p);
			return p;
		}
	}
}
