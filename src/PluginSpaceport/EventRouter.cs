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
			if (!subs.ContainsKey (action)) {
				subs.Add (action, new List<Action<DataEvent>> ());
			}
			subs[action].Add (callback);
		}

		public void UnsubDataEvent (string action, Action<DataEvent> callback)
		{
			if (subs.ContainsKey (action))
				subs[action].Remove (callback);
		}

		public void HandleEvent (object sender, NotifyEvent e,
			HandlingPriority priority)
		{
			var de = e as DataEvent;

			// If it's a DE event with subs for the DE's action
			if (de != null && subs.ContainsKey (de.Action)) {
				subs[de.Action].ForEach (i => i (de));
			}	
		}

		private Dictionary<string, List<Action<DataEvent>>> subs
			= new Dictionary<string, List<Action<DataEvent>>>();
	}
}
