using System;
using System.Collections.Generic;
using System.Text;
using GaDotNet.Common;
using GaDotNet.Common.Data;
using GaDotNet.Common.Helpers;

namespace LaunchPad
{
	public class Analytics
	{
		public Analytics (Version version)
		{
			this.version = version;
			this.factory = new RequestFactory (ACCOUNT_CODE);
		}

		public void OnStarted()
		{
			ReportPageView (GetURL ("started"), "Launchpad OnStarted");
		}

		public void TargetScreenOpened()
		{
			ReportPageView (GetURL ("targets"), "Targets");
		}

		public void PropertiesOpened()
		{
			ReportPageView (GetURL ("projectproperties"), "Spaceport App Properties");
		}

		public void TargetErrorOpened()
		{
			ReportPageView (GetURL ("targeterror"), "Target Error");
		}

		public void UpdateWindowOpened()
		{
			ReportPageView (GetURL ("update"), "Update");
		}

		public void SpaceportWebsite()
		{
			ReportEvent ("UI", "Opened Spaceport Website");
		}

		public void AboutUs()
		{
			ReportEvent ("UI", "Opened About Us");
		}

		private readonly Version version;
		private readonly RequestFactory factory;
		private const string DOMAIN = "launchpadplugin.entitygames.net";
		private const string ACCOUNT_CODE = "UA-4040403-8";

		private void ReportPageView (string page, string title=null)
		{
			var pageView = new GooglePageView (title ?? page,
				DOMAIN, "/" + page);
			var request = factory.BuildRequest (pageView);
			GoogleTracking.FireTrackingEvent (request);
		}

		private void ReportEvent (string category, string action,
			string label=null, int? value=null)
		{
			var googleEvent = new GoogleEvent (DOMAIN, category,
				action, label, value);
			var request = factory.BuildRequest (googleEvent);
			GoogleTracking.FireTrackingEvent (request);
		}

		private string GetURL (string page)
		{
			return string.Format ("{0}/{1}", version, page);
		}
	}
}
