using System;
using System.Collections.Generic;
using System.Resources;
using System.Text;

namespace UpdaterCore.Update
{
	public class UpdateManifest
	{
		protected UpdateManifest (
			Version productVersion,
			Version updaterVersion)
		{
			ProductVersion = productVersion;
			UpdaterVersion = updaterVersion;
		}

		public readonly Version ProductVersion;
		public readonly Version UpdaterVersion;

		public static UpdateManifest ParseManifest(string manifest)
		{
			var lines = manifest.Split ('\n');
			Version pVersion = null;
			Version uVersion = null;

			foreach (var pair in lines)
			{
				var parts = pair.Split ("=");

				switch (parts[0])
				{
					case "product":
						pVersion = new Version (parts[1]);
						continue;
					case "updater":
						uVersion = new Version (parts[1]);
						continue;
				}
			}
			if (pVersion == null)
				throw new MissingManifestResourceException ("Missing product version");
			if (uVersion == null)
				throw new MissingManifestResourceException ("Missing updater version");

			return new UpdateManifest (pVersion, uVersion);
		}
	}
}
