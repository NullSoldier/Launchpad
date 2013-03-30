using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Launchpad.Forms
{
	public partial class FileSelector : UserControl
	{
		public FileSelector()
		{
			InitializeComponent();
		}

		public string UseRelativePathTo;

		public TextBox inPath
		{
			get { return inFile; }
		}

		public string Path
		{
			get { return inFile.Text; }
			set { inFile.Text = value; }
		}

		private void btnBrowse_Click (object sender, EventArgs e)
		{
			var dialog = new FolderBrowserDialog();
			var result = dialog.ShowDialog (Parent);

			if (result == DialogResult.OK) {
				string relativePath = null;
				var gotRelative = UseRelativePathTo != null && FileHelper.GetRelative (
					UseRelativePathTo, dialog.SelectedPath, ref relativePath);

				Path = gotRelative ? relativePath : dialog.SelectedPath;
			}
		}
	}
}
