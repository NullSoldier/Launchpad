using System;
using System.Collections.Generic;
using System.Resources;
using System.Text;

namespace UpdaterCore.Update
{
	public class UpdateInformation
	{
		public UpdateInformation (
			UpdateManifest manifest,
			Uri patchZipUri,
			Uri updaterUri,
			string patchNotes)
		{
			Manifest = manifest;
			PatchZipUri = patchZipUri;
			UpdaterUri = updaterUri;
			PatchNotes = patchNotes;

		}

		public readonly UpdateManifest Manifest;
		public readonly Uri PatchZipUri;
		public readonly Uri UpdaterUri;
		public readonly string PatchNotes;
	}
}
