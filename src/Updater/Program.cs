using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Updater.Properties;
using log4net;
using log4net.Config;
using log4net.Core;
using log4net.Repository.Hierarchy;

namespace Updater
{
	public static class Program
	{
		[STAThread]
		public static void Main()
		{
			InitializeLogger();
			AppDomain.CurrentDomain.UnhandledException += onUnhandledException;

			InstallerArgsParser.Parse (Environment.GetCommandLineArgs(), new InstallerEntry());
		}

		private static void InitializeLogger()
		{
			var xmlDocument = new XmlDocument();
			xmlDocument.LoadXml (Resources.log4net);
			XmlConfigurator.Configure (xmlDocument.DocumentElement);
		}

		private static void onUnhandledException (object sender, UnhandledExceptionEventArgs ev)
		{
			Exception ex = (Exception)ev.ExceptionObject;
			LogManager.GetLogger (sender.GetType()).Fatal (ex.Message, ex);
		}
	}
}
