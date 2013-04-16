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
			Uri patchZipURI,
			string patchNotes)
		{
			Manifest = manifest;
			PatchNotes = patchNotes;
			PatchZipURI = patchZipURI;
		}

		public readonly UpdateManifest Manifest;
		public readonly Uri PatchZipURI;
		public readonly string PatchNotes;
	}
}
