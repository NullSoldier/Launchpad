using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PluginSpaceport.Observable;

namespace PluginSpaceport
{
	public partial class frmDeployTargets : Form, IObserver<Target>
	{
		public frmDeployTargets (SPDeviceWatcher watcher, Settings settings)
		{
			InitializeComponent();

			this.settings = settings;
			this.watcher = watcher;
		}

		private Settings settings;
		private SPDeviceWatcher watcher;
		private IDisposable unsub;

		private void LoadingForm (object s, EventArgs e)
		{
			NotifyAdded (new Target ("My Simulator", DevicePlatform.Sim));
			unsub = watcher.Subscribe (this);

			listTargets.ItemChecked += DeviceChecked;
		}

		private void ClosingForm (object s, FormClosingEventArgs e)
		{
			unsub.Dispose();
		}

		private void DeviceChecked (object sender, ItemCheckedEventArgs e)
		{
			if (e.Item.Checked) {
				settings.DeviceTargets.Add (e.Item.Text);
			} else {
				settings.DeviceTargets.Remove (e.Item.Text);
			}
		}

		public void NotifyAdded (Target t)
		{
			Invoke ((MethodInvoker) (() =>
			{
				var i = new ListViewItem (new[]
				{
					t.Name,
					t.PlatformName
				});
				i.Name = t.Name;
				i.Checked = settings.DeviceTargets.Contains (t.Name);

				listTargets.Items.Add (i);
			}));
		}

		public void NotifyRemoved (Target target)
		{
			Invoke ((MethodInvoker) (() => {
				listTargets.Items.RemoveByKey (target.Name);
			}));
		}

		public void OnComplete()
		{
			throw new NotImplementedException ();
		}

		public void OnError()
		{
			throw new NotImplementedException ();
		}
	}
}
