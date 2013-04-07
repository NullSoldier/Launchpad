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
		public frmTargets (DeviceWatcher watcher, Settings settings)
		{
			InitializeComponent();

			this.settings = settings;
			this.watcher = watcher;
		}

		private Settings settings;
		private DeviceWatcher watcher;
		private IDisposable unsub;

		private void formLoaded (object s, EventArgs e)
		{
			if (!watcher.IsWorking) {
				lblError.Visible = true;
				lnkError.Visible = true;
				lblCloseNote.Visible = false;
			}
			
			var images = new ImageList ();
			images.Images.Add (DevicePlatform.Sim,			Resources.simIcon);
			images.Images.Add (DevicePlatform.FlashPlayer,	Resources.flashIcon);
			images.Images.Add (DevicePlatform.iOS,			Resources.appleIcon);
			images.Images.Add (DevicePlatform.Android,		Resources.androidIcon);
			listTargets.SmallImageList = images;

			addBuiltInTargets();

			// Subscribe to device added/removed notifications
			unsub = watcher.Subscribe (this);
			listTargets.ItemChecked += onTargetChecked;
		}

		private void onErrorClicked (object s, LinkLabelLinkClickedEventArgs e)
		{
			MessageBox.Show (this,
				watcher.LastError,
				"Error Getting Devices",
				MessageBoxButtons.OK,
				MessageBoxIcon.Error);
		}

		private void onTargetChecked (object s, ItemCheckedEventArgs e)
		{
			setTargetEnabled (e.Item.Tag as Target, e.Item.Checked);
		}

		private void ClosingForm (object s, FormClosingEventArgs e)
		{
			unsub.Dispose();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			Close ();
		}

		public void NotifyAdded (Target t)
		{
			Invoke ((MethodInvoker) (() =>
				addTargetItem (t, settings.DeviceTargets.Contains (t))));
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
		}

		private void setTargetEnabled(Target t, bool enabled)
		{
			switch (t.Platform)
			{
				case DevicePlatform.FlashPlayer:
					settings.DeployDefault = enabled;
					break;
				case DevicePlatform.Sim:
					settings.DeploySim = enabled;
					break;
				default:
					if (enabled) { settings.DeviceTargets.Add (t); }
					else { settings.DeviceTargets.Remove (t); }
					break;
			}
		}

		private void addTargetItem (Target t, bool isChecked)
		{
			var i = new ListViewItem (new[] {
				t.Name,
				t.Platform.GetString()
			});
			i.Name = t.ID;
			i.Tag = t;
			i.ImageKey = t.Platform.GetString();
			i.Checked = isChecked;
			listTargets.Items.Add (i);
		}

		private void addBuiltInTargets()
		{
			var f = new Target ("Flash Player", DevicePlatform.FlashPlayer);
			addTargetItem (f, settings.DeployDefault);

			var s = new Target ("sim", "Spaceport Simulator", DevicePlatform.Sim);
			addTargetItem (s, settings.DeploySim);
		}
	}
}