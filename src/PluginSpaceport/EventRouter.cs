using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using InstallerCore;
using InstallerCore.Rollback;
using PluginCore;
using PluginCore.Managers;
using ProjectManager.Projects;

namespace PluginSpaceport
{
	public class EventRouter : IEventHandler
	{
		public void SubDataEvent (string action, Action<DataEvent> callback)
		{
			if (!deSubs.ContainsKey (action)) {
				deSubs.Add (action, new List<Action<DataEvent>> ());
			}
			deSubs[action].Add (callback);
		}

		public void HandleEvent (object sender, NotifyEvent e,
			HandlingPriority priority)
		{
			var de = e as DataEvent;

			// If it's a DE event with subs for the DE's action
			if (de != null && deSubs.ContainsKey (de.Action)) {
				deSubs[de.Action].ForEach (i => i (de));
			}	
		}

		private Dictionary<string, List<Action<DataEvent>>> deSubs
			= new Dictionary<string, List<Action<DataEvent>>>();
	}
}
