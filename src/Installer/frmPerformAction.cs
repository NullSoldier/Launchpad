using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PluginInstaller
{
	public partial class frmPerformAction : Form
	{
		public frmPerformAction()
		{
			InitializeComponent();
		}

		public void SetInstruction (string text)
		{
			lblInstruction.Text = text;
		}

		public void EnableMarquee ()
		{
			progressBar.Style = ProgressBarStyle.Marquee;
		}

		public void SetProgress (float percentage)
		{
			progressBar.Style = ProgressBarStyle.Continuous;
			progressBar.Value = (int)percentage;
		}
	}
}
