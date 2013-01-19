using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Timers;
using PluginSpaceport.Observable;

namespace PluginSpaceport
{
	public class SPDeviceWatcher
		: IObservable<Target>
	{
		public SPDeviceWatcher (PushWrapper push)
		{
			this.push = push;

			timer = new Timer (2000);
			timer.AutoReset = true;
			timer.Elapsed += (s, ev) =>
				push.GetDevicesNames (ProcessDevices);
		}

		public IDisposable Subscribe (IObserver<Target> o)
		{
			subs.Add (o);
			foreach (var t in active) {
				o.NotifyAdded (t);
			}

			return new Unsubscriber<Target> (subs, o);
		}

		public void Start ()
		{
			timer.Start();
		}
		
		public void Stop()
		{
			timer.Stop();
		}

		private readonly Timer timer;
		private readonly PushWrapper push;
		private readonly List<Target> active = new List<Target>();
		private readonly List<IObserver<Target>> subs = new List<IObserver<Target>> ();

		//TODO: use hashet and implement hash on target by using the name
		private void ProcessDevices(IEnumerable<Target> devices)
		{
			var current = new List<Target> (active);

			foreach (var d in devices) {
				// A new device was found
				if (!current.Contains (d))
				{
					active.Add (d);
					subs.ForEach (s => s.NotifyAdded (d));
				}
				current.Remove (d);
			}

			// Only items left are things that were removed
			foreach (var d in current) {
				active.Remove (d);
				subs.ForEach (s => s.NotifyRemoved (d));
			}
		}
	}
}
