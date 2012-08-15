using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;
using PluginCore;
using PluginCore.Managers;
using WeifenLuo.WinFormsUI.Docking;

namespace SpaceportUpdaterPlugin
{
	public class PluginMain : IPlugin
	{
		private const string guid = "7b05efcc-d6e8-49c4-85b9-85ae9e22ead9";
		private const string name = "Spaceport Updater Plugin";
		private const string help = "http://spaceport.io";
		private const string author = "Jason (Null) Spafford";
		private const string description = "A spaceport IDE plugin to check for, and update the Spaceport plugin.";
		private const int apiLevel = 1;
		private object settingsObject;

		public void Initialize()
		{
			TraceManager.AddAsync ("Starting Spaceport Plugin v0.00001");

			HookIntoMenu();
		}

		public void Dispose()
		{
			TraceManager.AddAsync ("Destroying Spaceport Updater Plugin");
		}

		public void HandleEvent(object sender, NotifyEvent e, HandlingPriority priority)
		{
			throw new NotImplementedException();
		}

		private void HookIntoMenu()
		{
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
