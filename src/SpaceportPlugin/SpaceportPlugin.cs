using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PluginCore;
using PluginCore.Helpers;
using PluginCore.Managers;

namespace SpaceportPlugin
{
	public class SpaceportPlugin : IPlugin
	{
		private const string guid = "3a015ec0-53f3-493a-a5f7-07a55bb93b64";
		private const string name = "Spaceport Plugin";
		private const string help = "http://spaceport.io";
		private const string author = "Jason (Null) Spafford";
		private const string description = "A spaceport IDE plugin for Flashbuilder";
		private const int apiLevel = 1;
		private object settingsObject;

		public void Initialize()
		{
			TraceManager.AddAsync ("Starting Spaceport Plugin");
			EventManager.AddEventHandler (this, EventType.FileSave);

			HookIntoMenu();
		}

		public void Dispose()
		{
			TraceManager.AddAsync ("Destroying Spaceport Plugin");
		}

		public void HandleEvent(object sender, NotifyEvent e, HandlingPriority priority)
		{
			if (e.Type == EventType.FileSave)
				TraceManager.AddAsync ("Spaceport Plugin detected file saved " + e);
		}

		private void HookIntoMenu()
		{
			ToolStripItem spaceportMenu = new ToolStripButton("Spaceport");

			PluginBase.MainForm.MenuStrip.Items.Add (spaceportMenu);
		}

		#region Required Properties
		public int Api
		{
			get { return apiLevel; }
		}

		public string Name
		{
			get { return name; }
		}

		public string Guid
		{
			get { return guid; }
		}

		public string Help
		{
			get { return help; }
		}

		public string Author
		{
			get { return author; }
		}

		public string Description
		{
			get { return description; }
		}

		public object Settings
		{
			get { return settingsObject; }
		}
		#endregion
	}
}
