using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Launchpad.Helpers;
using Launchpad.Observable;
using Launchpad.Properties;

namespace Launchpad
{
	public partial class frmTargets : Form, IObserver<Target>
	{
		public frmTargets (SPDeviceWatcher watcher, Settings settings)
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
			var images = new ImageList ();
			images.Images.Add (DevicePlatform.Sim,			Resources.simIcon);
			images.Images.Add (DevicePlatform.FlashPlayer,	Resources.flashIcon);
			images.Images.Add (DevicePlatform.iOS,			Resources.appleIcon);
			images.Images.Add (DevicePlatform.Android,		Resources.androidIcon);
			listTargets.SmallImageList = images;
			
			unsub = watcher.Subscribe (this);
			listTargets.ItemChecked += DeviceChecked;
		}

		private void ClosingForm (object s, FormClosingEventArgs e)
		{
			unsub.Dispose();
		}

		private void DeviceChecked (object sender, ItemCheckedEventArgs e)
		{
			var t = (Target) e.Item.Tag;
			Check.IsNull (t);

			if (e.Item.Checked) {
				settings.DeviceTargets.Add (t);
			} else {
				settings.DeviceTargets.Remove (t);
			}
		}

		public void NotifyAdded (Target t)
		{
			Invoke ((MethodInvoker) (() =>
			{
				var i = new ListViewItem (new[]
				{
					t.Name,
					t.Platform.GetString()
				});
				i.Name = t.ID;
				i.Tag = t;
				i.Checked = settings.DeviceTargets.Contains (t);
				i.ImageKey = t.Platform.GetString();
				listTargets.Items.Add (i);
			}));
		}

		public void NotifyRemoved (Target target)
		{
			Invoke ((MethodInvoker) (() =>
				listTargets.Items.RemoveByKey (target.ID)));
		}

		public void OnComplete()
		{
			throw new NotImplementedException ();
		}

		public void OnError()
		{
			throw new NotImplementedException ();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
