using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using PluginCore;

namespace PluginCommon
{
	public static class PluginHelper
	{
		public static T CheckPluginLoaded<T> (string guid)
			where T : class, IPlugin
		{
			T plugin = (T)PluginBase.MainForm.FindPlugin (guid);
			if (plugin == null)
				throw new InvalidOperationException ("The primary spaceport plugin was not loaded.");

			return plugin;
		}
	}
}
