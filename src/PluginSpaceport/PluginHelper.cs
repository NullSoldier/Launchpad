using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using PluginCore;

namespace PluginCommon
{
	public static class PluginHelper
	{
		public static bool TryGetLoadedPlugin<T> (string guid, out T loadedPlugin)
			where T : class, IPlugin
		{
			loadedPlugin = PluginBase.MainForm.FindPlugin (guid) as T;
			return loadedPlugin != null;
		}
	}
}
