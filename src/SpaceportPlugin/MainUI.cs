using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace PluginSpaceport
{
	public partial class MainUI : UserControl
	{
		public MainUI()
		{
			InitializeComponent ();
		}

		private void btnMystery_Click(object sender, EventArgs e)
		{
			Process.Start ("http://spaceport.io");
		}
	}
}
