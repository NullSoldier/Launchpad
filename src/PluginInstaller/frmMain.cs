using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PluginInstaller.Properties;

namespace PluginInstaller
{
	public partial class frmMain : Form
	{
		public frmMain()
		{
			InitializeComponent();

			this.Icon = Icon.FromHandle (Resources.icon.GetHicon());
		}

		private void frmMain_Load(object sender, EventArgs e)
		{

		}
	}
}
