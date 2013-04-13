using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using LaunchPad.Properties;
using log4net;
using log4net.Config;
using log4net.Core;

namespace LaunchPad.Helpers
{
	public static class Log4NetHelper
	{
		public static void ConfigureFromXML (string xml)
		{
			var xd = new XmlDocument();
			xd.LoadXml (xml);

			XmlConfigurator.Configure (xd.DocumentElement);
		}
	}
}
