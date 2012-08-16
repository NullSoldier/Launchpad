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
		private int fullHeight;

		private void frmPatch_Load(object sender, EventArgs e)
		{
			fullHeight = Size.Height;
			MinimumSize = new Size (MinimumSize.Width, fullHeight - inNotes.Height); 

			this.HidePatchNotes();
		}

		private void HidePatchNotes()
		{
			this.inNotes.Visible = false;
			this.Size = new Size (Size.Width, fullHeight - inNotes.Size.Height);
			this.MaximumSize = this.Size;
			this.SizeGripStyle = SizeGripStyle.Hide;

			this.lnkNotes.Text = "View patch notes...";
		}

		private void ShowPatchNotes()
		{
			inNotes.Visible = true;
			MaximumSize = Size.Empty;
			Size = new Size (Size.Width, fullHeight);
			SizeGripStyle = SizeGripStyle.Show;

			this.lnkNotes.Text = "Hide patch notes...";
		}

		private void lnkNotes_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (inNotes.Visible)
				HidePatchNotes();
			else
				ShowPatchNotes();
		}

		private void frmPatch_Resize(object sender, EventArgs e)
		{
			if (inNotes.Visible)
				fullHeight = Size.Height;
		}
	}
}
