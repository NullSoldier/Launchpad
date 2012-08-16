using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SpaceportUpdaterPlugin
{
	public partial class frmPatch : Form
	{
		public frmPatch (UpdaterController controller)
		{
			InitializeComponent ();
		}

		private UpdaterController controller;

		private void frmPatch_Load(object sender, EventArgs e)
		{
			this.HidePatchNotes();
		}

		private void HidePatchNotes()
		{
			this.Size = new Size (this.Size.Width, this.inNotes.Bounds.Top);
		}

		private void ShowPatchNotes()
		{
			this.Size = new Size (this.Size.Width, this.inNotes.Bounds.Bottom);
		}
	}
}
